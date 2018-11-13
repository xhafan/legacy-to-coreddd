using System.Threading.Tasks;
using CoreDdd.Domain.Events;
using CoreDdd.Nhibernate.TestHelpers;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddShared.Commands;
using CoreDddShared.Domain;
using IntegrationTestsShared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shouldly;

namespace AspNetCoreMvcApp.IntegrationTests.Controllers.ManageShipsControllers
{
    [TestFixture]
    public class when_creating_new_ship
    {
        private PersistenceTestHelper _p;
        private ServiceProvider _serviceProvider;
        private IServiceScope _serviceScope;

        private IActionResult _actionResult;
        private int _shipCountBefore;

        [SetUp]
        public async Task Context()
        {
            _serviceProvider = new ServiceProviderHelper().BuildServiceProvider();
            DomainEvents.Initialize(_serviceProvider.GetService<IDomainEventHandlerFactory>());

            _serviceScope = _serviceProvider.CreateScope();

            _p = new PersistenceTestHelper(_serviceProvider.GetService<NhibernateUnitOfWork>());
            _p.BeginTransaction();

            _shipCountBefore = _GetShipCount();

            var manageShipsController = new ManageShipsControllerBuilder(_serviceProvider).Build();

            var createNewShipCommand = new CreateNewShipCommand
            {
                ShipName = "ship name",
                Tonnage =  23.4m,
                ImoNumber = "IMO 12345"
            };
            _actionResult = await manageShipsController.CreateNewShip(createNewShipCommand);

            _p.Flush();
            _p.Clear();
        }

        [Test]
        public void new_ship_is_created()
        {
            _GetShipCount().ShouldBe(_shipCountBefore + 1);
        }

        [Test]
        public void action_result_is_redirect_to_action_result_with_last_created_ship_id_parameterer()
        {
            _actionResult.ShouldBeOfType<RedirectToActionResult>();
            var redirectToActionResult = (RedirectToActionResult) _actionResult;
            redirectToActionResult.ControllerName.ShouldBeNull();
            redirectToActionResult.ActionName.ShouldBe("CreateNewShip");
            redirectToActionResult.RouteValues.ShouldNotBeNull();
            redirectToActionResult.RouteValues.ContainsKey("lastCreatedShipId").ShouldBeTrue();
            ((int)redirectToActionResult.RouteValues["lastCreatedShipId"]).ShouldBeGreaterThan(0);
        }

        [TearDown]
        public void TearDown()
        {
            _p.Rollback();
            _serviceScope.Dispose();
            _serviceProvider.Dispose();
        }

        private int _GetShipCount()
        {
            return _p.UnitOfWork.Session.QueryOver<Ship>().RowCount();
        }
    }
}