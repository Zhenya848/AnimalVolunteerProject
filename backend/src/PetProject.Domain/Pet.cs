namespace PetProject.Domain
{
    public class Pet
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }
        public string PetType { get; private set; }
        public string Description { get; private set; }
        public string Breed { get; private set; }
        public string Color { get; private set; }
        public string HealthInfo { get; private set; }
        public string Address { get; private set; }
        public string TelephoneNumber { get; private set; }

        public float Weight { get; private set; }
        public float Height { get; private set; }

        public bool IsCastrated { get; private set; }
        public bool IsVaccinated { get; private set; }

        public DateOnly BirthdayTime { get; private set; }
        public DateOnly DateOfCreation { get; private set; }

        public List<Requisite> Requisites { get; private set; }
        public HelpStatus HelpStatus { get; private set; }
    }

    public enum HelpStatus
    {
        NeedHelp,
        LookingForAHome,
        FindAHome
    }
}
