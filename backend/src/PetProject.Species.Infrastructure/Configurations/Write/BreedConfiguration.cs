using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Core;
using PetProject.Core.ValueObjects.IdValueObjects;
using PetProject.Species.Domain;

namespace PetProject.Species.Infrastructure.Configurations.Write
{
    public class BreedConfiguration : IEntityTypeConfiguration<Breed>
    {
        public void Configure(EntityTypeBuilder<Breed> builder)
        {
            builder.ToTable("breeds");
            
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasConversion(i => i.Value, value => BreedId.Create(value));

            builder.Property(x => x.title).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        }
    }
}