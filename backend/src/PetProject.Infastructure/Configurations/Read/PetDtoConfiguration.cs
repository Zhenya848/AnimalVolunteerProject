using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Shared.ValueObjects.Dtos;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using PetProject.Domain.Volunteers.ValueObjects.Collections;
using PetProject.Domain.Volunteers.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PetProject.Domain.Shared.ValueObjects.IdClasses;

namespace PetProject.Infastructure.Configurations.Read
{
    internal class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
    {
        public void Configure(EntityTypeBuilder<PetDto> builder)
        {
            builder.ToTable("pets");

            builder.HasKey(i => i.Id);
            builder.Property(vi => vi.VolunteerId);

            builder.Property(n => n.Name);
            builder.Property(c => c.Color);
            builder.Property(h => h.HealthInfo);

            builder.Property(s => s.Street);
            builder.Property(c => c.City);
            builder.Property(s => s.State);
            builder.Property(z => z.Zipcode);

            builder.Property(d => d.Description);
            builder.Property(t => t.PhoneNumber);

            builder.Property(w => w.Weight);
            builder.Property(h => h.Height);

            builder.Property(ic => ic.IsCastrated);
            builder.Property(iv => iv.IsVaccinated);

            builder.Property(bt => bt.BirthdayTime);
            builder.Property(doc => doc.DateOfCreation);

            builder.Property(sn => sn.SerialNumber);

            builder.Property(pl => pl.Requisites)
                .HasConversion(
                    requisites => JsonSerializer.Serialize(
                        string.Empty,
                        JsonSerializerOptions.Default),

                    json => JsonSerializer.Deserialize<RequisiteDto[]>(json, JsonSerializerOptions.Default)!);

            builder.Property(pl => pl.Photos)
                .HasConversion(
                    photos => JsonSerializer.Serialize(
                        string.Empty,
                        JsonSerializerOptions.Default),

                    json => JsonSerializer.Deserialize<PetPhotoDto[]>(json, JsonSerializerOptions.Default)!);
        }
    }
}
