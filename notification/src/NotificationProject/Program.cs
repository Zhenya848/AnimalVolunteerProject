using NotificationProject;
using NotificationProject.Services;
using NotificationProject.Services.Requests;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddFromNotificationService(builder.Configuration);

var app = builder.Build();

app.MapGrpcService<NotificationGrpcService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();