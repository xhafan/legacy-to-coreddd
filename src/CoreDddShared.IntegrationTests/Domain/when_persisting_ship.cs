using CoreDdd.Nhibernate.TestHelpers;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddShared.Domain;
using NUnit.Framework;
using Shouldly;
using TestsShared;

namespace CoreDddShared.IntegrationTests.Domain
{
    [TestFixture]
    public class when_persisting_ship
    {
        private NhibernateUnitOfWork _unitOfWork;
        private Ship _newShip;
        private Ship _persistedShip;

        [SetUp]
        public void Context()
        {
            _unitOfWork = new NhibernateUnitOfWork(new CoreDddSharedNhibernateConfigurator());
            _unitOfWork.BeginTransaction();

            _newShip = new ShipBuilder().Build();

            _unitOfWork.Save(_newShip); // save entity into DB -> send INSERT SQL statement into DB
            _unitOfWork.Clear(); // clear NHibernate session so the following SQL SELECT would not load the cached entity version from the session, but would query the database

            _persistedShip = _unitOfWork.Get<Ship>(_newShip.Id);
        }

        [Test]
        public void ship_can_be_retrieved()
        {
            _persistedShip.ShouldNotBeNull();
        }

        [Test]
        public void persisted_ship_id_matches_the_saved_ship_id()
        {
            _persistedShip.ShouldBe(_newShip);
        }

        [Test]
        public void ship_data_are_persisted_correctly()
        {
            _persistedShip.Name.ShouldBe(ShipBuilder.ShipName);
            _persistedShip.Tonnage.ShouldBe(ShipBuilder.Tonnage);
            _persistedShip.ImoNumber.ShouldBe(ShipBuilder.ImoNumber);
            _persistedShip.HasImoNumberBeenVerified.ShouldBe(false);
            _persistedShip.IsImoNumberValid.ShouldBe(false);
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork.Rollback();
        }
    }
}