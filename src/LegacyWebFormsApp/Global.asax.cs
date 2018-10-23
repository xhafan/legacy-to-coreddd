using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CoreDdd.AspNet.HttpModules;
using CoreDdd.Commands;
using CoreDdd.Nhibernate.Configurations;
using CoreDdd.Nhibernate.Register.Castle;
using CoreDdd.Queries;
using CoreDdd.Register.Castle;
using CoreDdd.UnitOfWorks;
using CoreIoC;
using CoreIoC.Castle;
using DatabaseBuilder;
using LegacyWebFormsApp.Commands;
using LegacyWebFormsApp.Queries;

namespace LegacyWebFormsApp
{
    public class Global : System.Web.HttpApplication
    {
        private WindsorContainer _windsorContainer;

        protected void Application_Start(object sender, EventArgs e)
        {
            _BuildDatabase();
            _RegisterCoreDddServicesIntoIoCContainer();
        }

        private void _BuildDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Legacy"].ConnectionString;
            var scriptsDirectoryPath = Path.Combine(_GetAssemblyCodeBaseLocation(), @"..\..\DatabaseScripts");
            var builderOfDatabase = new BuilderOfDatabase(() => new SqlConnection(connectionString));
            builderOfDatabase.BuildDatabase(scriptsDirectoryPath);
        }

        private void _RegisterCoreDddServicesIntoIoCContainer()
        {
            _windsorContainer = new WindsorContainer();

            CoreDddNhibernateInstaller.SetUnitOfWorkLifeStyle(x => x.PerWebRequest);

            _windsorContainer.Install(
                FromAssembly.Containing<CoreDddInstaller>(),
                FromAssembly.Containing<CoreDddNhibernateInstaller>()
            );
            _windsorContainer.Register(
                Component
                    .For<INhibernateConfigurator>()
                    .ImplementedBy<LegacyWebFormsAppNhibernateConfigurator>()
                    .LifestyleSingleton()
            );
            UnitOfWorkHttpModule.Initialize(_windsorContainer.Resolve<IUnitOfWorkFactory>());

            // register command handlers
            _windsorContainer.Register(
                Classes
                    .FromAssemblyContaining<CreateNewShipCommandHandler>()
                    .BasedOn(typeof(ICommandHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient())
            );

            // register query handlers
            _windsorContainer.Register(
                Classes
                    .FromAssemblyContaining<GetAllShipsQueryHandler>()
                    .BasedOn(typeof(IQueryHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient())
            );

            IoC.Initialize(new CastleContainer(_windsorContainer));
        }

        // https://stackoverflow.com/a/283917/379279
        private string _GetAssemblyCodeBaseLocation()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}