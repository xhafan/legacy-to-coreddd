using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using AspNetCoreMvcApp.BusRequestSenders;
using CoreDdd.AspNetCore.Middlewares;
using CoreDdd.Commands;
using CoreDdd.Domain.Events;
using CoreDdd.Nhibernate.Configurations;
using CoreDdd.Nhibernate.Register.DependencyInjection;
using CoreDdd.Queries;
using CoreDdd.Register.DependencyInjection;
using CoreDddShared;
using CoreDddShared.Commands;
using CoreDddShared.Domain;
using DatabaseBuilder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;

namespace AspNetCoreMvcApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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
                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            );

            var rebusInputQueueName = "AspNetCoreMvcApp";
            var rebusRabbitMqConnectionString = "amqp://admin:password01@localhost";

            services.AddRebus(configure => configure
                .Logging(l => l.Trace())
                .Transport(t => t.UseRabbitMq(rebusRabbitMqConnectionString, rebusInputQueueName))
                .Options(o => o.EnableSynchronousRequestReply())
                .Routing(x => x.TypeBased().MapAssemblyOf<CreateNewShipCommand>("ServiceApp"))
            );

            services.AddSingleton<IBusRequestSender, BusRequestSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRebus();

            app.UseMiddleware<TransactionScopeUnitOfWorkDependencyInjectionMiddleware>(System.Transactions.IsolationLevel.ReadCommitted);

            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            DomainEvents.Initialize(app.ApplicationServices.GetService<IDomainEventHandlerFactory>());

            _BuildDatabase(app);
        }

        private void _BuildDatabase(IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;
            var nhibernateConfigurator = serviceProvider.GetService<INhibernateConfigurator>();
            var configuration = nhibernateConfigurator.GetConfiguration();
            var connectionString = configuration.Properties["connection.connection_string"];
            var scriptsDirectoryPath = Path.Combine(_GetAssemblyCodeBaseLocation(), "DatabaseScripts");
            var builderOfDatabase = new BuilderOfDatabase(() => new SqlConnection(connectionString));
            builderOfDatabase.BuildDatabase(scriptsDirectoryPath);
        }

        // https://stackoverflow.com/a/283917/379279
        private string _GetAssemblyCodeBaseLocation()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
