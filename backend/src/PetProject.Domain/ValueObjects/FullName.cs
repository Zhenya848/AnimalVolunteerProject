using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;

namespace PetProject.Domain.ValueObjects
{
    public record FullName
    {
        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public string Patronymic { get; private set; } = default!;

        private FullName(string firstName, string lastName, string patronymic)
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
        }

        public static Result<FullName, Error> Create(string firstName, string lastName, string patronymic) 
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return Errors.General.ValueIsInvalid("first name is null or white space! first name");

            if (string.IsNullOrWhiteSpace(lastName))
                return Errors.General.ValueIsInvalid("last name is null or white space! last name");

            if (string.IsNullOrWhiteSpace(patronymic))
                return Errors.General.ValueIsInvalid("patronymic is null or white space! patronymic");

            return new FullName(firstName, lastName, patronymic);
        }
    }
}
