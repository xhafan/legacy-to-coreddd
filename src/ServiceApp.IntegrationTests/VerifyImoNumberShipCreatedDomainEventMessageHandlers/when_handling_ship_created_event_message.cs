using System.Threading.Tasks;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.TestHelpers;
using CoreDddShared;
using CoreDddShared.Domain;
using CoreDddShared.Domain.Events;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;
using TestsShared;

namespace ServiceApp.IntegrationTests.VerifyImoNumberShipCreatedDomainEventMessageHandlers
{
    [TestFixture]
    public class when_handling_ship_created_event_message
    {
        private PersistenceTestHelper _p;
        private Ship _newShip;

        [SetUp]
        public async Task Context()
        {
            _p = new PersistenceTestHelper(new CoreDddSharedNhibernateConfigurator());
            _p.BeginTransaction();

            var imoNumber = "IMO 7564321";
            _newShip = new ShipBuilder().WithImoNumber(imoNumber).Build();
            _p.Save(_newShip);

            var internationalMaritimeOrganizationVerifier = A.Fake<IInternationalMaritimeOrganizationVerifier>();
            A.CallTo(() => internationalMaritimeOrganizationVerifier.IsImoNumberValid(imoNumber)).Returns(true);

            var shipCreatedDomainEventMessageHandler = new VerifyImoNumberShipCreatedDomainEventMessageHandler(
                new NhibernateRepository<Ship>(_p.UnitOfWork),
                internationalMaritimeOrganizationVerifier
                );

            await shipCreatedDomainEventMessageHandler.Handle(new ShipCreatedDomainEventMessage {ShipId = _newShip.Id});

            _p.Flush();
            _p.Clear();
        }

        [Test]
        public void imo_number_is_verified()
        {
            var ship = _p.Get<Ship>(_newShip.Id);
            ship.IsImoNumberVerified.ShouldBeTrue();
        }

        [TearDown]
        public void TearDown()
        {
            _p.Rollback();
        }
    }
}