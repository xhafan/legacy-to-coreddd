using System.Threading.Tasks;
using CoreDdd.Commands;
using CoreDddShared.Commands;
using Rebus.Bus;
using Rebus.Handlers;

namespace ServiceApp.CommandMessageHandlers
{
    public class CreateNewShipCommandMessageHandler : IHandleMessages<CreateNewShipCommand>
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IBus _bus;

        public CreateNewShipCommandMessageHandler(
            ICommandExecutor commandExecutor,
            IBus bus
        )
        {
            _bus = bus;
            _commandExecutor = commandExecutor;
        }

        public async Task Handle(CreateNewShipCommand command)
        {
            var createdShipId = 0;
            _commandExecutor.CommandExecuted += args => createdShipId = (int)args.Args;
            await _commandExecutor.ExecuteAsync(command);

            await _bus.Reply(new CreateNewShipCommandReply {CreatedShipId = createdShipId});
        }
    }
}