using AspNetCoreMvcApp.BusRequestSenders;
using AspNetCoreMvcApp.Controllers;
using CoreDdd.Commands;
using CoreDdd.Queries;
using FakeItEasy;

namespace AspNetCoreMvcApp.Tests.Controllers.ManageShipsControllers
{
    public class ManageShipsControllerBuilder
    {
        private ICommandExecutor _commandExecutor;
        private IQueryExecutor _queryExecutor;
        private IBusRequestSender _busRequestSender;

        public ManageShipsControllerBuilder()
        {
            _commandExecutor = A.Fake<ICommandExecutor>();
            _queryExecutor = A.Fake<IQueryExecutor>();
            _busRequestSender = A.Fake<IBusRequestSender>();
        }

        public ManageShipsControllerBuilder WithCommandExecutor(ICommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
            return this;
        }

        public ManageShipsControllerBuilder WithQueryExecutor(IQueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
            return this;
        }

        public ManageShipsControllerBuilder WithBusRequestSender(IBusRequestSender busRequestSender)
        {
            _busRequestSender = busRequestSender;
            return this;
        }

        public ManageShipsController Build()
        {
            return new ManageShipsController(_commandExecutor, _queryExecutor, _busRequestSender);
        }
    }
}