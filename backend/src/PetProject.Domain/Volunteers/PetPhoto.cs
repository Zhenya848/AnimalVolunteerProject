using PetProject.Domain.Shared.ValueObjects.IdClasses;

namespace PetProject.Domain.Volunteers
{
    public class PetPhoto : Shared.Entity<PetPhotoId>
    {
        public string Path { get; private set; } = default!;
        public bool IsMainPhoto { get; private set; }

        private PetPhoto(PetPhotoId id) : base(id)
        {

        }
    }
}
