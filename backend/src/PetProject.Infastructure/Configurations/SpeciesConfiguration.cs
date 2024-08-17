using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Entities.Aggregates;
using PetProject.Domain.ValueObjects.IdClasses;

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
                    id => SpeciesId.Create(id)
                );

            builder.Property(x => x.Name)
                .IsRequired();

            builder.HasMany(b => b.Breeds).WithOne().HasForeignKey("species_id");
        }
    }
}
