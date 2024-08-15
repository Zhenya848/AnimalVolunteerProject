using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Aggregates;
using PetProject.Domain.Shared;

namespace PetProject.Infastructure.Configurations
{
    public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
    {
        public void Configure(EntityTypeBuilder<Volunteer> builder)
        {
            builder.ToTable("Volunteers");

            builder.HasKey(v => v.Id);
            builder.Property(v => v.Id).HasConversion(i => i.Id, value => VolunteerId.Create(value));

            builder.ComplexProperty(n => n.Name, nb =>
            {
                nb.Property(f => f.FirstName).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
                nb.Property(l => l.LastName).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
                nb.Property(p => p.Patronymic).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            });

            builder.Property(d => d.Description).HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
            builder.Property(t => t.TelephoneNumber).HasMaxLength(13);

            builder.Property(e => e.EXP);
            builder.Property(c => c.CountOfShelterAnimals);
            builder.Property(c => c.CountOfHomelessAnimals);
            builder.Property(c => c.CountOfIllAnimals);

            builder.HasMany(s => s.SotialNetworks).WithOne().HasForeignKey("volunteer_id");
            builder.HasMany(r => r.Requisites).WithOne().HasForeignKey("volunteer_id");
            builder.HasMany(p => p.Pets).WithOne().HasForeignKey("volunteer_id");
        }
    }
}
