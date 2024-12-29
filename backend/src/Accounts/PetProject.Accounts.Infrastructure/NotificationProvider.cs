using CSharpFunctionalExtensions;
using Google.Protobuf.Collections;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using NotificationService;
using PetProject.Accounts.Application;
using PetProject.Core;

namespace PetProject.Infrastructure.Authentification;

public class NotificationProvider : INotificationProvider
{
    private readonly ILogger<NotificationProvider> _logger;

    public NotificationProvider(ILogger<NotificationProvider> logger)
    {
        _logger = logger;
    }
    
    public async Task<UnitResult<Error>> SendNotification(string message, string[] roles, string[] users)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(Constants.ServiceAdresses.NOTIFICATION_SERVICE_ADRESS);
            var client = new Greeter.GreeterClient(channel);

            var request = new SendNotificationRequest();
            
            request.Message = message;
            request.Roles.AddRange(roles);
            request.Users.AddRange(users);
            
            await client.SendNotificationAsync(request);
        }
        catch (Exception e)
        {
            return Error.Failure("notification.send.failed", e.Message);
        }
        
        return Result.Success<Error>();
    }
    
    public async Task<UnitResult<Error>> UpdateNotificationSettings(
        Guid userId, 
        bool email = true, 
        bool telegram = false, 
        bool web = true)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(Constants.ServiceAdresses.NOTIFICATION_SERVICE_ADRESS);
            var client = new Greeter.GreeterClient(channel);

            var request = new UpdateNotificationSettingsRequest() 
                { UserId = userId.ToString(), Email = email, Telegram = telegram, Web = web };
            
            await client.UpdateNotificationSettingsAsync(request);
        }
        catch (Exception e)
        {
            return Error.Failure("notification_settings.update.failed", e.Message);
        }
        
        return Result.Success<Error>();
    }
}