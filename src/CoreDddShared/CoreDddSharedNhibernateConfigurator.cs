using System.Reflection;
using CoreDdd.Nhibernate.Configurations;
using CoreDddShared.Domain;
using HibernatingRhinos.Profiler.Appender.NHibernate;
#if DEBUG

#endif

namespace CoreDddShared
{
    public class CoreDddSharedNhibernateConfigurator : NhibernateConfigurator
    {
#if DEBUG
        public CoreDddSharedNhibernateConfigurator()
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