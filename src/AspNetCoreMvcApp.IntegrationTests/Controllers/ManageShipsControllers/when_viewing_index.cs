using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvcApp.Domain;
using AspNetCoreMvcApp.Dtos;
using CoreDdd.Nhibernate.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Shouldly;

namespace AspNetCoreMvcApp.IntegrationTests.Controllers.ManageShipsControllers
{
    [TestFixture]
    public class when_viewing_index
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

            _actionResult = await manageShipsController.Index();
        }

        [Test]
        public void action_result_is_view_result()
        {
            _actionResult.ShouldBeOfType<ViewResult>();
        }

        [Test]
        public void view_model_is_collection_of_ship_dtos()
        {
            var viewResult = (ViewResult)_actionResult;
            (viewResult.Model as IEnumerable<ShipDto>).ShouldNotBeNull();
            var shipDtos = ((IEnumerable<ShipDto>)viewResult.Model).ToList();
            shipDtos.Count.ShouldBe(1);
            shipDtos.Single().Id.ShouldBe(_newShip.Id);
        }

        [TearDown]
        public void TearDown()
        {
            _p.Rollback();
        }
    }
}