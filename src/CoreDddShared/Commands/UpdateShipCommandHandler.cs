using System.Threading.Tasks;
using CoreDdd.Commands;
using CoreDdd.Domain.Repositories;
using CoreDddShared.Domain;

namespace CoreDddShared.Commands
{
    public class UpdateShipCommandHandler : BaseCommandHandler<UpdateShipCommand>
    {
        private readonly IRepository<Ship> _shipRepository;

        public UpdateShipCommandHandler(IRepository<Ship> shipRepository)
        {
            _shipRepository = shipRepository;
        }

#if NET40
        // sync Execute for Web Forms app - .NET 4
        public override void Execute(UpdateShipCommand command)
        {
            var ship = _shipRepository.Get(command.ShipId);
            ship.UpdateData(command.ShipName, command.Tonnage);
        }
#endif

#if !NET40
        // async ExecuteAsync for ASP.NET Core MVC app - .NET 4.5+ and .NET Core
        public override async Task ExecuteAsync(UpdateShipCommand command)
        {
            var ship = await _shipRepository.GetAsync(command.ShipId);
            ship.UpdateData(command.ShipName, command.Tonnage);
        }
#endif
    }
}