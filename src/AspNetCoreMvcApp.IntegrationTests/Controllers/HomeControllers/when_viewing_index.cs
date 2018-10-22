using AspNetCoreMvcApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Shouldly;

namespace AspNetCoreMvcApp.IntegrationTests.Controllers.HomeControllers
{
    [TestFixture]
    public class when_viewing_index
    {
        private IActionResult _actionResult;

        [SetUp]
        public void Context()
        {
            var homeController = new HomeController();

            _actionResult = homeController.Index();
        }

        [Test]
        public void request_is_redirected_to_manage_ships_index()
        {
            _actionResult.ShouldBeOfType<RedirectToActionResult>();
            var redirectToActionResult = (RedirectToActionResult)_actionResult;
            redirectToActionResult.ControllerName.ShouldBe("ManageShips");
            redirectToActionResult.ActionName.ShouldBe("Index");
        }
    }
}