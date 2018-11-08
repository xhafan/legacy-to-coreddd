using System.Collections.Generic;
using System.Linq;
using CoreDdd.Nhibernate.TestHelpers;
using CoreDddShared.Domain;
using CoreDddShared.Dtos;
using CoreDddShared.Queries;
using NUnit.Framework;
using Shouldly;
using TestsShared;

namespace CoreDddShared.IntegrationTests.Dtos
{
    [TestFixture]
    public class when_fetching_ship_dto
    {
        private PersistenceTestHelper _p;
        private Ship _newShip;
        private IEnumerable<ShipDto> _shipDtos;

        [SetUp]
        public void Context()
        {
            _p = new PersistenceTestHelper(new CoreDddSharedNhibernateConfigurator());
            _p.BeginTransaction();

            _newShip = new ShipBuilder().Build();
            _p.Save(_newShip);
            _p.Clear();

            var queryHandler = new GetAllShipsQueryHandler(_p.UnitOfWork);

            _shipDtos = queryHandler.Execute<ShipDto>(new GetAllShipsQuery());
        }

        [Test]
        public void ship_dto_can_be_fetched()
        {
            _shipDtos.Count().ShouldBe(1);
        }

        [Test]
        public void ship_dto_data_are_fetched_correctly()
        {
            var shipDto = _shipDtos.Single();
            shipDto.Id.ShouldBe(_newShip.Id);
            shipDto.Name.ShouldBe(ShipBuilder.ShipName);
            shipDto.Tonnage.ShouldBe(ShipBuilder.Tonnage);
            shipDto.ImoNumber.ShouldBe(ShipBuilder.ImoNumber);
            shipDto.IsImoNumberVerified.ShouldBe(false);
        }

        [TearDown]
        public void TearDown()
        {
            _p.Rollback();
        }
    }
}