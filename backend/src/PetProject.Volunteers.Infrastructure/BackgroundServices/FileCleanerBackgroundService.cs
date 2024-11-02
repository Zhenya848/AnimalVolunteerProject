using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetProject.Core.Application.Messaging;
using PetProject.Volunteers.Application.Files.Commands.Delete;
using PetProject.Volunteers.Application.Providers;
using FileInfo = PetProject.Volunteers.Application.Providers.FileInfo;

namespace PetProject.Volunteers.Infrastructure.BackgroundServices
{
    public class FileCleanerBackgroundService : BackgroundService
    {
        private readonly ILogger<FileCleanerBackgroundService> _logger;
        private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
        private readonly IServiceScopeFactory _scopeFactory;

        public FileCleanerBackgroundService(
            ILogger<FileCleanerBackgroundService> logger,
            IMessageQueue<IEnumerable<FileInfo>> messageQueue,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _messageQueue = messageQueue;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Files Cleaner Background Service is starting");

            await using var scope = _scopeFactory.CreateAsyncScope();
            var fileProvider = scope.ServiceProvider.GetRequiredService<IFileProvider>();

            while (stoppingToken.IsCancellationRequested == false)
            {
                var fileInfos = await _messageQueue.ReadAsync(stoppingToken);

                foreach (var fileInfo in fileInfos) 
                {
                    await fileProvider.DeleteFile(new DeleteFileCommand(fileInfo.BucketName, fileInfo.ObjectName), stoppingToken);
                }
            }

            await Task.Yield();
        }
    }
}
