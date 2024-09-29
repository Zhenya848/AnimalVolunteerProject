using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Shared.ValueObjects.Dtos.ForQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetProject.Infastructure.Configurations.Read
{
    internal class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
    {
        public void Configure(EntityTypeBuilder<VolunteerDto> builder)
        {
            builder.ToTable("volunteers");

            builder.HasKey(v => v.Id);

            builder.OwnsMany(s => s.SocialNetworksDto, sb =>
            {
                sb.ToJson();

                sb.Property(n => n.name);
                sb.Property(r => r.reference);
            });
        }
    }
}
