using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Core.ValueObjects.Dtos;
using PetProject.Core.ValueObjects.Dtos.ForQuery;

namespace PetProject.Core.Infrastructure.Configurations.Read
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

            builder.Property(si => si.SpeciesId).HasColumnName("pet_type_info_species_id");
            builder.Property(bi => bi.BreedId).HasColumnName("pet_type_info_breed_id"); ;

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
