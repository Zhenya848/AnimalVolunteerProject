namespace PetProject.Domain.ValueObjects.IdClasses
{
    public record BreedId
    {
        public Guid Id { get; private set; }

        private BreedId(Guid id) => Id = id;

        public static BreedId AddNewId() => new(Guid.NewGuid());
        public static BreedId AddEmptyId() => new(Guid.Empty);

        public static BreedId Create(Guid id) => new(id);
    }
}
