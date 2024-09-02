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

        public async Task<Result<string, Error>> UploadFile(
            CreateFileRequest fileData, 
            CancellationToken cancellationToken)
        {
            try
            {
                var bucketExistArgs = new BucketExistsArgs().WithBucket(fileData.BucketName);
                var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

                if (bucketExist == false)
                {
                    var makeBucketArgs = new MakeBucketArgs().WithBucket(fileData.BucketName);

                    await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                }

                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(fileData.BucketName)
                    .WithStreamData(fileData.Stream)
                    .WithObjectSize(fileData.Stream.Length)
                    .WithObject(Guid.NewGuid().ToString());

                var res = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

                return res.ObjectName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
        }

        public async Task<Result<string, Error>> DeleteFile(
            DeleteFileRequest fileData, 
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

        public async Task<Result<string, Error>> GetFile(GetFileRequest fileData, CancellationToken cancellationToken)
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
    }
}
