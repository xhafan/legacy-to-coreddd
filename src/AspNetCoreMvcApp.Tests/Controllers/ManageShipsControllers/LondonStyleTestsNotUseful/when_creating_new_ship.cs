using System;
using System.Threading.Tasks;
using CoreDdd.Commands;
using CoreDdd.Queries;
using CoreDddShared.Commands;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Shouldly;

namespace AspNetCoreMvcApp.Tests.Controllers.ManageShipsControllers.LondonStyleTestsNotUseful
{
    [TestFixture]
    public class when_creating_new_ship
    {
        private IActionResult _actionResult;
        private ICommandExecutor _commandExecutor;
        private CreateNewShipCommand _createNewShipCommand;
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

            _commandExecutor = A.Fake<ICommandExecutor>();
            _FakeThatWhenCommandIsExecutedTheCommandExecutedEventIsRaisedWithCreatedShipIdAsEventArgs();
            var queryExecutor = A.Fake<IQueryExecutor>();

            var manageShipsController = new ManageShipsControllerBuilder()
                .WithCommandExecutor(_commandExecutor)
                .WithQueryExecutor(queryExecutor)
                .Build();

            _actionResult = await manageShipsController.CreateNewShip(_createNewShipCommand);
        }

        [Test]
        public void command_is_executed()
        {
            A.CallTo(() => _commandExecutor.ExecuteAsync(_createNewShipCommand)).MustHaveHappened();
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

        // This method is simulating "what would happen in real command executor"
        private void _FakeThatWhenCommandIsExecutedTheCommandExecutedEventIsRaisedWithCreatedShipIdAsEventArgs()
        {
            A.CallTo(() => _commandExecutor.ExecuteAsync(_createNewShipCommand)).Invokes(() =>
            {
                _commandExecutor.CommandExecuted +=
                    Raise.FreeForm<Action<CommandExecutedArgs>>.With(new CommandExecutedArgs { Args = CreatedShipId });
            });
        }
    }
}