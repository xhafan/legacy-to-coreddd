#if NETCOREAPP
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreDdd.Commands;
using CoreDdd.Domain.Events;
using CoreDdd.Nhibernate.TestHelpers;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddShared.Commands;
using FakeItEasy;
using IntegrationTestsShared;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Rebus.Bus;
using ServiceApp.CommandMessageHandlers;
using Shouldly;

namespace ServiceApp.IntegrationTests.CommandMessageHandlers.CreateNewShipCommandMessageHandlers
{
    [TestFixture]
    public class when_handling_create_new_ship_command_message
    {
        private PersistenceTestHelper _p;
        private ServiceProvider _serviceProvider;
        private IBus _bus;

        [SetUp]
        public async Task Context()
        {
            _serviceProvider = new ServiceProviderHelper().BuildServiceProvider();
            DomainEvents.Initialize(_serviceProvider.GetService<IDomainEventHandlerFactory>());

            _p = new PersistenceTestHelper(_serviceProvider.GetService<NhibernateUnitOfWork>());
            _p.BeginTransaction();

            var createNewShipCommand = new CreateNewShipCommand
            {
                ShipName = "ship name",
                Tonnage = 23.45678m,
                ImoNumber = "IMO 12345"
            };

            _bus = A.Fake<IBus>();
            var createNewShipCommandMessageHandler = new CreateNewShipCommandMessageHandler(_serviceProvider.GetService<ICommandExecutor>(), _bus);

            await createNewShipCommandMessageHandler.Handle(createNewShipCommand);

            _p.Flush();
            _p.Clear();
        }

        [Test]
        public void bus_replied_with_created_ship_id()
        {
            A.CallTo(() => _bus.Reply(
                A<CreateNewShipCommandReply>.That.Matches(p => _MachingReply(p)),
                A<Dictionary<string, string>>._
                )).MustHaveHappened();
        }

        private bool _MachingReply(CreateNewShipCommandReply createNewShipCommandReply)
        {
            createNewShipCommandReply.CreatedShipId.ShouldBeGreaterThan(0);
            return true;
        }

        [TearDown]
        public void TearDown()
        {
            _p.Rollback();
        }
    }
}
#endif