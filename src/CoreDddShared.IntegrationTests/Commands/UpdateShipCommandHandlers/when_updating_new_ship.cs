using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.TestHelpers;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddShared.Commands;
using CoreDddShared.Domain;
using NUnit.Framework;
using Shouldly;
using TestsShared;

namespace CoreDddShared.IntegrationTests.Commands.UpdateShipCommandHandlers
{
    [TestFixture]
    public class when_updating_new_ship
    {
        private NhibernateUnitOfWork _unitOfWork;
        private Ship _updatedShip;

        [SetUp]
        public void Context()
        {
            _unitOfWork = new NhibernateUnitOfWork(new CoreDddSharedNhibernateConfigurator());
            _unitOfWork.BeginTransaction();

            var ship = new ShipBuilder().Build();
            _unitOfWork.Save(ship);

            var updateShipCommand = new UpdateShipCommand
            {
                ShipId = ship.Id,
                ShipName = "updated ship name",
                Tonnage = 34.5m
            };
            var updateShipCommandHandler = new UpdateShipCommandHandler(new NhibernateRepository<Ship>(_unitOfWork));
#if NET40
            updateShipCommandHandler.Execute(updateShipCommand);
#endif
#if NETCOREAPP
            updateShipCommandHandler.ExecuteAsync(updateShipCommand).Wait();
#endif

            _unitOfWork.Flush();
            _unitOfWork.Clear();

            _updatedShip = _unitOfWork.Get<Ship>(ship.Id);
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
            _unitOfWork.Rollback();
        }
    }
}