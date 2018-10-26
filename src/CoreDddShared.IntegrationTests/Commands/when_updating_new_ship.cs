using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.TestHelpers;
using CoreDddShared.Commands;
using CoreDddShared.Domain;
using NUnit.Framework;
using Shouldly;

namespace CoreDddShared.IntegrationTests.Commands
{
    [TestFixture]
    public class when_updating_new_ship
    {
        private PersistenceTestHelper _p;
        private Ship _updatedShip;

        [SetUp]
        public void Context()
        {
            _p = new PersistenceTestHelper(new CoreDddSharedNhibernateConfigurator());
            _p.BeginTransaction();

            var ship = new Ship("ship name", tonnage: 23.4m);
            _p.Save(ship);

            var updateShipCommand = new UpdateShipCommand
            {
                ShipId = ship.Id,
                ShipName = "updated ship name",
                Tonnage = 34.5m
            };
            var updateShipCommandHandler = new UpdateShipCommandHandler(new NhibernateRepository<Ship>(_p.UnitOfWork));
#if NET40
            updateShipCommandHandler.Execute(updateShipCommand);
#endif
#if !NET40
            updateShipCommandHandler.ExecuteAsync(updateShipCommand).Wait();
#endif

            _p.Flush();
            _p.Clear();

            _updatedShip = _p.Get<Ship>(ship.Id);
        }

        [Test]
        public void ship_data_are_updated()
        {
            _updatedShip.Name.ShouldBe("updated ship name");
            _updatedShip.Tonnage.ShouldBe(34.5m);
        }

        [TearDown]
        public void TearDown()
        {
            _p.Rollback();
        }
    }
}