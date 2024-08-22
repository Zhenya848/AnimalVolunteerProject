using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.Domain.Volunteers.ValueObjects.Collections
{
    public record SocialNetworksList
    {
        public IReadOnlyList<SocialNetwork> SocialNetworks = default!;

        private SocialNetworksList() { }

        public SocialNetworksList(IEnumerable<SocialNetwork> socialNetworks) =>
            SocialNetworks = socialNetworks.ToList();
    }
}