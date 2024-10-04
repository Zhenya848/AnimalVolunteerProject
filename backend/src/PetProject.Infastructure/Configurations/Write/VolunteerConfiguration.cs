using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.Dtos;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Volunteers;
using PetProject.Domain.Volunteers.ValueObjects;
using PetProject.Domain.Volunteers.ValueObjects.Collections;
using PetProject.Infastructure.Extensions;
using System.Text.Json;

namespace PetProject.Infastructure.Configurations.Write
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

            builder.ComplexProperty(d => d.Description, db =>
            { db.Property(v => v.Value).HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH); });

            builder.ComplexProperty(e => e.Experience, eb => { eb.Property(v => v.Value); });

            builder.Property(rl => rl.Requisites)
                .ValueToDtoConversion(
                requisite => new RequisiteDto(requisite.Name, requisite.Description),
                dto => Requisite.Create(dto.Name, dto.Description).Value);

            builder.Property(snl => snl.SocialNetworks)
                .ValueToDtoConversion(
                socialNetwork => new SocialNetworkDto(socialNetwork.Name, socialNetwork.Reference),
                dto => SocialNetwork.Create(dto.Name, dto.Reference).Value);

            builder.HasMany(p => p.Pets).WithOne().HasForeignKey("volunteer_id");
            builder.Property<bool>("_isDeleted").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("is_deleted");
        }
    }
}