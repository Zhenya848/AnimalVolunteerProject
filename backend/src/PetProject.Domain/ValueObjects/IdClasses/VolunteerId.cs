namespace PetProject.Domain.ValueObjects.IdClasses
{
    public record VolunteerId
    {
        public Guid Value { get; }

        private VolunteerId(Guid id) => Value = id;

        public static VolunteerId AddNewId() => new(Guid.NewGuid());
        public static VolunteerId AddEmptyId() => new(Guid.Empty);

        public static VolunteerId Create(Guid id) => new(id);
    }
}
