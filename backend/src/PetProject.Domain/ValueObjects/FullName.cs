namespace PetProject.Domain.ValueObjects
{
    public record FullName
    {
        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public string Patronymic { get; private set; } = default!;
    }
}
