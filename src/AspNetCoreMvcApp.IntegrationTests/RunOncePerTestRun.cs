using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading;
using DatabaseBuilder;
using NUnit.Framework;

namespace AspNetCoreMvcApp.IntegrationTests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        private Mutex _mutex;

        [OneTimeSetUp]
        public void SetUp()
        {
            _AcquireSynchronizationMutex();
            _CreateDatabase();
        }

        private void _AcquireSynchronizationMutex()
        {
            var mutexName = "LegacyToCoreDdd.IntegrationTests";
            _mutex = new Mutex(false, mutexName);
            if (!_mutex.WaitOne(TimeSpan.FromSeconds(60)))
            {
                throw new Exception(
                    "Timeout waiting for synchronization mutex to prevent running concurrent tests over the same database");
            }
        }

        private void _CreateDatabase()
        {
            using (var nhibernateConfigurator = new AspNetCoreAppNhibernateConfigurator())
            {
                var configuration = nhibernateConfigurator.GetConfiguration();
                var connectionString = configuration.Properties["connection.connection_string"];
                var scriptsDirectoryPath = Path.Combine(_GetAssemblyCodeBaseLocation(), "DatabaseScripts");

                var builderOfDatabase = new BuilderOfDatabase(() => new SqlConnection(connectionString));
                builderOfDatabase.BuildDatabase(scriptsDirectoryPath);
            }
        }

        // https://stackoverflow.com/a/283917/379279
        private string _GetAssemblyCodeBaseLocation()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _mutex.ReleaseMutex();
            _mutex.Dispose();
        }
    }
}