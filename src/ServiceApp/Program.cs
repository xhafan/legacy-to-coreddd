using CoreDdd.Nhibernate.Register.DependencyInjection;
using CoreDdd.Rebus.UnitOfWork;
using CoreDdd.Register.DependencyInjection;
using CoreDdd.UnitOfWorks;
using CoreDddShared;
using CoreDddShared.Domain;
using CoreDddShared.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.ServiceProvider;

namespace ServiceApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddCoreDdd();
            services.AddCoreDddNhibernate<CoreDddSharedNhibernateConfigurator>();

            services.AddTransient<IInternationalMaritimeOrganizationVerifier, InternationalMaritimeOrganizationVerifier>();

            services.AutoRegisterHandlersFromAssemblyOf<VerifyImoNumberShipCreatedDomainEventMessageHandler>();

            services.AddRebus(configure => configure
                .Logging(l => l.ColoredConsole())
                .Transport(t => t.UseRabbitMq("amqp://admin:password01@localhost", "ServiceApp"))
                .Options(o =>
                {
                    o.EnableUnitOfWork(
                        RebusUnitOfWork.Create,
                        RebusUnitOfWork.Commit,
                        RebusUnitOfWork.Rollback,
                        RebusUnitOfWork.Cleanup
                    );
                })

            );

            var provider = services.BuildServiceProvider();

            RebusUnitOfWork.Initialize(provider.GetService<IUnitOfWorkFactory>());

            provider.UseRebus(bus =>
            {
                bus.Subscribe<ShipCreatedDomainEventMessage>().Wait();
            });
        }
    }
}
