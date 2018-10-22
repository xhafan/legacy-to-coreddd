using CoreDdd.Commands;
using CoreDdd.Domain.Repositories;
using LegacyWebFormsApp.Domain;

namespace LegacyWebFormsApp.Commands
{
    public class UpdateShipCommandHandler : BaseCommandHandler<UpdateShipCommand>
    {
        private readonly IRepository<Ship> _shipRepository;

        public UpdateShipCommandHandler(IRepository<Ship> shipRepository)
        {
            _shipRepository = shipRepository;
        }

        public override void Execute(UpdateShipCommand command)
        {
            var ship = _shipRepository.Get(command.ShipId);
            ship.UpdateData(command.ShipName, command.Tonnage);
        }
    }
}