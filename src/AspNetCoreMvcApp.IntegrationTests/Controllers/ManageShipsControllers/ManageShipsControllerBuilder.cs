using AspNetCoreMvcApp.Controllers;
using CoreDdd.Commands;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Queries;

namespace AspNetCoreMvcApp.IntegrationTests.Controllers.ManageShipsControllers
{
    public class ManageShipsControllerBuilder
    {
        private readonly NhibernateUnitOfWork _unitOfWork;

        public ManageShipsControllerBuilder(NhibernateUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ManageShipsController Build()
        {
            var commandExecutor = new CommandExecutor(new FakeCommandHandlerFactory(_unitOfWork));
            var queryExecutor = new QueryExecutor(new FakeQueryHandlerFactory(_unitOfWork));
            return new ManageShipsController(commandExecutor, queryExecutor);
        }
    }
}