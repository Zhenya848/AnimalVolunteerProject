using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Entities;
using PetProject.Domain.Shared;

namespace PetProject.Infastructure.Configurations
{
    public class SotialNetworkConfiguration : IEntityTypeConfiguration<SotialNetwork>
    {
        public void Configure(EntityTypeBuilder<SotialNetwork> builder)
        {
            builder.ToTable("SotialNetworks");

            builder.HasKey(sn => sn.Id);
            builder.Property(sn => sn.Id).HasConversion(i => i.Id, value => SotialNetworkId.Create(value));

            builder.Property(n => n.Name).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            builder.Property(r => r.Reference).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        }
    }
}
