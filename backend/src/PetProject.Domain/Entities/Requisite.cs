namespace PetProject.Domain.Entities
{
    public class Requisite : Shared.Entity<RequisiteId>
    {
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;

        private Requisite(RequisiteId id) : base(id)
        {

        }
    }
}