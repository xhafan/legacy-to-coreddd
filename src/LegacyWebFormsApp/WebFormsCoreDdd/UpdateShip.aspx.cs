using System;
using CoreDdd.Commands;
using CoreIoC;
using LegacyWebFormsApp.Commands;

namespace LegacyWebFormsApp.WebFormsCoreDdd
{
    public partial class UpdateShip : System.Web.UI.Page
    {
        private ICommandExecutor _commandExecutor;

        protected void Page_Load(object sender, EventArgs e)
        {
            _commandExecutor = IoC.Resolve<ICommandExecutor>();
        }

        protected void UpdateShipButton_OnClick(object sender, EventArgs e)
        {
            var updateShipCommand = new UpdateShipCommand
            {
                ShipId = int.Parse(ShipIdTextBox.Text),
                ShipName = ShipNameTextBox.Text,
                Tonnage = decimal.Parse(TonnageTextBox.Text)
            };
            _commandExecutor.Execute(updateShipCommand);
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            IoC.Release(_commandExecutor);
        }
    }
}