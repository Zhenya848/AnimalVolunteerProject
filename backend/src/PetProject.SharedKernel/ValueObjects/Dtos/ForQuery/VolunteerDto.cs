namespace PetProject.Core.ValueObjects.Dtos.ForQuery
{
    public record VolunteerDto
    {
        public Guid Id { get; }

        public string FirstName { get; } = default!;
        public string LastName { get; } = default!;
        public string Patronymic { get; } = default!;

        public string Description { get; } = default!;
        public string PhoneNumber { get; } = default!;

        public RequisiteDto[] Requisites { get; init; } = [];
        public SocialNetworkDto[] SocialNetworks { get; init; } = [];

        public int Experience { get; } = default!;
    }
}
