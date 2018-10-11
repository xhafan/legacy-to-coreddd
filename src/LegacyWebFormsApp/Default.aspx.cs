using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LegacyWebFormsApp
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _LoadShips();
        }

        protected void CreateShipButton_Click(object sender, EventArgs e)
        {
            var shipName = CreateShipNameTextBox.Text;
            var tonnage = decimal.Parse(CreateTonnageTextBox.Text);

            _ExecuteSqlCommand(cmd =>
            {
                cmd.CommandText = $"EXEC CreateShip '{shipName}', {tonnage}";
                var shipId = (int)cmd.ExecuteScalar();
            });
            _LoadShips();
        }

        protected void UpdateShipButton_OnClickShipButton_Click(object sender, EventArgs e)
        {
            var shipId = UpdateShipIdTextBox.Text;
            var shipName = UpdateShipNameTextBox.Text;
            var tonnage = decimal.Parse(UpdateTonnageTextBox.Text);

            _ExecuteSqlCommand(cmd =>
            {
                cmd.CommandText = $"EXEC UpdateShip {shipId}, '{shipName}', {tonnage}";
                cmd.ExecuteNonQuery();
            });
            _LoadShips();
        }

        private void _ExecuteSqlCommand(Action<SqlCommand> commandAction)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Legacy"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            using (var cmd = connection.CreateCommand())
            {
                connection.Open();

                commandAction(cmd);
            }
        }

        private void _LoadShips()
        {
            ExistingShipsListBox.Items.Clear();

            var connectionString = ConfigurationManager.ConnectionStrings["Legacy"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            using (var cmd = connection.CreateCommand())
            {
                connection.Open();

                cmd.CommandText = "SELECT ShipId, ShipName, Tonnage FROM Ship";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var shipId = reader.GetInt32(0);
                        var shipName = reader.GetString(1);
                        var tonnage = reader.GetDecimal(2);
                        ExistingShipsListBox.Items.Add($"Id: {shipId}, Name: {shipName}, Tonnage: {tonnage}");
                    }
                }
            }
        }

    }
}