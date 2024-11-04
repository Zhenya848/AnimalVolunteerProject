using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Core.ValueObjects.IdValueObjects;

namespace PetProject.Species.Infrastructure.Configurations.Write
{
    public class SpeciesConfiguration : IEntityTypeConfiguration<Domain.Species>
    {
        public void Configure(EntityTypeBuilder<Domain.Species> builder)
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

            builder.HasMany(b => b.Breeds).WithOne().HasForeignKey("species_id")
                .OnDelete(DeleteBehavior.Cascade).IsRequired();
        }
    }
}
