using CoreDddShared.Domain;

namespace TestsShared
{
    public class ShipBuilder
    {
        public const string ShipName = "SEA STAR";
        public const decimal Tonnage = 23.4m;
        public const string ImoNumber = "IMO 8814275";

        private string _shipName = ShipName;
        private decimal _tonnage = Tonnage;
        private string _imoNumber = ImoNumber;

        public ShipBuilder WithShipName(string shipName)
        {
            _shipName = shipName;
            return this;
        }

        public ShipBuilder WithTonnage(decimal tonnage)
        {
            _tonnage = tonnage;
            return this;
        }

        public ShipBuilder WithImoNumber(string imoNumber)
        {
            _imoNumber = imoNumber;
            return this;
        }

        public Ship Build()
        {
            return new Ship(_shipName, _tonnage, _imoNumber);
        }
    }
}