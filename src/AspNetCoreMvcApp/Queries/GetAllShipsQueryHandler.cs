using AspNetCoreMvcApp.Dtos;
using CoreDdd.Nhibernate.Queries;
using CoreDdd.Nhibernate.UnitOfWorks;
using NHibernate;

namespace AspNetCoreMvcApp.Queries
{
    public class GetAllShipsQueryHandler : BaseQueryOverHandler<GetAllShipsQuery>
    {
        public GetAllShipsQueryHandler(NhibernateUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        protected override IQueryOver GetQueryOver<TResult>(GetAllShipsQuery query)
        {
            return Session.QueryOver<ShipDto>();
        }
    }
}