namespace PetProject.Domain.Shared.ValueObjects.IdClasses
{
    public record BreedId
    {
        public Guid Value { get; private set; }

        private BreedId(Guid id) => Value = id;

        public static BreedId AddNewId() => new(Guid.NewGuid());
        public static BreedId AddEmptyId() => new(Guid.Empty);

        public static BreedId Create(Guid id) => new(id);
    }
}
