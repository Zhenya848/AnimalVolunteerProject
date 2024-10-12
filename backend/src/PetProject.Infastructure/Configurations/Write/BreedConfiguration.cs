using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Species;

namespace PetProject.Infastructure.Configurations.Write
{
    public class BreedConfiguration : IEntityTypeConfiguration<Breed>
    {
        public void Configure(EntityTypeBuilder<Breed> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasConversion(i => i.Value, value => BreedId.Create(value));

            builder.Property(x => x.title).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        }
    }
}