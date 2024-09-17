using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetProject.Domain.Shared;
using PetProject.Domain.Volunteers.ValueObjects;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Volunteers;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasConversion(i => i.Value, value => PetId.Create(value));

        builder.Property(n => n.Name).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(c => c.Color).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(h => h.HealthInfo).HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

        builder.ComplexProperty(pti => pti.PetTypeInfo, ptib =>
        {
            ptib.Property(bi => bi.BreedId).HasConversion(i => i.Value, value => BreedId.Create(value));
            ptib.Property(si => si.SpeciesId).HasConversion(i => i.Value, value => SpeciesId.Create(value));
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

        builder.ComplexProperty(d => d.Description, db =>
        { db.Property(v => v.Value).HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH); });

        builder.ComplexProperty(s => s.SerialNumber, sb =>
        { sb.Property(v => v.Value).IsRequired().HasColumnName("serial_number"); });

        builder.Property(w => w.Weight);
        builder.Property(h => h.Height);
        builder.Property(ic => ic.IsCastrated);
        builder.Property(iv => iv.IsVaccinated);

        builder.Property(bt => bt.BirthdayTime);
        builder.Property(doc => doc.DateOfCreation);

        builder.OwnsOne(rl => rl.RequisitesList, rlb =>
        {
            rlb.ToJson();

            rlb.OwnsMany(r => r.Requisites, rb =>
            {
                rb.Property(n => n.Name).IsRequired();
                rb.Property(d => d.Description).IsRequired();
            });
        });

        builder.OwnsOne(pl => pl.PhotosList, plb =>
        {
            plb.ToJson();

            plb.OwnsMany(p => p.Photos, pb =>
            {
                pb.Property(p => p.Path).IsRequired();
                pb.Property(imp => imp.IsMainPhoto).IsRequired();
            });
        });

        builder.Property<bool>("_isDeleted").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("is_deleted");

        builder.Property(hs => hs.HelpStatus).HasConversion<string>()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
    }
}