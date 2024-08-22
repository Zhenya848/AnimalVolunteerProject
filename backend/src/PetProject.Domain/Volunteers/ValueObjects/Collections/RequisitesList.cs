using PetProject.Domain.Volunteers.ValueObjects;

namespace PetProject.Domain.Volunteers.ValueObjects.Collections
{
    public record RequisitesList
    {
        public IReadOnlyList<Requisite> Requisites = default!;

        private RequisitesList() { }

        public RequisitesList(IEnumerable<Requisite> requisites) =>
            Requisites = requisites.ToList();
    }
}
