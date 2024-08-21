using PetProject.Application;
using PetProject.Application.Repositories;
using PetProject.Application.Volunteers.Services.CreateReadUpdateDeleteService;
using PetProject.Infastructure;
using PetProject.Infastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddFromInfrastructure()
    .AddFromApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
