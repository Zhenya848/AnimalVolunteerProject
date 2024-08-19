using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;

namespace PetProject.Domain.ValueObjects
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
                return Errors.General.ValueIsInvalid("Name is null or white space!");

            if (string.IsNullOrWhiteSpace(description))
                return Errors.General.ValueIsInvalid("Description is null or white space!");

            return new Requisite(name, description);
        }
    }
}