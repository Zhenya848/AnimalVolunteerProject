using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Entities;
using PetProject.Domain.Shared;

namespace PetProject.Infastructure.Configurations
{
    public class RequisiteConfiguration : IEntityTypeConfiguration<Requisite>
    {
        public void Configure(EntityTypeBuilder<Requisite> builder)
        {
            builder.ToTable("Requisites");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).HasConversion(i => i.Id, value => RequisiteId.Create(value));

            builder.Property(n => n.Name).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            builder.Property(d => d.Description).HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        }
    }
}
