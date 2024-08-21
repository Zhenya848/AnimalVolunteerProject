using PetProject.Domain.Entities;

namespace PetProject.Domain.ValueObjects.Details
{
    public class VolunteerDetails
    {
        public IEnumerable<SocialNetwork> SocialNetworks = [];
        public IEnumerable<Requisite> Requisites = [];

        public VolunteerDetails(IEnumerable<SocialNetwork> socialNetworks, IEnumerable<Requisite> requisites)
        {
            SocialNetworks = socialNetworks;
            Requisites = requisites;
        }

        private VolunteerDetails() { }
    }
}
