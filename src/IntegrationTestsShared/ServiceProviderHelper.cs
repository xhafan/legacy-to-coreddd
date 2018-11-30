#if NETSTANDARD
using CoreDdd.Commands;
using CoreDdd.Nhibernate.Register.DependencyInjection;
using CoreDdd.Queries;
using CoreDdd.Register.DependencyInjection;
using CoreDddShared;
using CoreDddShared.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTestsShared
{
    public class ServiceProviderHelper
    {
        public ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddCoreDdd();
            services.AddCoreDddNhibernate<CoreDddSharedNhibernateConfigurator>();

            // register command handlers, query handlers
            services.Scan(scan => scan
                .FromAssemblyOf<Ship>()
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            );
            return services.BuildServiceProvider();
        }
    }
}
#endif