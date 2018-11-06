using CoreDdd.Domain.Events;

namespace CoreDddShared.Domain.Events
{
    public class ShipCreatedDomainEvent : IDomainEvent
    {
        public int ShipId { get; set; }
    }
}