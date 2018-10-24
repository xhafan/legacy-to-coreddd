using System;
using System.Linq;
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

        [Test]
        public void ship_history_record_is_created()
        {
            _ship.ShipHistories.Count().ShouldBe(1);
        }

        [Test]
        public void ship_history_record_data_are_populated()
        {
            var shipHistory = _ship.ShipHistories.Single();
            shipHistory.Ship.ShouldBe(_ship);
            shipHistory.Name.ShouldBe("ship name");
            shipHistory.Tonnage.ShouldBe(23.4m);
            shipHistory.CreatedOn.ShouldBeInRange(DateTime.Now.AddSeconds(-10), DateTime.Now.AddSeconds(+10));
        }
    }
}
