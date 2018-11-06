using System.Threading.Tasks;
using CoreDddShared.Domain.Events;
using Rebus.Handlers;
using ServiceApp.WebServiceNotifications;

namespace ServiceApp
{
    public class ShipCreatedDomainEventMessageHandler : IHandleMessages<ShipCreatedDomainEventMessage>
    {
        private readonly IShipCreatedWebServiceNotification _shipCreatedWebServiceNotification;

        public ShipCreatedDomainEventMessageHandler(IShipCreatedWebServiceNotification shipCreatedWebServiceNotification)
        {
            _shipCreatedWebServiceNotification = shipCreatedWebServiceNotification;
        }

        public async Task Handle(ShipCreatedDomainEventMessage message)
        {
            await _shipCreatedWebServiceNotification.ShipCreatedNotification(message.ShipId);
        }
    }
}