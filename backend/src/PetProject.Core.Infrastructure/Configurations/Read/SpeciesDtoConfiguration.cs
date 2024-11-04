using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Core.ValueObjects.Dtos.ForQuery;

namespace PetProject.Core.Infrastructure.Configurations.Read
{
    internal class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesDto>
    {
        public void Configure(EntityTypeBuilder<SpeciesDto> builder)
        {
            builder.ToTable("species");

            builder.HasKey(i => i.Id);

            builder.Property(n => n.Name);
        }
    }
}
