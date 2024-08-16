namespace PetProject.Domain.Aggregates
{
    public class VolunteerId
    {
        public Guid Id { get; private set; }

        private VolunteerId(Guid id) => Id = id;

        public static VolunteerId AddNewId() => new(Guid.NewGuid());
        public static VolunteerId AddEmptyId() => new(Guid.Empty);

        public static VolunteerId Create(Guid id) => new(id);
    }
}
