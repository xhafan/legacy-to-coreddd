using System;
using CoreDdd.Queries;
using CoreDddShared.Dtos;
using CoreDddShared.Queries;
using CoreIoC;

namespace LegacyWebFormsApp.WebFormsCoreDdd
{
    public partial class ListShips : System.Web.UI.Page
    {
        private IQueryExecutor _queryExecutor;

        protected void Page_Load(object sender, EventArgs e)
        {
            _queryExecutor = IoC.Resolve<IQueryExecutor>();

            _LoadShips();
        }

        private void _LoadShips()
        {
            ExistingShipsListBox.Items.Clear();

            var getAllShipsQuery = new GetAllShipsQuery();
            var shipDtos = _queryExecutor.Execute<GetAllShipsQuery, ShipDto>(getAllShipsQuery);

            foreach (var shipDto in shipDtos)
            {
                ExistingShipsListBox.Items.Add($"Id: {shipDto.Id}, Name: {shipDto.Name}, Tonnage: {shipDto.Tonnage}, IMO number: {shipDto.ImoNumber}");
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            IoC.Release(_queryExecutor);
        }
    }
}