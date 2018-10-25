using System;

namespace LegacyWebFormsApp.WebFormsAdoNet
{
    public partial class CreateShip : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void CreateShipButton_Click(object sender, EventArgs e)
        {
            var shipName = ShipNameTextBox.Text;
            var tonnage = decimal.Parse(TonnageTextBox.Text);

            SqlCommandExecutor.ExecuteSqlCommand(cmd =>
            {
                cmd.CommandText = $"EXEC CreateShip '{shipName}', {tonnage}";
                var shipId = (int)cmd.ExecuteScalar();
                LastShipIdCreatedLabel.Text = $"{shipId}";
            });
        }
    }
}