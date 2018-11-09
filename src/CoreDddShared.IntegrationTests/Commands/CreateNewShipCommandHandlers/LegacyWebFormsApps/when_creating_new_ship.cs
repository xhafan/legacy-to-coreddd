#if NET40
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.TestHelpers;
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
        private PersistenceTestHelper _p;
        private Ship _persistedShip;
        private int _generatedShipId;

        [SetUp]
        public void Context()
        {
            _p = new PersistenceTestHelper(new CoreDddSharedNhibernateConfigurator());
            _p.BeginTransaction();

            var createNewShipCommand = new CreateNewShipCommand
            {
                ShipName = "ship name",
                Tonnage = 23.45678m,
                ImoNumber = "IMO 765432"
            };

            var internationalMaritimeOrganizationVerifier = A.Fake<IInternationalMaritimeOrganizationVerifier>();
            A.CallTo(() => internationalMaritimeOrganizationVerifier.IsImoNumberValid("IMO 765432")).Returns(true);

            var createNewShipCommandHandler = new CreateNewShipCommandHandler(
                new NhibernateRepository<Ship>(_p.UnitOfWork),
                internationalMaritimeOrganizationVerifier
                );
            createNewShipCommandHandler.CommandExecuted += args => _generatedShipId = (int) args.Args;
            createNewShipCommandHandler.Execute(createNewShipCommand);

            _p.Flush();
            _p.Clear();

            _persistedShip = _p.Get<Ship>(_generatedShipId);
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
            var ship = _p.Get<Ship>(_persistedShip.Id);
            ship.HasImoNumberBeenVerified.ShouldBeTrue();
            ship.IsImoNumberValid.ShouldBeTrue();
        }

        [TearDown]
        public void TearDown()
        {
            _p.Rollback();
        }
    }
}
#endif