using System;
using System.Linq;
using CoreDddShared.Domain;
using NUnit.Framework;
using Shouldly;
using TestsShared;

namespace CoreDddShared.Tests.Domain.Ships
{
    [TestFixture]
    public class when_creating_new_ship
    {
        private Ship _ship;

        [SetUp]
        public void Context()
        {
            _ship = new ShipBuilder().Build();
        }

        [Test]
        public void ship_data_are_populated()
        {
            _ship.Name.ShouldBe(ShipBuilder.ShipName);
            _ship.Tonnage.ShouldBe(ShipBuilder.Tonnage);
            _ship.ImoNumber.ShouldBe(ShipBuilder.ImoNumber);
        }

        [Test]
        public void ship_history_record_is_created_and_its_data_are_populated()
        {
            var shipHistory = _ship.ShipHistories.SingleOrDefault();
            shipHistory.ShouldNotBeNull();
            shipHistory.Name.ShouldBe(ShipBuilder.ShipName);
            shipHistory.Tonnage.ShouldBe(ShipBuilder.Tonnage);
            shipHistory.CreatedOn.ShouldBeInRange(DateTime.Now.AddSeconds(-10), DateTime.Now.AddSeconds(+10));
        }
    }
}
