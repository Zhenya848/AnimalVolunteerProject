namespace PetProject.Domain.Volunteers.ValueObjects.Collections
{
    public record PetPhotosList
    {
        public IReadOnlyList<PetPhoto> Photos = default!;

        private PetPhotosList() { }

        public PetPhotosList(IEnumerable<PetPhoto> photos) { Photos = photos.ToList(); }
    }
}
