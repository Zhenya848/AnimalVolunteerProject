namespace PetProject.Domain.Shared
{
    public record Error
    {
        private const string SEPARATOR = "|";

        public string Code { get; }
        public string Message { get; }
        public ErrorType ErrorType { get; }

        private Error(string code, string message, ErrorType errorType)
        {
            Code = code;
            Message = message;
            ErrorType = errorType;
        }

        public static Error Validation(string code, string message) =>
            new Error(code, message, ErrorType.Validation);

        public static Error NotFound(string code, string message) =>
            new Error(code, message, ErrorType.NotFound);

        public static Error Failure(string code, string message) =>
            new Error(code, message, ErrorType.Failure);

        public static Error Conflict(string code, string message) =>
            new Error(code, message, ErrorType.Conflict);

        public string Serialize() =>
            string.Join(SEPARATOR, Code, Message, ErrorType);

        public static Error Deserialize(string serialialized)
        {
            string[] parts = serialialized.Split(SEPARATOR);

            if (parts.Length != 3 || Enum.TryParse(parts[2], out ErrorType type) == false)
                throw new ArgumentException("Invalid serialized error format!");

            return new Error(parts[0], parts[1], type);
        }
    }
    
    public enum ErrorType
    {
        Validation,
        NotFound,
        Failure,
        Conflict
    }
}
