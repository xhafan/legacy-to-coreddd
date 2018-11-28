using System.Threading.Tasks;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.TestHelpers;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddShared;
using CoreDddShared.Domain;
using CoreDddShared.Domain.Events;
using FakeItEasy;
using NUnit.Framework;
using ServiceApp.DomainEventMessageHandlers;
using Shouldly;
using TestsShared;

namespace ServiceApp.IntegrationTests.DomainEventMessageHandlers.VerifyImoNumberShipCreatedDomainEventMessageHandlers
{
    [TestFixture]
    public class when_handling_ship_created_event_message
    {
        private NhibernateUnitOfWork _unitOfWork;
        private Ship _newShip;

        [SetUp]
        public async Task Context()
        {
            _unitOfWork = new NhibernateUnitOfWork(new CoreDddSharedNhibernateConfigurator());
            _unitOfWork.BeginTransaction();

            var imoNumber = "IMO 7564321";
            _newShip = new ShipBuilder().WithImoNumber(imoNumber).Build();
            _unitOfWork.Save(_newShip);

            var internationalMaritimeOrganizationVerifier = A.Fake<IInternationalMaritimeOrganizationVerifier>();
            A.CallTo(() => internationalMaritimeOrganizationVerifier.IsImoNumberValid(imoNumber)).Returns(true);

            var shipCreatedDomainEventMessageHandler = new VerifyImoNumberShipCreatedDomainEventMessageHandler(
                new NhibernateRepository<Ship>(_unitOfWork),
                internationalMaritimeOrganizationVerifier
                );

            await shipCreatedDomainEventMessageHandler.Handle(new ShipCreatedDomainEventMessage {ShipId = _newShip.Id});

            _unitOfWork.Flush();
            _unitOfWork.Clear();
        }

        [Test]
        public void imo_number_is_verified_and_is_valid()
        {
            var ship = _unitOfWork.Get<Ship>(_newShip.Id);
            ship.HasImoNumberBeenVerified.ShouldBeTrue();
            ship.IsImoNumberValid.ShouldBeTrue();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork.Rollback();
        }
    }
}