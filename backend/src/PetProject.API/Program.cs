using PetProject.Infrastructure.Authentification;
using PetProject.API.Middlewares;
using Serilog;
using Microsoft.OpenApi.Models;
using PetProject.Accounts.Application;
using PetProject.Accounts.Implementation;
using PetProject.Accounts.Presentation;
using PetProject.Core.Application;
using PetProject.Core.Infrastructure;
using PetProject.Framework;
using PetProject.Infrastructure.Authentification.Seeding;
using PetProject.Species.Application;
using PetProject.Species.Infrastructure;
using PetProject.Volunteers.Application;
using PetProject.Volunteers.Infrastructure;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq") ?? throw new ArgumentNullException("Seq"))
    .CreateLogger();

builder.Services.AddSerilog();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyTestService", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });
});

builder.Services
    .AddFromFramework()
    .AddFromCoreApplication()
    .AddFromCoreInfrastructure(builder.Configuration)
    .AddFromImplementation()
    .AddFromAccountsInfrastructure(builder.Configuration)
    .AddFromAccountsApplication()
    .AddFromVolunteersInfrastructure(builder.Configuration)
    .AddFromVolunteersApplication()
    .AddFromSpeciesInfrastructure(builder.Configuration)
    .AddFromSpeciesApplication();

var app = builder.Build();

var accountsSeeder = app.Services.GetRequiredService<AccountsSeeder>();
await accountsSeeder.SeedAsync();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //await app.ApplyMigrations();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
