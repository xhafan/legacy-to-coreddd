using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using CoreDdd.Nhibernate.DatabaseSchemaGenerators;
using DatabaseBuilder;
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
                var scriptsDirectoryPath = Path.Combine(_GetAssemblyLocation(), "DatabaseScripts");

                var builderOfDatabase = new BuilderOfDatabase(() => _CreateDbConnection(connectionDriverClass, connectionString));
                builderOfDatabase.BuildDatabase(scriptsDirectoryPath);
            }
        }

        private IDbConnection _CreateDbConnection(string connectionDriverClass, string connectionString)
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