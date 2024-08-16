using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Entities;
using PetProject.Domain.Shared;

namespace PetProject.Infastructure.Configurations
{
    public class PetPhotoConfiguration : IEntityTypeConfiguration<PetPhoto>
    {
        public void Configure(EntityTypeBuilder<PetPhoto> builder)
        {
            builder.ToTable("petPhotos");

            builder.HasKey(pp => pp.Id);
            builder.Property(pp => pp.Id).HasConversion(i => i.Id, value => PetPhotoId.Create(value));

            builder.Property(p => p.Path).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            builder.Property(imp => imp.IsMainPhoto);
        }
    }
}
