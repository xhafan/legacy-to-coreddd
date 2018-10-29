using System.Threading.Tasks;
using AspNetCoreMvcApp.Controllers;
using CoreDdd.Nhibernate.TestHelpers;
using CoreDddShared;
using CoreDddShared.Commands;
using CoreDddShared.Domain;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Shouldly;

namespace AspNetCoreMvcApp.IntegrationTests.Controllers.ManageShipsControllers
{
    [TestFixture]
    public class when_creating_new_ship
    {
        private PersistenceTestHelper _p;
        private IActionResult _actionResult;
        private int _shipCountBefore;

        [SetUp]
        public async Task Context()
        {
            _p = new PersistenceTestHelper(new CoreDddSharedNhibernateConfigurator());
            _p.BeginTransaction();

            _shipCountBefore = _GetShipCount();

            var manageShipsController = new ManageShipsControllerBuilder(_p.UnitOfWork).Build();

            var createNewShipCommand = new CreateNewShipCommand
            {
                ShipName = "ship name",
                Tonnage =  23.4m
            };
            _actionResult = await manageShipsController.CreateNewShip(createNewShipCommand);

            _p.Flush();
        }

        [Test]
        public void action_result_is_view_result_with_last_generated_ship_id_parameterer()
        {
            _actionResult.ShouldBeOfType<RedirectToActionResult>();
            var redirectToActionResult = (RedirectToActionResult) _actionResult;
            redirectToActionResult.ControllerName.ShouldBeNull();
            redirectToActionResult.ActionName.ShouldBe("CreateNewShip");
            redirectToActionResult.RouteValues.ContainsKey("lastCreatedShipId").ShouldBeTrue();
            ((int)redirectToActionResult.RouteValues["lastCreatedShipId"]).ShouldBeGreaterThan(0);
        }

        [Test]
        public void new_ship_is_created()
        {
            _GetShipCount().ShouldBe(_shipCountBefore + 1);
        }

        [TearDown]
        public void TearDown()
        {
            _p.Rollback();
        }

        private int _GetShipCount()
        {
            return _p.UnitOfWork.Session.QueryOver<Ship>().RowCount();
        }
    }
}