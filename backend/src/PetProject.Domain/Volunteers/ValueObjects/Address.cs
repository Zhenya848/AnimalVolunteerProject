using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;

namespace PetProject.Domain.Volunteers.ValueObjects
{
    public record Address
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }

        private Address(string street, string city, string state, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public static Result<Address, Error> Create(string street, string city, string state, string zipCode)
        {
            if (string.IsNullOrWhiteSpace(street))
                return Errors.General.ValueIsInvalid("Street");

            if (string.IsNullOrWhiteSpace(city))
                return Errors.General.ValueIsInvalid("City");

            if (string.IsNullOrWhiteSpace(state))
                return Errors.General.ValueIsInvalid("State");

            if (string.IsNullOrWhiteSpace(zipCode))
                return Errors.General.ValueIsInvalid("Zip code");

            return new Address(street, city, state, zipCode);
        }
    }
}
