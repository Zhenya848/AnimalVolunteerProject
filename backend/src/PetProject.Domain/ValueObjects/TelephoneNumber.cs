namespace PetProject.Domain.ValueObjects
{
    public record TelephoneNumber
    {
        private string _phoneRegularka = "^[+][0-9]{7,14}$";
        public string PhoneNumber {  get; private set; }

        public const int MAX_LENGTH = 13;

        private TelephoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
    }
}
