using CoreDdd.Commands;
using CoreDdd.Domain.Repositories;
using LegacyWebFormsApp.Domain;

namespace LegacyWebFormsApp.Commands
{
    public class CreateNewShipCommandHandler : BaseCommandHandler<CreateNewShipCommand>
    {
        private readonly IRepository<Ship> _shipRepository;

        public CreateNewShipCommandHandler(IRepository<Ship> shipRepository)
        {
            _shipRepository = shipRepository;
        }

        public override void Execute(CreateNewShipCommand command)
        {
            var newShip = new Ship(command.ShipName, command.Tonnage);
            _shipRepository.Save(newShip);

            RaiseCommandExecutedEvent(new CommandExecutedArgs { Args = newShip.Id });
        }
    }
}