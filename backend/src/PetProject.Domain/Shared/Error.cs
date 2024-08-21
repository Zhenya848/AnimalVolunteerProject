namespace PetProject.Domain.Shared
{
    public record Error
    {
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

        public static Error ValueIsRequired(string code, string message) =>
            new Error(code, message, ErrorType.ValueIsRequired);

        public static Error Failure(string code, string message) =>
            new Error(code, message, ErrorType.Failure);

        public static Error Conflict(string code, string message) =>
            new Error(code, message, ErrorType.Conflict);
    }
    
    public enum ErrorType
    {
        Validation,
        NotFound,
        ValueIsRequired,
        Failure,
        Conflict
    }
}
