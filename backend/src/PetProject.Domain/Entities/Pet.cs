using PetProject.Domain.ValueObjects;
using PetProject.Domain.ValueObjects.IdClasses;

namespace PetProject.Domain.Entities
{
    public class Pet : Shared.Entity<PetId>
    {
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public string Breed { get; private set; } = default!;
        public string Color { get; private set; } = default!;
        public string HealthInfo { get; private set; } = default!;
        public Addres Address { get; private set; } = default!;
        public TelephoneNumber TelephoneNumber { get; private set; } = default!;

        public PetTypeInfo PetTypeInfo { get; private set; } = default!;

        public float Weight { get; private set; }
        public float Height { get; private set; }

        public bool IsCastrated { get; private set; }
        public bool IsVaccinated { get; private set; }

        public DateOnly BirthdayTime { get; private set; }
        public DateOnly DateOfCreation { get; private set; }

        public List<Requisite> Requisites { get; private set; } = default!;
        public List<PetPhoto> PetPhotos { get; private set; } = default!;

        public HelpStatus HelpStatus { get; private set; }

        private Pet(PetId id) : base(id)
        {

        }
    }
}
