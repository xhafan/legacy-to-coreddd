#if NETSTANDARD2_0 // AspNetCoreMvcApp
using CoreDdd.Commands;
using CoreDdd.Domain.Repositories;
using CoreDddShared.Domain;
using System.Threading.Tasks;

namespace CoreDddShared.Commands
{
    public class CreateNewShipCommandHandler : BaseCommandHandler<CreateNewShipCommand>
    {
        private readonly IRepository<Ship> _shipRepository;

        public CreateNewShipCommandHandler(IRepository<Ship> shipRepository)
        {
            _shipRepository = shipRepository;
        }
        
        public override async Task ExecuteAsync(CreateNewShipCommand command)
        {
            var newShip = new Ship(command.ShipName, command.Tonnage, command.ImoNumber);
            await _shipRepository.SaveAsync(newShip);
            newShip.OnCreationCompleted();

            RaiseCommandExecutedEvent(new CommandExecutedArgs { Args = newShip.Id });
        }    
    }
}
#endif

#if NET40 // LegacyWebFormsApp
using CoreDdd.Commands;
using CoreDdd.Domain.Repositories;
using CoreDddShared.Domain;

namespace CoreDddShared.Commands
{
    public class CreateNewShipCommandHandler : BaseCommandHandler<CreateNewShipCommand>
    {
        private readonly IRepository<Ship> _shipRepository;
        private readonly IInternationalMaritimeOrganizationVerifier _internationalMaritimeOrganizationVerifier;

        public CreateNewShipCommandHandler(
            IRepository<Ship> shipRepository,
            IInternationalMaritimeOrganizationVerifier internationalMaritimeOrganizationVerifier
            )
        {
            _internationalMaritimeOrganizationVerifier = internationalMaritimeOrganizationVerifier;
            _shipRepository = shipRepository;
        }

        public override void Execute(CreateNewShipCommand command)
        {
            var newShip = new Ship(command.ShipName, command.Tonnage, command.ImoNumber);
            _shipRepository.Save(newShip);
            newShip.VerifyImoNumber(_internationalMaritimeOrganizationVerifier);

            RaiseCommandExecutedEvent(new CommandExecutedArgs { Args = newShip.Id });
        }
    }
}
#endif