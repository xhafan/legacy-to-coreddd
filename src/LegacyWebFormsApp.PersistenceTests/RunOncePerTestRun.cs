using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using CoreDdd.Nhibernate.DatabaseSchemaGenerators;
using Npgsql;
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
                var connectionDriverClass = configuration.Properties["connection.driver_class"];
                var connectionString = configuration.Properties["connection.connection_string"];

                using (var connection = _CreateDbConnection(connectionDriverClass, connectionString))
                {
                    connection.Open();
                    new DatabaseSchemaCreator().CreateDatabaseSchema(nhibernateConfigurator, connection);
                }
            }
        }

        private DbConnection _CreateDbConnection(string connectionDriverClass, string connectionString)
        {
            switch (connectionDriverClass)
            {
                case string x when x.Contains("SQLite"):
                    return new SQLiteConnection(connectionString);
                case string x when x.Contains("SqlClient"):
                    return new SqlConnection(connectionString);
                case string x when x.Contains("NpgsqlDriver"):
                    return new NpgsqlConnection(connectionString);
                default:
                    throw new Exception("Unsupported NHibernate connection.driver_class");
            }
        }
    }
}