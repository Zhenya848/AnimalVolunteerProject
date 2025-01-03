﻿using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Core.ValueObjects.Dtos;
using PetProject.Core.ValueObjects.Dtos.ForQuery;

namespace PetProject.Core.Infrastructure.Configurations.Read
{
    internal class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
    {
        public void Configure(EntityTypeBuilder<VolunteerDto> builder)
        {
            builder.ToTable("volunteers");

            builder.HasKey(i => i.Id);

            builder.Property(f => f.FirstName);
            builder.Property(l => l.LastName);
            builder.Property(p => p.Patronymic);

            builder.Property(d => d.Description);
            builder.Property(p => p.PhoneNumber);
            builder.Property(e => e.Experience);
        }
    }
}
