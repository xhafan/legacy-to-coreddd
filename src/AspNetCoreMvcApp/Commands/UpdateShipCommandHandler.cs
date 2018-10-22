using System.Threading.Tasks;
using AspNetCoreMvcApp.Domain;
using CoreDdd.Commands;
using CoreDdd.Domain.Repositories;

namespace AspNetCoreMvcApp.Commands
{
    public class UpdateShipCommandHandler : BaseCommandHandler<UpdateShipCommand>
    {
        private readonly IRepository<Ship> _shipRepository;

        public UpdateShipCommandHandler(IRepository<Ship> shipRepository)
        {
            _shipRepository = shipRepository;
        }

        public override async Task ExecuteAsync(UpdateShipCommand command)
        {
            var ship = await _shipRepository.GetAsync(command.ShipId);
            ship.UpdateData(command.ShipName, command.Tonnage);
        }
    }
}