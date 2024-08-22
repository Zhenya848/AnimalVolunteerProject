namespace PetProject.Domain.Shared.ValueObjects.IdClasses
{
    public record VolunteerId
    {
        public Guid Value { get; }

        private VolunteerId(Guid id) => Value = id;

        public static VolunteerId AddNewId() => new(Guid.NewGuid());
        public static VolunteerId AddEmptyId() => new(Guid.Empty);

        public static VolunteerId Create(Guid id) => new(id);

        public static implicit operator Guid(VolunteerId volunteerId)
        {
            ArgumentNullException.ThrowIfNull(volunteerId);
            return volunteerId.Value;
        }
    }
}
