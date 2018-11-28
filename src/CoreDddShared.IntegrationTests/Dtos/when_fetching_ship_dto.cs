using System.Collections.Generic;
using System.Linq;
using CoreDdd.Nhibernate.TestHelpers;
using CoreDdd.Nhibernate.UnitOfWorks;
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
        private NhibernateUnitOfWork _unitOfWork;
        private Ship _newShip;
        private IEnumerable<ShipDto> _shipDtos;

        [SetUp]
        public void Context()
        {
            _unitOfWork = new NhibernateUnitOfWork(new CoreDddSharedNhibernateConfigurator());
            _unitOfWork.BeginTransaction();

            _newShip = new ShipBuilder().Build();
            _unitOfWork.Save(_newShip);
            _unitOfWork.Clear();

            var queryHandler = new GetAllShipsQueryHandler(_unitOfWork);

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
            shipDto.HasImoNumberBeenVerified.ShouldBe(false);
            shipDto.IsImoNumberValid.ShouldBe(false);
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork.Rollback();
        }
    }
}