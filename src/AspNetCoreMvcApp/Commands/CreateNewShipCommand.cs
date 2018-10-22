using CoreDdd.Commands;

namespace AspNetCoreMvcApp.Commands
{
    public class CreateNewShipCommand : ICommand
    {
        public string ShipName { get; set; }
        public decimal Tonnage { get; set; }
    }
}