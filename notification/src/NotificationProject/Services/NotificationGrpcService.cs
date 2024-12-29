using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using NotificationProject;
using NotificationProject.Application;
using NotificationService;

namespace NotificationProject.Services;

public class NotificationGrpcService : Greeter.GreeterBase
{
    private readonly Application.NotificationService _notificationService;
    private readonly ILogger<NotificationGrpcService> _logger;

    public NotificationGrpcService(
        Application.NotificationService notificationService, 
        ILogger<NotificationGrpcService> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public override async Task<SendNotificationResponse> SendNotification(
        SendNotificationRequest request,
        ServerCallContext context)
    {
        await _notificationService.SendNotificationHandle(request);
        
        return new SendNotificationResponse();
    }

    public override async Task<UpdateNotificationSettingsResponse> UpdateNotificationSettings(
        UpdateNotificationSettingsRequest request,
        ServerCallContext context)
    {
        await _notificationService.UpdateNotificationSettingsHandle(request);
        
        return new UpdateNotificationSettingsResponse();
    }
}