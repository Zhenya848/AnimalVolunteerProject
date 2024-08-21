namespace PetProject.Domain.Shared
{
    public static class Errors
    {
        public static class General
        {
            public static Error ValueIsInvalid(string? name = null) =>
                Error.Validation("value.is.invalid", $"{(name != null ? name : "value")} is invalid");

            public static Error NotFound(Guid? id = null) =>
                Error.NotFound("record.not.found", $"record not found{(id != null ? " for id: " + id : "")}");

            public static Error Failure(string? name = null) =>
                Error.Failure("failure", $"{(name != null ? name : "value")} is failure");

            public static Error Conflict(string? name = null) =>
                Error.Conflict("conflict", $"{(name != null ? name : "value")} is conflict");
        }

        public static class Volunteer
        {
            public static Error AlreadyExist()
            {
                return Error.Validation("user.already.exist", "volunteer with this phone number already exist!");
            }
        }
    }
}
