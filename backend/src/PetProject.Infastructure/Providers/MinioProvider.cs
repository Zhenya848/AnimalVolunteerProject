using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetProject.Application.Files.Create;
using PetProject.Application.Files.Delete;
using PetProject.Application.Files.Get;
using PetProject.Application.Files.Providers;
using PetProject.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PetProject.Infastructure.Providers
{
    public class MinioProvider : IFileProvider
    {
        private readonly IMinioClient _minioClient;
        private readonly ILogger _logger;

        public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
        {
            _minioClient = minioClient;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyList<string>, Error>> UploadFiles(
            CreateFilesCommand request, 
            CancellationToken cancellationToken)
        {
            var semaphoreSlim = new SemaphoreSlim(5);

            try
            {
                var bucketExistArgs = new BucketExistsArgs().WithBucket(request.BucketName);
                var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

                if (bucketExist == false)
                {
                    var makeBucketArgs = new MakeBucketArgs().WithBucket(request.BucketName);

                    await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                }

                var fileTasks = request.Files.Select(
                    async f => await PutObject(f, semaphoreSlim, cancellationToken, request.BucketName));

                var result = await Task.WhenAll(fileTasks);

                if (result.Any(p => p.IsFailure))
                    return result.First().Error;

                return request.Files.Select(p => p.ObjectName).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
            finally { semaphoreSlim.Release(); }
        }

        public async Task<Result<string, Error>> DeleteFile(
            DeleteFileCommand fileData, 
            CancellationToken cancellationToken)
        {
            try
            {
                if (await IsFileExist(fileData.BucketName, fileData.ObjectName) == false)
                    return Error.NotFound("file.not.found", $"File with name {fileData.ObjectName} not found!");

                var removeFileArgs = new RemoveObjectArgs()
                    .WithBucket(fileData.BucketName)
                    .WithObject(fileData.ObjectName);

                await _minioClient.RemoveObjectAsync(removeFileArgs, cancellationToken);

                return fileData.ObjectName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Error.Failure("file.delete", "Fail to delete file in minio");
            }
        }

        public async Task<Result<string, Error>> GetFile(GetFileCommand fileData, CancellationToken cancellationToken)
        {
            try
            {
                if (await IsFileExist(fileData.BucketName, fileData.ObjectName) == false)
                    return Error.NotFound("file.not.found", $"File with name {fileData.ObjectName} not found!");

                var getFileArgs = new PresignedGetObjectArgs()
                    .WithBucket(fileData.BucketName)
                    .WithObject(fileData.ObjectName)
                    .WithExpiry(86400);

                return await _minioClient.PresignedGetObjectAsync(getFileArgs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Error.Failure("file.get", "Fail to get file from minio");
            }
        }

        private async Task<bool> IsFileExist(string bucketName, string objectName)
        {
            var fileExistArgs = new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);
            var fileExist = await _minioClient.StatObjectAsync(fileExistArgs);

            return fileExist.ContentType != null;
        }

        private async Task<Result<string, Error>> PutObject(
            FileData fileDto,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken,
            string bucketName)
        {
            await semaphoreSlim.WaitAsync(cancellationToken);

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithStreamData(fileDto.Stream)
                .WithObjectSize(fileDto.Stream.Length)
                .WithObject(fileDto.ObjectName);

            try
            {
                await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

                return fileDto.ObjectName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to upload file in minio with path {path} in bucket {bucket}",
                    fileDto.ObjectName,
                    bucketName);

                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
            finally { semaphoreSlim.Release(); }
        }
    }
}
