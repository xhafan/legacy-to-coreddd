using CoreDdd.Nhibernate.UnitOfWorks;
using IntegrationTestsShared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shouldly;

namespace AspNetCoreMvcApp.IntegrationTests.Controllers.ManageShipsControllers
{
    [TestFixture]
    public class when_viewing_create_new_ship
    {
        private NhibernateUnitOfWork _unitOfWork;
        private ServiceProvider _serviceProvider;
        private IServiceScope _serviceScope;

        private IActionResult _actionResult;

        [SetUp]
        public void Context()
        {
            _serviceProvider = new ServiceProviderHelper().BuildServiceProvider();
            _serviceScope = _serviceProvider.CreateScope();

            _unitOfWork = _serviceProvider.GetService<NhibernateUnitOfWork>();
            _unitOfWork.BeginTransaction();

            var manageShipsController = new ManageShipsControllerBuilder(_serviceProvider).Build();

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
            _unitOfWork.Rollback();
            _serviceScope.Dispose();
            _serviceProvider.Dispose();
        }
    }
}