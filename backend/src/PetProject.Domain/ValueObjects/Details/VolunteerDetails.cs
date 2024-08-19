using PetProject.Domain.Entities;

namespace PetProject.Domain.ValueObjects.Details
{
    public class VolunteerDetails
    {
        private List<SocialNetwork> _socialNetworks = [];
        private List<Requisite> _requisites = [];

        public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
        public IReadOnlyList<Requisite> Requisites => _requisites;

        public VolunteerDetails(List<SocialNetwork> socialNetworks, List<Requisite> requisites)
        {
            _socialNetworks = socialNetworks;
            _requisites = requisites;
        }

        private VolunteerDetails() { }
    }
}
