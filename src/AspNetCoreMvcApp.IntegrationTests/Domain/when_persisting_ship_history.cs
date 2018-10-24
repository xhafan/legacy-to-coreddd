using System;
using System.Linq;
using AspNetCoreMvcApp.Domain;
using CoreDdd.Nhibernate.TestHelpers;
using NUnit.Framework;
using Shouldly;

namespace AspNetCoreMvcApp.IntegrationTests.Domain
{
    [TestFixture]
    public class when_persisting_ship_history
    {
        private PersistenceTestHelper _p;
        private Ship _newShip;
        private Ship _persistedShip;
        private ShipHistory _persistedShipHistory;

        [SetUp]
        public void Context()
        {
            _p = new PersistenceTestHelper(new AspNetCoreAppNhibernateConfigurator());
            _p.BeginTransaction();

            _newShip = new Ship("ship name", tonnage: 23.4m);

            _p.Save(_newShip);

            _p.Clear();

            _persistedShip = _p.Get<Ship>(_newShip.Id);
            _persistedShipHistory = _persistedShip.ShipHistories.SingleOrDefault();
        }

        [Test]
        public void ship_history_can_be_retrieved()
        {
            _persistedShipHistory.ShouldNotBeNull();
        }

        [Test]
        public void persisted_ship_history_id_matches_the_saved_ship_history_id()
        {
            _persistedShipHistory.ShouldBe(_newShip.ShipHistories.Single());
        }

        [Test]
        public void ship_history_data_are_persisted_correctly()
        {
            _persistedShipHistory.Ship.ShouldBe(_persistedShip);
            _persistedShipHistory.Name.ShouldBe("ship name");
            _persistedShipHistory.Tonnage.ShouldBe(23.4m);
            _persistedShipHistory.CreatedOn.ShouldBeInRange(DateTime.Now.AddSeconds(-10), DateTime.Now.AddSeconds(+10));
        }

        [TearDown]
        public void TearDown()
        {
            _p.Rollback();
        }
    }
}