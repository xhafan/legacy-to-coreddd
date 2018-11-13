using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CoreDdd.Commands;
using CoreDdd.Domain.Events;
using CoreDdd.Nhibernate.Configurations;
using CoreDdd.Nhibernate.Register.Castle;
using CoreDdd.Rebus.UnitOfWork;
using CoreDdd.Register.Castle;
using CoreDdd.UnitOfWorks;
using CoreDddShared;
using CoreDddShared.Commands;
using CoreDddShared.Domain;
using CoreDddShared.Domain.Events;
using Rebus.CastleWindsor;
using Rebus.Config;
using ServiceApp.MessageHandlers;

namespace ServiceApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var windsorContainer = new WindsorContainer())
            {
                CoreDddNhibernateInstaller.SetUnitOfWorkLifeStyle(x => x.PerRebusMessage());

                windsorContainer.Install(
                    FromAssembly.Containing<CoreDddInstaller>(),
                    FromAssembly.Containing<CoreDddNhibernateInstaller>()
                );

                windsorContainer.Register(
                    Component.For<INhibernateConfigurator>()
                        .ImplementedBy<CoreDddSharedNhibernateConfigurator>()
                        .LifeStyle.Singleton
                );

                windsorContainer.Register(
                    Component.For<IInternationalMaritimeOrganizationVerifier>()
                        .ImplementedBy<InternationalMaritimeOrganizationVerifier>()
                        .LifeStyle.Transient
                );

                // register command handlers
                windsorContainer.Register(
                    Classes
                        .FromAssemblyContaining<CreateNewShipCommandHandler>()
                        .BasedOn(typeof(ICommandHandler<>))
                        .WithService.FirstInterface()
                        .Configure(x => x.LifestyleTransient()));

                // register domain event handlers
                windsorContainer.Register(
                    Classes
                        .FromAssemblyContaining<ShipCreatedDomainEventHandler>()
                        .BasedOn(typeof(IDomainEventHandler<>))
                        .WithService.FirstInterface()
                        .Configure(x => x.LifestyleTransient()));

                windsorContainer.AutoRegisterHandlersFromAssemblyOf<VerifyImoNumberShipCreatedDomainEventMessageHandler>();

                DomainEvents.Initialize(windsorContainer.Resolve<IDomainEventHandlerFactory>());

                RebusUnitOfWork.Initialize(
                    unitOfWorkFactory: windsorContainer.Resolve<IUnitOfWorkFactory>(),
                    isolationLevel: System.Data.IsolationLevel.ReadCommitted
                );

                var rebusConfigurer = Configure.With(new CastleWindsorContainerAdapter(windsorContainer))
                    .Transport(x => x.UseRabbitMq("amqp://admin:password01@localhost", "ServiceApp"))
                    .Options(o =>
                    {
                        o.EnableUnitOfWork(
                            RebusUnitOfWork.Create,
                            RebusUnitOfWork.Commit,
                            RebusUnitOfWork.Rollback,
                            RebusUnitOfWork.Cleanup
                        );
                    });
                using (var bus = rebusConfigurer.Start())
                {
                    bus.Subscribe<ShipCreatedDomainEventMessage>().Wait();

                    Console.WriteLine("Press enter to quit");
                    Console.ReadLine();
                }
            }
        }
    }
}
