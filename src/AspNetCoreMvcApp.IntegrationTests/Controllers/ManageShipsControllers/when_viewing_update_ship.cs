using CoreDdd.Nhibernate.TestHelpers;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddShared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shouldly;

namespace AspNetCoreMvcApp.IntegrationTests.Controllers.ManageShipsControllers
{
    [TestFixture]
    public class when_viewing_update_ship
    {
        private PersistenceTestHelper _p;
        private ServiceProvider _serviceProvider;
        private IServiceScope _serviceScope;

        private IActionResult _actionResult;

        [SetUp]
        public void Context()
        {
            _serviceProvider = new ServiceProviderHelper().BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _p = new PersistenceTestHelper(_serviceProvider.GetService<NhibernateUnitOfWork>());
            _p.BeginTransaction();

            var manageShipsController = new ManageShipsControllerBuilder(_serviceProvider).Build();

            _actionResult = manageShipsController.UpdateShip();
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
            _serviceScope.Dispose();
            _serviceProvider.Dispose();
        }
    }
}