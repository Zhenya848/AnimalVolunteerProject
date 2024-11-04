namespace PetProject.Core.ValueObjects.Dtos.ForQuery
{
    public record PetDto
    {
        public Guid Id { get; }
        public Guid VolunteerId { get; }

        public string Name { get; } = default!;
        public string Description { get; } = default!;
        public string Color { get; } = default!;
        public string HealthInfo { get; } = default!;
        public string Street { get; } = default!;
        public string City { get; } = default!;
        public string State { get; } = default!;
        public string Zipcode { get; } = default!;
        public string PhoneNumber { get; } = default!;

        public Guid BreedId { get; }
        public Guid SpeciesId { get; }

        public int SerialNumber { get; } = default!;

        public float Weight { get; }
        public float Height { get; }

        public bool IsCastrated { get; }
        public bool IsVaccinated { get; }

        public DateTime BirthdayTime { get; }
        public DateTime DateOfCreation { get; }

        public RequisiteDto[] Requisites { get; set; } = [];
        public PetPhotoDto[] Photos { get; set; } = [];

        public HelpStatus HelpStatus { get; }
    }
}
