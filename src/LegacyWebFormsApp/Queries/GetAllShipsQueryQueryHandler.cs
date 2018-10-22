using CoreDdd.Nhibernate.Queries;
using CoreDdd.Nhibernate.UnitOfWorks;
using LegacyWebFormsApp.Dtos;
using NHibernate;

namespace LegacyWebFormsApp.Queries
{
    public class GetAllShipsQueryQueryHandler : BaseQueryOverHandler<GetAllShipsQuery>
    {
        public GetAllShipsQueryQueryHandler(NhibernateUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        protected override IQueryOver GetQueryOver<TResult>(GetAllShipsQuery query)
        {
            return Session.QueryOver<ShipDto>();
        }
    }
}