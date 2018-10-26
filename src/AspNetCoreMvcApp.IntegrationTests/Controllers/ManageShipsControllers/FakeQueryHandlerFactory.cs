using System;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.Queries;
using CoreDddShared.Queries;

namespace AspNetCoreMvcApp.IntegrationTests.Controllers.ManageShipsControllers
{
    public class FakeQueryHandlerFactory : IQueryHandlerFactory
    {
        private readonly NhibernateUnitOfWork _unitOfWork;

        public FakeQueryHandlerFactory(NhibernateUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryHandler<TQuery> Create<TQuery>() where TQuery : IQuery
        {
            if (typeof(TQuery) == typeof(GetAllShipsQuery))
            {
                return (IQueryHandler<TQuery>)new GetAllShipsQueryHandler(_unitOfWork);
            }
            throw new Exception("Unsupported query");
        }

        public void Release<TQuery>(IQueryHandler<TQuery> queryHandler) where TQuery : IQuery
        {
        }
    }
}