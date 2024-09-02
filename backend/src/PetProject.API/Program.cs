using PetProject.API;
using PetProject.Application;
using PetProject.Application.Repositories;
using PetProject.Application.Volunteers.Services.CreateReadUpdateDeleteService;
using PetProject.Infastructure;
using PetProject.Infastructure.Repositories;
using FluentValidation.AspNetCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using PetProject.API.Validation;
using Microsoft.EntityFrameworkCore;
using PetProject.API.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq") ?? throw new ArgumentNullException("Seq"))
    .CreateLogger();

builder.Services.AddSerilog();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddFromInfrastructure(builder.Configuration)
    .AddFromApplication()
    .AddFromAPI();

builder.Services.AddFluentValidationAutoValidation(conf =>
{
    conf.OverrideDefaultResultFactoryWith<CustomResultFactory>();
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyMigrations();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
