using AspNetCoreMvcApp.BusRequestSenders;
using AspNetCoreMvcApp.Controllers;
using CoreDdd.Commands;
using CoreDdd.Queries;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMvcApp.IntegrationTests.Controllers.ManageShipsControllers
{
    public class ManageShipsControllerBuilder
    {
        private readonly ServiceProvider _serviceProvider;
        private IBusRequestSender _busRequestSender;

        public ManageShipsControllerBuilder(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _busRequestSender = A.Fake<IBusRequestSender>();
        }

        public ManageShipsControllerBuilder WithBus(IBusRequestSender busRequestSender)
        {
            _busRequestSender = busRequestSender;
            return this;
        }

        public ManageShipsController Build()
        {
            var commandExecutor = new CommandExecutor(_serviceProvider.GetService<ICommandHandlerFactory>());
            var queryExecutor = new QueryExecutor(_serviceProvider.GetService<IQueryHandlerFactory>());
            return new ManageShipsController(commandExecutor, queryExecutor, _busRequestSender);
        }
    }
}