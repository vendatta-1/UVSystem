using System.ComponentModel.DataAnnotations.Schema;

namespace UVS.Common.Domain;

public abstract class Entity
{
    private List<IDomainEvent> _domainEvents = [];
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }
    
    
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    protected Entity()
    {}
    
    protected void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
    public void ClearDomainEvents()=> _domainEvents.Clear();
    
}