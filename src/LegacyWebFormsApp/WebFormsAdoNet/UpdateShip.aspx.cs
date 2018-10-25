using System;

namespace LegacyWebFormsApp.WebFormsAdoNet
{
    public partial class UpdateShip : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void UpdateShipButton_OnClick(object sender, EventArgs e)
        {
            var shipId = ShipIdTextBox.Text;
            var shipName = ShipNameTextBox.Text;
            var tonnage = decimal.Parse(TonnageTextBox.Text);

            SqlCommandExecutor.ExecuteSqlCommand(cmd =>
            {
                cmd.CommandText = $"EXEC UpdateShip {shipId}, '{shipName}', {tonnage}";
                cmd.ExecuteNonQuery();
            });
        }
    }
}