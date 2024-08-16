namespace PetProject.Domain.ValueObjects
{
    public record Requisite
    {
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
    }
}