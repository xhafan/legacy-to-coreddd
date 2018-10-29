using AspNetCoreMvcApp.Controllers;
using CoreDdd.Commands;
using CoreDdd.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMvcApp.IntegrationTests.Controllers.ManageShipsControllers
{
    public class ManageShipsControllerBuilder
    {
        private readonly ServiceProvider _serviceProvider;

        public ManageShipsControllerBuilder(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ManageShipsController Build()
        {
            var commandExecutor = new CommandExecutor(_serviceProvider.GetService<ICommandHandlerFactory>());
            var queryExecutor = new QueryExecutor(_serviceProvider.GetService<IQueryHandlerFactory>());
            return new ManageShipsController(commandExecutor, queryExecutor);
        }
    }
}