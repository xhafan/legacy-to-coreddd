using System.Reflection;
using CoreDdd.Nhibernate.Configurations;
using LegacyWebFormsApp.Domain;
#if DEBUG
using HibernatingRhinos.Profiler.Appender.NHibernate;
#endif

namespace LegacyWebFormsApp
{
    public class LegacyWebFormsAppNhibernateConfigurator : NhibernateConfigurator
    {
#if DEBUG
        public LegacyWebFormsAppNhibernateConfigurator()
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