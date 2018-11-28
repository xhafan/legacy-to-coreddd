#if NETCOREAPP
using System.Threading.Tasks;
using CoreDdd.Domain.Events;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.TestHelpers;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.TestHelpers.DomainEvents;
using CoreDddShared.Commands;
using CoreDddShared.Domain;
using CoreDddShared.Domain.Events;
using NUnit.Framework;
using Shouldly;

namespace CoreDddShared.IntegrationTests.Commands.CreateNewShipCommandHandlers.AspNetCoreMvcApps
{
    [TestFixture]
    public class when_creating_new_ship
    {
        private NhibernateUnitOfWork _unitOfWork;
        private Ship _persistedShip;
        private int _createdShipId;
        private IDomainEvent _raisedDomainEvent;

        [SetUp]
        public async Task Context()
        {
            var domainEventHandlerFactory = new FakeDomainEventHandlerFactory(domainEvent => _raisedDomainEvent = domainEvent as IDomainEvent);
            DomainEvents.Initialize(domainEventHandlerFactory);

            _unitOfWork = new NhibernateUnitOfWork(new CoreDddSharedNhibernateConfigurator());
            _unitOfWork.BeginTransaction();

            var createNewShipCommand = new CreateNewShipCommand
            {
                ShipName = "ship name",
                Tonnage = 23.45678m,
                ImoNumber = "IMO 12345"
            };
            var createNewShipCommandHandler = new CreateNewShipCommandHandler(new NhibernateRepository<Ship>(_unitOfWork));
            createNewShipCommandHandler.CommandExecuted += args => _createdShipId = (int) args.Args;
            await createNewShipCommandHandler.ExecuteAsync(createNewShipCommand);

            _unitOfWork.Flush();
            _unitOfWork.Clear();

            _persistedShip = _unitOfWork.Get<Ship>(_createdShipId);
        }

        [Test]
        public void ship_can_be_retrieved_and_data_are_persisted_correctly()
        {
            _persistedShip.ShouldNotBeNull();
            _persistedShip.Name.ShouldBe("ship name");
            _persistedShip.Tonnage.ShouldBe(23.45678m);
            _persistedShip.ImoNumber.ShouldBe("IMO 12345");
        }

        [Test]
        public void ship_created_domain_event_is_raised()
        {
            _raisedDomainEvent.ShouldNotBeNull();
            _raisedDomainEvent.ShouldBeOfType<ShipCreatedDomainEvent>();
            var shipCreatedDomainEvent = (ShipCreatedDomainEvent) _raisedDomainEvent;
            shipCreatedDomainEvent.ShipId.ShouldBe(_createdShipId);
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork.Rollback();
        }
    }
}
#endif