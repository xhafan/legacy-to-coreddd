using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using DatabaseBuilder;
using NUnit.Framework;

namespace LegacyWebFormsApp.PersistenceTests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            _CreateDatabase();
        }

        private void _CreateDatabase()
        {
            using (var nhibernateConfigurator = new LegacyWebFormsAppNhibernateConfigurator())
            {
                var configuration = nhibernateConfigurator.GetConfiguration();
                var connectionString = configuration.Properties["connection.connection_string"];
                var scriptsDirectoryPath = Path.Combine(_GetAssemblyLocation(), "DatabaseScripts");

                var builderOfDatabase = new BuilderOfDatabase(() => new SqlConnection(connectionString));
                builderOfDatabase.BuildDatabase(scriptsDirectoryPath);
            }
        }

        // https://stackoverflow.com/a/283917/379279
        private string _GetAssemblyLocation()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}