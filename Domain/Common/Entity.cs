using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;

public abstract class Entity
{
    public int Id { get; protected set; }
    
    private readonly List<IDomainEvent> _domainEvents = new();
    
    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected Entity() { }
    
    protected Entity(int id)
    {
        Id = id;
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity entity || GetType() != obj.GetType())
            return false;

        return Id == entity.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
