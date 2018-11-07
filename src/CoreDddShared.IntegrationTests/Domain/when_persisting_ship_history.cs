using System;
using System.Linq;
using CoreDdd.Nhibernate.TestHelpers;
using CoreDddShared.Domain;
using NUnit.Framework;
using Shouldly;
using TestsShared;

namespace CoreDddShared.IntegrationTests.Domain
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
            _p = new PersistenceTestHelper(new CoreDddSharedNhibernateConfigurator());
            _p.BeginTransaction();

            _newShip = new ShipBuilder().Build();

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
            _persistedShipHistory.Name.ShouldBe(ShipBuilder.ShipName);
            _persistedShipHistory.Tonnage.ShouldBe(ShipBuilder.Tonnage);
            _persistedShipHistory.CreatedOn.ShouldBeInRange(DateTime.Now.AddSeconds(-10), DateTime.Now.AddSeconds(+10));
        }

        [TearDown]
        public void TearDown()
        {
            _p.Rollback();
        }
    }
}