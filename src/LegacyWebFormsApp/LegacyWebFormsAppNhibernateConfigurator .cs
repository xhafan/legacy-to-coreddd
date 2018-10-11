using System.Reflection;
using CoreDdd.Nhibernate.Configurations;
using LegacyWebFormsApp.Domain;

namespace LegacyWebFormsApp
{
    public class LegacyWebFormsAppNhibernateConfigurator : NhibernateConfigurator
    {
        protected override Assembly[] GetAssembliesToMap()
        {
            return new[] { typeof(Ship).Assembly };
        }
    }
}