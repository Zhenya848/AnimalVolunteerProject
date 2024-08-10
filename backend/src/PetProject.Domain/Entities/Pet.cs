namespace PetProject.Domain.Entities
{
    public class Pet
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; } = default!;
        public string PetType { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public string Breed { get; private set; } = default!;
        public string Color { get; private set; } = default!;
        public string HealthInfo { get; private set; } = default!;
        public string Address { get; private set; } = default!;
        public string TelephoneNumber { get; private set; } = default!;

        public float Weight { get; private set; }
        public float Height { get; private set; }

        public bool IsCastrated { get; private set; }
        public bool IsVaccinated { get; private set; }

        public DateOnly BirthdayTime { get; private set; }
        public DateOnly DateOfCreation { get; private set; }

        public List<Requisite> Requisites { get; private set; } = default!;
        public List<PetPhoto> PetPhotos { get; private set; } = default!;

        public HelpStatus HelpStatus { get; private set; }
    }
}
