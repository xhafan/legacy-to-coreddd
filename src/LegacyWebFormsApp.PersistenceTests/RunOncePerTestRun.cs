using System.Data.SqlClient;
using CoreDdd.Nhibernate.DatabaseSchemaGenerators;
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

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    new DatabaseSchemaCreator().CreateDatabaseSchema(nhibernateConfigurator, connection);
                }
            }
        }
    }
}