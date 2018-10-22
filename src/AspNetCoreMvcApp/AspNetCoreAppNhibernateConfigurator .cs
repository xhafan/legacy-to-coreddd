using System.Reflection;
using AspNetCoreMvcApp.Domain;
using CoreDdd.Nhibernate.Configurations;
using HibernatingRhinos.Profiler.Appender.NHibernate;

#if DEBUG

#endif

namespace AspNetCoreMvcApp
{
    public class AspNetCoreAppNhibernateConfigurator : NhibernateConfigurator
    {
#if DEBUG
        public AspNetCoreAppNhibernateConfigurator()
        {
            NHibernateProfiler.Initialize();
        }
#endif

        protected override Assembly[] GetAssembliesToMap()
        {
            return new[] { typeof(Ship).Assembly };
        }

#if DEBUG
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                NHibernateProfiler.Shutdown();
            }
            base.Dispose(disposing);
        }
#endif
    }
}