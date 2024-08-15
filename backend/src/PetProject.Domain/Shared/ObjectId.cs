namespace PetProject.Domain.Shared
{
    public abstract class ObjectId<TId>
    {
        public Guid Id { get; private set; }

        public static TId AddNewId() => Create(Guid.NewGuid());
        public static TId AddEmptyId() => Create(Guid.Empty);

        public static TId Create(Guid id) => (TId)Activator.CreateInstance(typeof(TId), id)!;
    }
}
