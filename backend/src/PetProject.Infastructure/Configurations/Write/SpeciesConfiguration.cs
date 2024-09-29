using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Species;

namespace PetProject.Infastructure.Configurations.Write
{
    public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
    {
        public void Configure(EntityTypeBuilder<Species> builder)
        {
            builder.ToTable("species");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasConversion(
                    speciesId => speciesId.Value,
                    id => SpeciesId.Create(id)
                );

            builder.Property(x => x.Name)
                .IsRequired();

            builder.HasMany(b => b.Breeds).WithOne().HasForeignKey("species_id");
        }
    }
}
