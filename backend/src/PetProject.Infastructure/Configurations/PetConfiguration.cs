using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetProject.Domain.Entities;
using PetProject.Domain.Shared;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("Pets");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasConversion(i => i.Id, value => PetId.Create(value));

        builder.Property(n => n.Name).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(pt => pt.PetType).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(d => d.Description).HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        builder.Property(b => b.Breed).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(c => c.Color).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(h => h.HealthInfo).HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        builder.Property(a => a.Address).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        builder.Property(t => t.TelephoneNumber).HasMaxLength(13);

        builder.Property(w => w.Weight);
        builder.Property(h => h.Height);
        builder.Property(ic => ic.IsCastrated);
        builder.Property(iv => iv.IsVaccinated);

        builder.Property(bt => bt.BirthdayTime);
        builder.Property(dof => dof.DateOfCreation);

        builder.HasMany(s => s.Requisites).WithOne().HasForeignKey("pet_id");
        builder.HasMany(r => r.PetPhotos).WithOne().HasForeignKey("pet_id");

        builder.Property(hs => hs.HelpStatus).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
    }
}