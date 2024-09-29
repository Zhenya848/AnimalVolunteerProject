using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Infastructure.Configurations.Read
{
    internal class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
    {
        public void Configure(EntityTypeBuilder<PetDto> builder)
        {
            builder.ToTable("pets");

            builder.HasKey(v => v.Id);
        }
    }
}
