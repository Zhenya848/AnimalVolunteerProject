namespace PetProject.Core.ValueObjects.IdValueObjects
{
    public record PetId
    {
        public Guid Value { get; private set; }

        private PetId(Guid id) => Value = id;

        public static PetId AddNewId() => new(Guid.NewGuid());
        public static PetId AddEmptyId() => new(Guid.Empty);

        public static PetId Create(Guid id) => new(id);

        public static implicit operator Guid(PetId petId)
        {
            ArgumentNullException.ThrowIfNull(petId);
            return petId.Value;
        }
    }
}
