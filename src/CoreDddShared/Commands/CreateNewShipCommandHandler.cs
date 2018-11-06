using System.Threading.Tasks;
using CoreDdd.Commands;
using CoreDdd.Domain.Events;
using CoreDdd.Domain.Repositories;
using CoreDddShared.Domain;

namespace CoreDddShared.Commands
{
    public class CreateNewShipCommandHandler : BaseCommandHandler<CreateNewShipCommand>
    {
        private readonly IRepository<Ship> _shipRepository;

        public CreateNewShipCommandHandler(IRepository<Ship> shipRepository)
        {
            _shipRepository = shipRepository;
        }
#if NET40
        // sync Execute for Web Forms app - .NET 4
        public override void Execute(CreateNewShipCommand command)
        {
            var newShip = new Ship(command.ShipName, command.Tonnage);
            _shipRepository.Save(newShip);
            newShip.OnCreationCompleted();

            RaiseCommandExecutedEvent(new CommandExecutedArgs {Args = newShip.Id});
        }
#endif

#if !NET40
        // async ExecuteAsync for ASP.NET Core MVC app - .NET 4.5+ and .NET Core
        public override async Task ExecuteAsync(CreateNewShipCommand command)
        {
            var newShip = new Ship(command.ShipName, command.Tonnage);
            await _shipRepository.SaveAsync(newShip);
            newShip.OnCreationCompleted();

            RaiseCommandExecutedEvent(new CommandExecutedArgs { Args = newShip.Id });
        }
#endif
    }
}