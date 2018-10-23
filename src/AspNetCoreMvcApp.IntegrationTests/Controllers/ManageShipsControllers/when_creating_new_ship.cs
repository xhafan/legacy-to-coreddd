using System.Threading.Tasks;
using AspNetCoreMvcApp.Commands;
using AspNetCoreMvcApp.Controllers;
using AspNetCoreMvcApp.Domain;
using CoreDdd.Nhibernate.TestHelpers;
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
            _p = new PersistenceTestHelper(new AspNetCoreAppNhibernateConfigurator());
            _p.BeginTransaction();

            _shipCountBefore = _GetShipCount();

            var manageShipsController = new ManageShipsControllerBuilder(_p.UnitOfWork).Build();

            var createNewShipCommand = new CreateNewShipCommand
            {
                ShipName = "ship name",
                Tonnage =  23.4m
            };
            _actionResult = await manageShipsController.CreateNewShip(
                new CreateNewShipViewModel {CreateNewShipCommand = createNewShipCommand}
            );

            _p.Flush();
        }

        [Test]
        public void action_result_is_view_result_with_last_generated_ship_id_parameterer()
        {
            _actionResult.ShouldBeOfType<ViewResult>();
            var viewResult = (ViewResult) _actionResult;
            viewResult.ViewName.ShouldBeNull();
            viewResult.Model.ShouldBeOfType<CreateNewShipViewModel>();
            var createNewShipViewModel = (CreateNewShipViewModel)viewResult.Model;
            createNewShipViewModel.LastCreatedShipId.ShouldNotBeNull();
            createNewShipViewModel.LastCreatedShipId.Value.ShouldBeGreaterThan(0);
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