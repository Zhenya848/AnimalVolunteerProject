namespace PetProject.Domain.Aggregates
{
    public record SpeciesId
    {
        public Guid Id { get; private set; }

        private SpeciesId(Guid id) => Id = id;

        public static SpeciesId AddNewId() => new(Guid.NewGuid());
        public static SpeciesId AddEmptyId() => new(Guid.Empty);

        public static SpeciesId Create(Guid id) => new(id);
    }
}
