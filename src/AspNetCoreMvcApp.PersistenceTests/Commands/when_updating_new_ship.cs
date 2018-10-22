using System.Threading.Tasks;
using AspNetCoreMvcApp.Commands;
using AspNetCoreMvcApp.Domain;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.TestHelpers;
using NUnit.Framework;
using Shouldly;

namespace AspNetCoreMvcApp.PersistenceTests.Commands
{
    [TestFixture]
    public class when_updating_new_ship
    {
        private PersistenceTestHelper _p;
        private Ship _updatedShip;

        [SetUp]
        public async Task Context()
        {
            _p = new PersistenceTestHelper(new AspNetCoreAppNhibernateConfigurator());
            _p.BeginTransaction();

            var ship = new Ship("ship name", tonnage: 23.4m);
            _p.Save(ship);

            var updateShipCommand = new UpdateShipCommand
            {
                ShipId = ship.Id,
                ShipName = "updated ship name",
                Tonnage = 34.5m
            };
            var createNewShipCommandHandler = new UpdateShipCommandHandler(new NhibernateRepository<Ship>(_p.UnitOfWork));
            await createNewShipCommandHandler.ExecuteAsync(updateShipCommand);

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