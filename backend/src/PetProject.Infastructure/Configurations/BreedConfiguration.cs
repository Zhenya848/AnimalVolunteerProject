using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Entities;
using PetProject.Domain.Shared;

namespace PetProject.Infastructure.Configurations
{
    public class BreedConfiguration : IEntityTypeConfiguration<Breed>
    {
        public void Configure(EntityTypeBuilder<Breed> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasConversion(i => i.Id, value => BreedId.Create(value));

            builder.Property(x => x.title).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        }
    }
}
