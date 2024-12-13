namespace PetProject.Core;

public abstract class SoftDeletableEntity<TId> : Entity<TId>
{
    public bool IsDeleted { get; private set; }
    public DateTime? DeletionDate { get; private set; }
    
    protected SoftDeletableEntity(TId id) : base(id)
    {
        
    }

    public virtual void Delete()
    {
        IsDeleted = true;
        DeletionDate = DateTime.UtcNow;
    }

    public virtual void Restore()
    {
        IsDeleted = false;
        DeletionDate = null;
    }
}