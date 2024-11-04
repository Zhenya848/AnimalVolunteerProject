using CSharpFunctionalExtensions;
using PetProject.Core;

namespace PetProject.Volunteers.Domain.ValueObjects
{
    public record PetPhoto
    {
        public string Path { get; } = default!;
        public bool IsMainPhoto { get; }

        private PetPhoto(string path, bool isMainPhoto)
        {
            Path = path;
            IsMainPhoto = isMainPhoto;
        }

        public static Result<PetPhoto, Error> Create(string path, bool isMainPhoto)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Errors.General.ValueIsInvalid("path to storage");

            return new PetPhoto(path, isMainPhoto);
        }
    }
}
