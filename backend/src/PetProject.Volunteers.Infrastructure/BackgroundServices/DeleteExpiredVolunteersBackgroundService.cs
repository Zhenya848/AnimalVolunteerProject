using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using PetProject.Core.Application.Repositories;
using PetProject.Volunteers.Application.Volunteers.Repositories;
using PetProject.Volunteers.Infrastructure.DbContexts;
using Constants = PetProject.Core.Constants;

namespace PetProject.Volunteers.Infrastructure.BackgroundServices;

public class DeleteExpiredVolunteersBackgroundService : BackgroundService
{
    private readonly ILogger<DeleteExpiredVolunteersBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public DeleteExpiredVolunteersBackgroundService(
        ILogger<DeleteExpiredVolunteersBackgroundService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DeleteExpiredVolunteersBackgroundService is starting.");
        
        while (stoppingToken.IsCancellationRequested == false)
        {
            using var scope = _scopeFactory.CreateScope();
            var writeDbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
            
            var volunteersToDelete = writeDbContext.Volunteers
                .Include(p => p.Pets)
                .Where(dt => dt.DeletionDate != null && DateTime.UtcNow >= dt.DeletionDate.Value
                    .AddDays(Constants.Volunteers.LIFETIME_AFTER_DELETION));

            writeDbContext.Volunteers.RemoveRange(volunteersToDelete);
            await writeDbContext.SaveChangesAsync(stoppingToken);

            await Task.Delay(
                TimeSpan.FromHours(Constants.Volunteers.DELETE_EXPIRED_VOLUNTEERS_SERVICE_REDUCTION_HOURS),
                stoppingToken);
        }
    }
}