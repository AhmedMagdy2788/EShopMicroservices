namespace Ordering.Domain.Abstractions;

public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
}