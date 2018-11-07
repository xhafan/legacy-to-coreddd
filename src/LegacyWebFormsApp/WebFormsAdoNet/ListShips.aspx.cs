using System;
using System.Configuration;
using System.Data.SqlClient;

namespace LegacyWebFormsApp.WebFormsAdoNet
{
    public partial class ListShips : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _LoadShips();
        }

        private void _LoadShips()
        {
            ExistingShipsListBox.Items.Clear();

            var connectionString = ConfigurationManager.ConnectionStrings["Legacy"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            using (var cmd = connection.CreateCommand())
            {
                connection.Open();

                cmd.CommandText = "SELECT ShipId, ShipName, Tonnage, ImoNumber FROM Ship";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var shipId = reader.GetInt32(0);
                        var shipName = reader.GetString(1);
                        var tonnage = reader.GetDecimal(2);
                        var imoNumber = reader.GetString(3);
                        ExistingShipsListBox.Items.Add($"Id: {shipId}, Name: {shipName}, Tonnage: {tonnage}, IMO number: {imoNumber}");
                    }
                }
            }
        }
    }
}