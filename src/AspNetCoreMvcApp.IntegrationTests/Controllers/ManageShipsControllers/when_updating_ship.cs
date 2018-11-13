using System.Threading.Tasks;
using CoreDdd.Nhibernate.TestHelpers;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddShared.Commands;
using CoreDddShared.Domain;
using IntegrationTestsShared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shouldly;
using TestsShared;

namespace AspNetCoreMvcApp.IntegrationTests.Controllers.ManageShipsControllers
{
    [TestFixture]
    public class when_updating_ship
    {
        private PersistenceTestHelper _p;
        private ServiceProvider _serviceProvider;
        private IServiceScope _serviceScope;

        private IActionResult _actionResult;
        private Ship _newShip;

        [SetUp]
        public async Task Context()
        {
            _serviceProvider = new ServiceProviderHelper().BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _p = new PersistenceTestHelper(_serviceProvider.GetService<NhibernateUnitOfWork>());
            _p.BeginTransaction();

            _newShip = new ShipBuilder().Build();
            _p.Save(_newShip);

            var manageShipsController = new ManageShipsControllerBuilder(_serviceProvider).Build();

            var updateShipCommand = new UpdateShipCommand
            {
                ShipId = _newShip.Id,
                ShipName = "updated ship name",
                Tonnage =  34.5m
            };
            _actionResult = await manageShipsController.UpdateShip(updateShipCommand);

            _p.Flush();
            _p.Clear();
        }

        [Test]
        public void ship_is_updated()
        {
            var updatedShip = _p.UnitOfWork.Session.Get<Ship>(_newShip.Id);
            updatedShip.Name.ShouldBe("updated ship name");
        }

        [Test]
        public void action_result_is_the_same_view()
        {
            _actionResult.ShouldBeOfType<RedirectToActionResult>();
            var redirectToActionResult = (RedirectToActionResult)_actionResult;
            redirectToActionResult.ControllerName.ShouldBeNull();
            redirectToActionResult.ActionName.ShouldBe("UpdateShip");
        }

        [TearDown]
        public void TearDown()
        {
            _p.Rollback();
            _serviceScope.Dispose();
            _serviceProvider.Dispose();
        }
    }
}