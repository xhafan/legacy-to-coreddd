using System;
using CoreDdd.Commands;
using CoreDddShared.Commands;
using CoreIoC;

namespace LegacyWebFormsApp.WebFormsCoreDdd
{
    public partial class CreateShip : System.Web.UI.Page
    {
        private ICommandExecutor _commandExecutor;

        protected void Page_Load(object sender, EventArgs e)
        {
            _commandExecutor = IoC.Resolve<ICommandExecutor>();
        }

        protected void CreateShipButton_Click(object sender, EventArgs e)
        {
            var createNewShipCommand = new CreateNewShipCommand
            {
                ShipName = ShipNameTextBox.Text,
                Tonnage = decimal.Parse(TonnageTextBox.Text)
            };

            _commandExecutor.CommandExecuted += args =>
            {
                var generatedShipId = (int)args.Args;
                LastShipIdCreatedLabel.Text = $"{generatedShipId}";
            };
            _commandExecutor.Execute(createNewShipCommand);
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            IoC.Release(_commandExecutor);
        }
    }
}