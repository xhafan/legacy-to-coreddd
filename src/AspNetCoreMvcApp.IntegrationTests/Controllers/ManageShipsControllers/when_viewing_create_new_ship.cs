using CoreDdd.Nhibernate.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Shouldly;

namespace AspNetCoreMvcApp.IntegrationTests.Controllers.ManageShipsControllers
{
    [TestFixture]
    public class when_viewing_create_new_ship
    {
        private PersistenceTestHelper _p;
        private IActionResult _actionResult;

        [SetUp]
        public void Context()
        {
            _p = new PersistenceTestHelper(new AspNetCoreAppNhibernateConfigurator());
            _p.BeginTransaction();

            var manageShipsController = new ManageShipsControllerBuilder(_p.UnitOfWork).Build();

            _actionResult = manageShipsController.CreateNewShip();
        }

        [Test]
        public void action_result_is_view_result()
        {
            _actionResult.ShouldBeOfType<ViewResult>();
        }

        [TearDown]
        public void TearDown()
        {
            _p.Rollback();
        }
    }
}