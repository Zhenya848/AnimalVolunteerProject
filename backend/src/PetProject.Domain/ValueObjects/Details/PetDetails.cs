using PetProject.Domain.Entities;

namespace PetProject.Domain.ValueObjects.Details
{
    public class PetDetails
    {
        private List<Requisite> _requisites = [];

        public IReadOnlyList<Requisite> Requisites => _requisites;

        public PetDetails(List<Requisite> requisites)
        {
            _requisites = requisites;
        }

        private PetDetails() { }
    }
}
