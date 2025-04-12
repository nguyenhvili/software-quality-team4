using System.ComponentModel.DataAnnotations.Schema;

namespace StocksReporting.Domain.Common;

public class BaseEntity
{
    private readonly List<DomainEvent> _domainEvents = [];

    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.ToList();

    protected void RaiseEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearEvents() => _domainEvents.Clear();
}
