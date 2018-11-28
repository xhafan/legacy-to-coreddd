#if NET40
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.TestHelpers;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddShared.Commands;
using CoreDddShared.Domain;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;

namespace CoreDddShared.IntegrationTests.Commands.CreateNewShipCommandHandlers.LegacyWebFormsApps
{
    [TestFixture]
    public class when_creating_new_ship
    {
        private NhibernateUnitOfWork _unitOfWork;
        private Ship _persistedShip;
        private int _createdShipId;

        [SetUp]
        public void Context()
        {
            _unitOfWork = new NhibernateUnitOfWork(new CoreDddSharedNhibernateConfigurator());
            _unitOfWork.BeginTransaction();

            var createNewShipCommand = new CreateNewShipCommand
            {
                ShipName = "ship name",
                Tonnage = 23.45678m,
                ImoNumber = "IMO 765432"
            };

            var internationalMaritimeOrganizationVerifier = A.Fake<IInternationalMaritimeOrganizationVerifier>();
            A.CallTo(() => internationalMaritimeOrganizationVerifier.IsImoNumberValid("IMO 765432")).Returns(true);

            var createNewShipCommandHandler = new CreateNewShipCommandHandler(
                new NhibernateRepository<Ship>(_unitOfWork),
                internationalMaritimeOrganizationVerifier
                );
            createNewShipCommandHandler.CommandExecuted += args => _createdShipId = (int) args.Args;
            createNewShipCommandHandler.Execute(createNewShipCommand);

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
            _persistedShip.ImoNumber.ShouldBe("IMO 765432");
        }

        [Test]
        public void imo_number_is_verified_and_is_valid()
        {
            var ship = _unitOfWork.Get<Ship>(_persistedShip.Id);
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
#endif