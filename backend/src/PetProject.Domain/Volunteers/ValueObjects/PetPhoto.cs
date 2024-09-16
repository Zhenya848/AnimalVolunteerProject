using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects.IdClasses;

namespace PetProject.Domain.Volunteers.ValueObjects
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
