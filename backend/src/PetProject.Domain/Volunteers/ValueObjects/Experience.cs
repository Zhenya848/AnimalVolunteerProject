using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;

namespace PetProject.Domain.Volunteers.ValueObjects
{
    public record Experience
    {
        public int Value { get; }

        private Experience() { }

        private Experience(int value) {  Value = value; }

        public static Result<Experience, Error> Create(int exp)
        {
            if (exp < 0)
                return Errors.General.ValueIsInvalid("Wtf??? experience can't be less than zero! experience");

            return new Experience(exp);
        }
    }
}
