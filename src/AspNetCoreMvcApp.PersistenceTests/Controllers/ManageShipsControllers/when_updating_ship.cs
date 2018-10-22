﻿using System.Threading.Tasks;
using AspNetCoreMvcApp.Commands;
using AspNetCoreMvcApp.Domain;
using CoreDdd.Nhibernate.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Shouldly;

namespace AspNetCoreMvcApp.PersistenceTests.Controllers.ManageShipsControllers
{
    [TestFixture]
    public class when_updating_ship
    {
        private PersistenceTestHelper _p;
        private IActionResult _actionResult;
        private Ship _newShip;

        [SetUp]
        public async Task Context()
        {
            _p = new PersistenceTestHelper(new AspNetCoreAppNhibernateConfigurator());
            _p.BeginTransaction();

            _newShip = new Ship("ship name", tonnage: 23.4m);
            _p.Save(_newShip);

            var manageShipsController = new ManageShipsControllerBuilder(_p.UnitOfWork).Build();

            var updateShipCommand = new UpdateShipCommand
            {
                ShipId = _newShip.Id,
                ShipName = "updated ship name",
                Tonnage =  34.5m
            };
            _actionResult = await manageShipsController.UpdateShip(updateShipCommand);

            _p.Flush();
        }

        [Test]
        public void request_is_redirected_to_index()
        {
            _actionResult.ShouldBeOfType<RedirectToActionResult>();
            var redirectToActionResult = (RedirectToActionResult) _actionResult;
            redirectToActionResult.ControllerName.ShouldBe(null);
            redirectToActionResult.ActionName.ShouldBe("Index");
        }

        [Test]
        public void ship_is_updated()
        {
            var updatedShip = _p.UnitOfWork.Session.Get<Ship>(_newShip.Id);
            updatedShip.Name.ShouldBe("updated ship name");
        }

        [TearDown]
        public void TearDown()
        {
            _p.Rollback();
        }
    }
}