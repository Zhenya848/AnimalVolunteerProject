using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Shared.ValueObjects.Dtos;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetProject.Infastructure.Configurations.Read
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

            builder.Property(pl => pl.Requisites)
                .HasConversion(
                    requisites => JsonSerializer.Serialize(
                        string.Empty,
                        JsonSerializerOptions.Default),

                    json => JsonSerializer.Deserialize<RequisiteDto[]>(json, JsonSerializerOptions.Default)!)
                .HasColumnType("jsonb");

            builder.Property(pl => pl.SocialNetworks)
                .HasConversion(
                    requisites => JsonSerializer.Serialize(
                        string.Empty,
                        JsonSerializerOptions.Default),

                    json => JsonSerializer.Deserialize<SocialNetworkDto[]>(json, JsonSerializerOptions.Default)!)
                .HasColumnType("jsonb");
        }
    }
}
