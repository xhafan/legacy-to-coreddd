using System;
using System.Linq;
using CoreDddShared.Domain;
using NUnit.Framework;
using Shouldly;

namespace CoreDddShared.Tests.Domain.Ships
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

        [Test]
        public void new_ship_history_record_is_created()
        {
            _ship.ShipHistories.Count().ShouldBe(2);
        }

        [Test]
        public void latest_ship_history_record_data_are_populated()
        {
            var shipHistory = _ship.ShipHistories.Last();
            shipHistory.Name.ShouldBe("new ship name");
            shipHistory.Tonnage.ShouldBe(34.5m);
            shipHistory.CreatedOn.ShouldBeInRange(DateTime.Now.AddSeconds(-10), DateTime.Now.AddSeconds(+10));
        }
    }
}