namespace DDD.Domain.Entities.BaseEntities;

public abstract class AuditedEntity<T, Q> : Entity<T> where T : struct where Q : class
{
    public Q? CreatedBy { get; private set; }
    public Q? UpdatedBy { get; private set; }
    protected AuditedEntity(T id, Q? createdUserId) : base(id)
    {
        CreatedBy = createdUserId;
    }
    public void Update(Q updateBy)
    {
        UpdatedBy = updateBy;
    }
}
