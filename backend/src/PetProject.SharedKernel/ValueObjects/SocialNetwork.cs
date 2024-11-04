using CSharpFunctionalExtensions;
using PetProject.Core;

namespace PetProject.Volunteers.Domain.ValueObjects
{
    public record SocialNetwork
    {
        public string Name { get; } = default!;
        public string Reference { get; } = default!;

        private SocialNetwork(string name, string reference)
        {
            Name = name;
            Reference = reference;
        }

        private SocialNetwork() { }

        public static Result<SocialNetwork, Error> Create(string name, string reference)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Errors.General.ValueIsInvalid("Name is null or white space! name");

            if (string.IsNullOrWhiteSpace(reference))
                return Errors.General.ValueIsInvalid("Reference is null or white space! reference");

            return new SocialNetwork(name, reference);
        }
    }
}
