using System.Collections.Generic;
using System.Linq;
using CoreDdd.Nhibernate.TestHelpers;
using LegacyWebFormsApp.Domain;
using LegacyWebFormsApp.Dtos;
using LegacyWebFormsApp.Queries;
using NUnit.Framework;
using Shouldly;

namespace LegacyWebFormsApp.PersistenceTests.Dtos
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
            _p = new PersistenceTestHelper(new LegacyWebFormsAppNhibernateConfigurator());
            _p.BeginTransaction();

            _newShip = new Ship("ship name", tonnage: 23.4m);
            _p.Save(_newShip);
            _p.Clear();

            var queryHandler = new GetAllShipsQueryQueryHandler(_p.UnitOfWork);

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
            shipDto.Name.ShouldBe("ship name");
            shipDto.Tonnage.ShouldBe(23.4m);
        }

        [TearDown]
        public void TearDown()
        {
            _p.Rollback();
        }
    }
}