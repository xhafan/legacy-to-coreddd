using LegacyWebFormsApp.Domain;
using NUnit.Framework;
using Shouldly;

namespace LegacyWebFormsApp.Tests.Domain.Ships
{
    [TestFixture]
    public class when_creating_new_ship
    {
        private Ship _ship;

        [SetUp]
        public void Context()
        {
            _ship = new Ship("ship name", tonnage: 23.4m);
        }

        [Test]
        public void ship_data_are_populated()
        {
            _ship.Name.ShouldBe("ship name");
            _ship.Tonnage.ShouldBe(23.4m);
        }
    }
}
