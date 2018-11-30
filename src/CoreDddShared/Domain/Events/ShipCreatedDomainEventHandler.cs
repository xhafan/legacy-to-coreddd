#if NETSTANDARD // ShipCreatedDomainEventHandler will only work in AspNetCoreMvcApp and not in LegacyWebFormsApp
using CoreDdd.Domain.Events;
using Rebus.Bus.Advanced;

namespace CoreDddShared.Domain.Events
{
    public class ShipCreatedDomainEventHandler : IDomainEventHandler<ShipCreatedDomainEvent>
    {
        private readonly ISyncBus _bus;

        public ShipCreatedDomainEventHandler(ISyncBus bus)
        {
            _bus = bus;
        }

        public void Handle(ShipCreatedDomainEvent domainEvent)
        {
            _bus.Publish(new ShipCreatedDomainEventMessage {ShipId = domainEvent.ShipId});
        }
    }
}
#endif
