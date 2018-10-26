using CoreDdd.Commands;

namespace CoreDddShared.Commands
{
    public class CreateNewShipCommand : ICommand
    {
        public string ShipName { get; set; }
        public decimal Tonnage { get; set; }
    }
}