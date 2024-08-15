namespace PetProject.Domain.Entities
{
    public class SotialNetwork : Shared.Entity<SotialNetworkId>
    {
        public string Name { get; private set; } = default!;
        public string Reference { get; private set; } = default!;

        private SotialNetwork(SotialNetworkId id) : base(id)
        {

        }
    }
}
