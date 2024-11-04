using CSharpFunctionalExtensions;

namespace PetProject.Core.ValueObjects
{
    public record Description
    {
        public string Value { get; } = default!;
        
        private Description() { }

        private Description(string value) { Value = value; }

        public static Result<Description, Error> Create(string description) 
        {
            if (string.IsNullOrWhiteSpace(description))
                return Errors.General.ValueIsInvalid("Description");

            return new Description(description);
        }
    }
}
