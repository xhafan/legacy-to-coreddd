using System;
using System.Web.UI;
using CoreDdd.Commands;
using CoreDdd.Queries;
using CoreIoC;
using LegacyWebFormsApp.Commands;
using LegacyWebFormsApp.Dtos;
using LegacyWebFormsApp.Queries;

namespace LegacyWebFormsApp
{
    public partial class ManageShipsWebFormsCoreDdd : Page
    {
        private ICommandExecutor _commandExecutor;
        private IQueryExecutor _queryExecutor;

        protected void Page_Load(object sender, EventArgs e)
        {
            _commandExecutor = IoC.Resolve<ICommandExecutor>();
            _queryExecutor = IoC.Resolve<IQueryExecutor>();

            if (!IsPostBack)
            {
                _LoadShips();
            }
        }

        protected void CreateShipButton_Click(object sender, EventArgs e)
        {
            var createNewShipCommand = new CreateNewShipCommand
            {
                ShipName = CreateShipNameTextBox.Text,
                Tonnage = decimal.Parse(CreateTonnageTextBox.Text)
            };

            _commandExecutor.CommandExecuted += args =>
            {
                var generatedShipId = (int)args.Args;
                LastShipIdCreatedLabel.Text = $"{generatedShipId}";
            };
            _commandExecutor.Execute(createNewShipCommand);

            Response.Redirect(Request.RawUrl);
        }

        protected void UpdateShipButton_OnClickShipButton_Click(object sender, EventArgs e)
        {
            var updateShipCommand = new UpdateShipCommand
            {
                ShipId = int.Parse(UpdateShipIdTextBox.Text),
                ShipName = UpdateShipNameTextBox.Text,
                Tonnage = decimal.Parse(UpdateTonnageTextBox.Text)
            };
            _commandExecutor.Execute(updateShipCommand);

            Response.Redirect(Request.RawUrl);
        }

        private void _LoadShips()
        {
            ExistingShipsListBox.Items.Clear();

            var getAllShipsQuery = new GetAllShipsQuery();
            var shipDtos = _queryExecutor.Execute<GetAllShipsQuery, ShipDto>(getAllShipsQuery);

            foreach (var shipDto in shipDtos)
            {
                ExistingShipsListBox.Items.Add($"Id: {shipDto.Id}, Name: {shipDto.Name}, Tonnage: {shipDto.Tonnage}");
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            IoC.Release(_commandExecutor);
            IoC.Release(_queryExecutor);
        }
    }
}