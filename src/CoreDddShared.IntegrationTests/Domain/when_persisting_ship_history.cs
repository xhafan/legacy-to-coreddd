using System;
using System.Linq;
using CoreDdd.Nhibernate.TestHelpers;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddShared.Domain;
using NUnit.Framework;
using Shouldly;
using TestsShared;

namespace CoreDddShared.IntegrationTests.Domain
{
    [TestFixture]
    public class when_persisting_ship_history
    {
        private NhibernateUnitOfWork _unitOfWork;
        private Ship _newShip;
        private Ship _persistedShip;
        private ShipHistory _persistedShipHistory;

        [SetUp]
        public void Context()
        {
            _unitOfWork = new NhibernateUnitOfWork(new CoreDddSharedNhibernateConfigurator());
            _unitOfWork.BeginTransaction();

            _newShip = new ShipBuilder().Build();

            _unitOfWork.Save(_newShip);

            _unitOfWork.Clear();

            _persistedShip = _unitOfWork.Get<Ship>(_newShip.Id);
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
            _unitOfWork.Rollback();
        }
    }
}