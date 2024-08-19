namespace PetProject.Domain.Shared
{
    public static class Errors
    {
        public static class General
        {
            public static Error ValueIsInvalid(string? name = null) =>
                Error.Validation("value.is.invalid", $"{(name != null ? name : "value")} is invalid");
        }
    }
}
