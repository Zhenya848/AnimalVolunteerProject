using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Entities.Aggregates;
using PetProject.Domain.Shared;
using PetProject.Domain.ValueObjects;
using PetProject.Domain.ValueObjects.IdClasses;

namespace PetProject.Infastructure.Configurations
{
    public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
    {
        public void Configure(EntityTypeBuilder<Volunteer> builder)
        {
            builder.ToTable("volunteers");

            builder.HasKey(v => v.Id);
            builder.Property(v => v.Id).HasConversion(i => i.Value, value => VolunteerId.Create(value));

            builder.ComplexProperty(n => n.Name, nb =>
            {
                nb.Property(f => f.FirstName).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH).HasColumnName("first_name");
                nb.Property(l => l.LastName).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH).HasColumnName("last_name");
                nb.Property(p => p.Patronymic).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH).HasColumnName("patronymic");
            });

            builder.ComplexProperty(tn => tn.TelephoneNumber, tnb =>
            {
                tnb.Property(pn => pn.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(TelephoneNumber.MAX_LENGTH)
                    .HasColumnName("phone_number");
            });

            builder.Property(d => d.Description).HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
            builder.Property(e => e.EXP);

            builder.OwnsOne(d => d.Details, db => 
            { 
                db.ToJson();

                db.OwnsMany(r => r.Requisites, rb =>
                {
                    rb.Property(n => n.Name).IsRequired();
                    rb.Property(d => d.Description).IsRequired();
                });

                db.OwnsMany(sn => sn.SocialNetworks, snb =>
                {
                    snb.Property(n => n.Name).IsRequired();
                    snb.Property(r => r.Reference).IsRequired();
                });
            });

            builder.HasMany(p => p.Pets).WithOne().HasForeignKey("volunteer_id");
        }
    }
}