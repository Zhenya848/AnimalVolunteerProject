﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetProject.Domain.Entities;
using PetProject.Domain.Shared;
using PetProject.Domain.ValueObjects;
using PetProject.Domain.ValueObjects.IdClasses;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasConversion(i => i.Id, value => PetId.Create(value));

        builder.Property(n => n.Name).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(d => d.Description).HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        builder.Property(c => c.Color).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(h => h.HealthInfo).HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

        builder.ComplexProperty(pti => pti.PetTypeInfo, ptib =>
        {
            ptib.Property(bi => bi.BreedId).HasConversion(i => i.Id, value => BreedId.Create(value));
            ptib.Property(si => si.SpeciesId).HasConversion(i => i.Id, value => SpeciesId.Create(value));
        });

        builder.ComplexProperty(tn => tn.TelephoneNumber, tnb =>
        {
            tnb.Property(pn => pn.PhoneNumber)
                .IsRequired()
                .HasMaxLength(TelephoneNumber.MAX_LENGTH)
                .HasColumnName("phone_number");
        });

        builder.ComplexProperty(a => a.Address, ab =>
        {
            ab.Property(p => p.Street)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("street");

            ab.Property(p => p.City)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("city");

            ab.Property(p => p.State)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("state");

            ab.Property(p => p.ZipCode)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("zipcode");
        });

        builder.Property(w => w.Weight);
        builder.Property(h => h.Height);
        builder.Property(ic => ic.IsCastrated);
        builder.Property(iv => iv.IsVaccinated);

        builder.Property(bt => bt.BirthdayTime);
        builder.Property(doc => doc.DateOfCreation);

        builder.OwnsMany(r => r.Requisites, rb => { rb.ToJson(); });
        builder.HasMany(r => r.PetPhotos).WithOne().HasForeignKey("pet_id");

        builder.Property(hs => hs.HelpStatus).HasConversion<string>()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
    }
}