using CSharpFunctionalExtensions;

namespace PetProject.Core.ValueObjects
{
    public record Requisite
    {
        public string Name { get; } = default!;
        public string Description { get; } = default!;

        private Requisite(string name, string description)
        {
            Name = name;
            Description = description;
        }

        private Requisite() { }

        public static Result<Requisite, Error> Create(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Errors.General.ValueIsInvalid("Name is null or white space! name");

            if (string.IsNullOrWhiteSpace(description))
                return Errors.General.ValueIsInvalid("Description is null or white space! description");

            return new Requisite(name, description);
        }
    }
}