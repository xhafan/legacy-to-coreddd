using System;
using System.Configuration;
using System.Data.SqlClient;

namespace LegacyWebFormsApp.WebFormsAdoNet
{
    public static class SqlCommandExecutor
    {
        public static void ExecuteSqlCommand(Action<SqlCommand> commandAction)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Legacy"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            using (var cmd = connection.CreateCommand())
            {
                connection.Open();

                commandAction(cmd);
            }
        }
    }
}