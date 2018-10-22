using System;
using AspNetCoreMvcApp.Commands;
using AspNetCoreMvcApp.Domain;
using CoreDdd.Commands;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;

namespace AspNetCoreMvcApp.PersistenceTests.Controllers.ManageShipsControllers
{
    public class FakeCommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly NhibernateUnitOfWork _unitOfWork;

        public FakeCommandHandlerFactory(NhibernateUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        ICommandHandler<TCommand> ICommandHandlerFactory.Create<TCommand>()
        {
            if (typeof(TCommand) == typeof(CreateNewShipCommand))
            {
                return (ICommandHandler<TCommand>)new CreateNewShipCommandHandler(new NhibernateRepository<Ship>(_unitOfWork));
            }
            if (typeof(TCommand) == typeof(UpdateShipCommand))
            {
                return (ICommandHandler<TCommand>)new UpdateShipCommandHandler(new NhibernateRepository<Ship>(_unitOfWork));
            }
            throw new Exception("Unsupported command");
        }

        public void Release<TCommand>(ICommandHandler<TCommand> commandHandler) where TCommand : ICommand
        {
        }
    }
}