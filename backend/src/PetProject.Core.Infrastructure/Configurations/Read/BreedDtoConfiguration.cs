using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Core.ValueObjects.Dtos.ForQuery;

namespace PetProject.Core.Infrastructure.Configurations.Read
{
    internal class BreedDtoConfiguration : IEntityTypeConfiguration<BreedDto>
    {
        public void Configure(EntityTypeBuilder<BreedDto> builder)
        {
            builder.ToTable("breeds");

            builder.HasKey(i => i.Id);
            builder.Property(si => si.SpeciesId);

            builder.Property(t => t.Title);
        }
    }
}
