namespace PetProject.Domain
{
    public interface IPet
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string PetType { get; set; }
        public string Description { get; set; }
        public string Breed { get; set; }
        public string Color { get; set; }
        public string HealthInfo { get; set; }
        public string Address { get; set; }
        public string TelephoneNumber { get; set; }

        public float WeightInKg { get; set; }
        public float HeightInMeters { get; set; }

        public bool IsCastrated { get; set; }
        public bool IsVaccinated { get; set; }

        public DateTime BirthdayTime { get; set; }
        public DateTime DateOfCreation { get; set; }

        public Requisite[] Requisites { get; set; }
        public HelpStatus HelpStatus { get; set; }
    }

    public enum HelpStatus
    {
        NeedHelp,
        LookingForAHome,
        FindAHome
    }
}
