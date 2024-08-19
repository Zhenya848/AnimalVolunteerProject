using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;
using System.Text.RegularExpressions;

namespace PetProject.Domain.ValueObjects
{
    public record TelephoneNumber
    {
        private const string PHONE_REGEX = "^[+][0-9]{7,14}$";
        public string PhoneNumber { get; private set; } = default!;

        public const int MAX_LENGTH = 13;

        private TelephoneNumber(string phoneNumber) 
        {
            PhoneNumber = phoneNumber;
        }

        public static Result<TelephoneNumber, Error> Create(string phoneNumber)
        {
            if (Regex.IsMatch(phoneNumber, PHONE_REGEX) == false)
                return Errors.General.ValueIsInvalid("Invalid telephone number!");

            return new TelephoneNumber(phoneNumber);
        }
    }
}
