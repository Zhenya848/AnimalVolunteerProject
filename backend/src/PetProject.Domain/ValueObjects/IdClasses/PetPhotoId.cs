namespace PetProject.Domain.ValueObjects.IdClasses
{
    public record PetPhotoId
    {
        public Guid Id { get; private set; }

        private PetPhotoId(Guid id) => Id = id;

        public static PetPhotoId AddNewId() => new(Guid.NewGuid());
        public static PetPhotoId AddEmptyId() => new(Guid.Empty);

        public static PetPhotoId Create(Guid id) => new(id);
    }
}
