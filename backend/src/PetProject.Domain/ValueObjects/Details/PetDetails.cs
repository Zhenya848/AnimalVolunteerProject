using PetProject.Domain.Entities;

namespace PetProject.Domain.ValueObjects.Details
{
    public class PetDetails
    {
        public IReadOnlyList<Requisite> Requisites = default!;

        public PetDetails(IEnumerable<Requisite> requisites)
        {
            Requisites = requisites.ToList();
        }

        private PetDetails() { }
    }
}
