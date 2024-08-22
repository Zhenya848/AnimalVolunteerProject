namespace PetProject.Domain.Shared.ValueObjects.IdClasses
{
    public record PetPhotoId
    {
        public Guid Value { get; private set; }

        private PetPhotoId(Guid id) => Value = id;

        public static PetPhotoId AddNewId() => new(Guid.NewGuid());
        public static PetPhotoId AddEmptyId() => new(Guid.Empty);

        public static PetPhotoId Create(Guid id) => new(id);
    }
}
