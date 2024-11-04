using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetProject.Core.ValueObjects
{
    public record TelephoneNumber
    {
        private const string PHONE_REGEX = "^[+][0-9]{7,14}$";
        public string PhoneNumber { get; } = default!;

        public const int MAX_LENGTH = 13;

        private TelephoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public static Result<TelephoneNumber, Error> Create(string phoneNumber)
        {
            phoneNumber = phoneNumber.Trim();

            if (Regex.IsMatch(phoneNumber, PHONE_REGEX) == false)
                return Errors.General.ValueIsInvalid("Telephone number");

            return new TelephoneNumber(phoneNumber);
        }
    }
}
