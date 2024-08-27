using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.IdClasses;
using PetProject.Domain.Species;
using PetProject.Domain.Volunteers.ValueObjects;
using PetProject.Domain.Volunteers.ValueObjects.Collections;

namespace PetProject.Domain.Volunteers
{
    public class Pet : Shared.Entity<PetId>, ISoftDeletable
    {
        private bool _isDeleted = false;

        public string Name { get; private set; } = default!;
        public Description Description { get; private set; } = default!;
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

        public RequisitesList RequisitesList { get; private set; } = default!;
        public List<PetPhoto> Photos { get; private set; } = default!;

        public HelpStatus HelpStatus { get; private set; }

        private Pet(PetId id) : base(id)
        {

        }

        public void Delete() => _isDeleted = true;

        public void Restore() => _isDeleted = false;
    }
}
