using Microsoft.EntityFrameworkCore;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Application.Repositories
{
    public interface IReadDbContext
    {
        public DbSet<VolunteerDto> Volunteers { get; }
        public DbSet<PetDto> Pets { get; }
    }
}
