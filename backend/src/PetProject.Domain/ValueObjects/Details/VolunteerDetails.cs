using PetProject.Domain.Entities;

namespace PetProject.Domain.ValueObjects.Details
{
    public class VolunteerDetails
    {
        public IReadOnlyList<SocialNetwork> SocialNetworks = default!;
        public IReadOnlyList<Requisite> Requisites = default!;

        public VolunteerDetails(IEnumerable<SocialNetwork> socialNetworks, IEnumerable<Requisite> requisites)
        {
            SocialNetworks = socialNetworks.ToList();
            Requisites = requisites.ToList();
        }

        private VolunteerDetails() { }
    }
}
