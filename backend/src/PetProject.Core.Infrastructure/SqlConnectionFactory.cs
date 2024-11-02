using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PetProject.Core.Application;
using PetProject.Core.Application.Abstractions;

namespace PetProject.Core.Infrastructure
{
    public class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
    {
        public IDbConnection Create() =>
            new NpgsqlConnection(configuration.GetConnectionString("Database"));
    }
}
