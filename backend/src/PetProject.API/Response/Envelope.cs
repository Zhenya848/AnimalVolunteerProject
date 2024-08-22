using PetProject.Domain.Shared;

namespace PetProject.API.Response
{
    public record ResponseError(string? errorCode, string? errorMessage, string? propertyName);

    public class Envelope
    {
        public object? Result { get; }
        public List<ResponseError> ResponseErrors { get; }

        public DateTime? Time {  get; }

        private Envelope(object? result, IEnumerable<ResponseError> errors)
        {
            Result = result;
            ResponseErrors = errors.ToList();
            Time = DateTime.Now;
        }

        public static Envelope Ok(object? result) =>
            new Envelope (result, []);

        public static Envelope Error(IEnumerable<ResponseError> errors) =>
            new Envelope (null, errors);
    }
}