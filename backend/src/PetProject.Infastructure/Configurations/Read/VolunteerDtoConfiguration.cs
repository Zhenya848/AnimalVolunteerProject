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
