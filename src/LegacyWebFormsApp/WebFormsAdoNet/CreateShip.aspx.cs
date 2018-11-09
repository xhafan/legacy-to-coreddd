using System;
using CoreDddShared.Domain;

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
            var imoNumber = ImoNumberTextBox.Text;

            var internationalMaritimeOrganizationVerifier = new InternationalMaritimeOrganizationVerifier();
            var isImoNumberValid = internationalMaritimeOrganizationVerifier.IsImoNumberValid(imoNumber);

            SqlCommandExecutor.ExecuteSqlCommand(cmd =>
            {
                const int hasImoNumberBeenVerified = 1;
                cmd.CommandText = 
                    $"EXEC CreateShip '{shipName}', {tonnage}, '{imoNumber}', {hasImoNumberBeenVerified}, {(isImoNumberValid ? 1 : 0)}";
                var shipId = (int)cmd.ExecuteScalar();
                LastShipIdCreatedLabel.Text = $"{shipId}";
            });
        }
    }
}