using System.Threading.Tasks;
using AspNetCoreMvcApp.BusRequestSenders;
using CoreDddShared.Commands;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Shouldly;

namespace AspNetCoreMvcApp.Tests.Controllers.ManageShipsControllers
{
    [TestFixture]
    public class when_creating_new_ship_reliably
    {
        private IActionResult _actionResult;
        private CreateNewShipCommand _createNewShipCommand;
        private IBusRequestSender _busRequestSender;
        private const int CreatedShipId = 34;

        [SetUp]
        public async Task Context()
        {
            _createNewShipCommand = new CreateNewShipCommand
            {
                ShipName = "ship name",
                Tonnage = 23.4m,
                ImoNumber = "IMO 12345"
            };

            var createNewShipCommandReply = new CreateNewShipCommandReply {CreatedShipId = CreatedShipId };

            _busRequestSender = A.Fake<IBusRequestSender>();
            A.CallTo(() => _busRequestSender.SendRequest<CreateNewShipCommandReply>(_createNewShipCommand)).Returns(createNewShipCommandReply);

            var manageShipsController = new ManageShipsControllerBuilder()
                .WithBusRequestSender(_busRequestSender)
                .Build();

            _actionResult = await manageShipsController.CreateNewShipReliably(_createNewShipCommand);
        }

        [Test]
        public void action_result_is_redirect_to_action_result_with_last_generated_ship_id_parameterer()
        {
            _actionResult.ShouldBeOfType<RedirectToActionResult>();
            var redirectToActionResult = (RedirectToActionResult)_actionResult;
            redirectToActionResult.ControllerName.ShouldBeNull();
            redirectToActionResult.ActionName.ShouldBe("CreateNewShip");
            redirectToActionResult.RouteValues.ShouldNotBeNull();
            redirectToActionResult.RouteValues.ContainsKey("lastCreatedShipId").ShouldBeTrue();
            ((int)redirectToActionResult.RouteValues["lastCreatedShipId"]).ShouldBe(CreatedShipId);
        }
    }
}