using System.Threading.Tasks;
using CoreDdd.Domain.Repositories;
using CoreDddShared.Domain;
using CoreDddShared.Domain.Events;
using Rebus.Handlers;

namespace ServiceApp.MessageHandlers
{
    public class VerifyImoNumberShipCreatedDomainEventMessageHandler : IHandleMessages<ShipCreatedDomainEventMessage>
    {
        private readonly IRepository<Ship> _shipRepository;
        private readonly IInternationalMaritimeOrganizationVerifier _internationalMaritimeOrganizationVerifier;

        public VerifyImoNumberShipCreatedDomainEventMessageHandler(
            IRepository<Ship> shipRepository,
            IInternationalMaritimeOrganizationVerifier internationalMaritimeOrganizationVerifier
            )
        {
            _shipRepository = shipRepository;
            _internationalMaritimeOrganizationVerifier = internationalMaritimeOrganizationVerifier;
        }

        public async Task Handle(ShipCreatedDomainEventMessage message)
        {
            var ship = await _shipRepository.GetAsync(message.ShipId);
            await ship.VerifyImoNumber(_internationalMaritimeOrganizationVerifier);
        }
    }
}