namespace PetProject.Domain.Entities
{
    public class PetPhoto
    {
        public Guid Id { get; private set; }

        public string Path { get; private set; } = default!;
        public bool IsMainPhoto { get; private set; }
    }
}
