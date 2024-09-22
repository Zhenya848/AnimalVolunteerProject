using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;

namespace PetProject.Domain.Volunteers.ValueObjects
{
    public record FilePath
    {
        public string FullPath { get; }

        private FilePath(string path) =>
            FullPath = path;

        public static Result<FilePath, Error> Create(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var validExtensions = new List<string>() { ".png", ".jpg", ".jpeg", ".ico" };

            if (validExtensions.Where(e => e == extension).Any() == false)
                return Errors.General.ValueIsInvalid("File extension");

            return new FilePath(Guid.NewGuid() + extension);
        }
    }
}
