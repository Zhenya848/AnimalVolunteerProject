namespace PetProject.Core.ValueObjects.IdValueObjects
{
    public record SpeciesId
    {
        public Guid Value { get; private set; }

        private SpeciesId(Guid id) => Value = id;

        public static SpeciesId AddNewId() => new(Guid.NewGuid());
        public static SpeciesId AddEmptyId() => new(Guid.Empty);

        public static SpeciesId Create(Guid id) => new(id);
    }
}
