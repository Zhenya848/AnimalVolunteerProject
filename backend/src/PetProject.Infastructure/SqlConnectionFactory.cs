using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using PetProject.Application;
using PetProject.Application.Repositories.Read;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using PetProject.Infastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Infastructure
{
    public class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
    {
        public IDbConnection Create() =>
            new NpgsqlConnection(configuration.GetConnectionString("Database"));
    }
}
