namespace PetProject.Domain.Entities
{
    public class PetId
    {
        public Guid Id { get; private set; }

        private PetId(Guid id) => Id = id;

        public static PetId AddNewId() => new(Guid.NewGuid());
        public static PetId AddEmptyId() => new(Guid.Empty);

        public static PetId Create(Guid id) => new(id);
    }
}
