namespace PetProject.Domain.ValueObjects
{
    public record SotialNetwork
    {
        public string Name { get; private set; } = default!;
        public string Reference { get; private set; } = default!;
    }
}
