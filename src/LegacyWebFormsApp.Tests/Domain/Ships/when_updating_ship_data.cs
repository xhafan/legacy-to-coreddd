using LegacyWebFormsApp.Domain;
using NUnit.Framework;
using Shouldly;

namespace LegacyWebFormsApp.Tests.Domain.Ships
{
    [TestFixture]
    public class when_updating_ship_data
    {
        private Ship _ship;

        [SetUp]
        public void Context()
        {
            _ship = new Ship("ship name", tonnage: 23.4m);

            _ship.UpdateData("new ship name", tonnage: 34.5m);
        }

        [Test]
        public void ship_data_are_updated()
        {
            _ship.Name.ShouldBe("new ship name");
            _ship.Tonnage.ShouldBe(34.5m);
        }
    }
}