using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Aggregates;
using PetProject.Domain.Entities;
using PetProject.Domain.Shared;

namespace PetProject.Infastructure.Configurations
{
    public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
    {
        public void Configure(EntityTypeBuilder<Species> builder)
        {
            builder.ToTable("species");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasConversion(
                    speciesId => speciesId.Id,
                    result => SpeciesId.Create(result)
                );

            builder.Property(x => x.PetType)
                .IsRequired();

            builder.HasMany(b => b.Breeds).WithOne().HasForeignKey("species_id");
        }
    }
}
