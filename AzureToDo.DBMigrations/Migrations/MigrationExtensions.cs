using FluentMigrator.Builders.Create.Table;
using FluentMigrator;
using FluentMigrator.Infrastructure;
using Npgsql;

namespace AzureToDo.DBMigrations.Migrations
{
    public static class MigrationExtensions
    {
        public static IFluentSyntax CreateTableIfNotExists(this MigrationBase self, string tableName, Func<ICreateTableWithColumnOrSchemaOrDescriptionSyntax, IFluentSyntax> constructTableFunction, string schemaName = "dbo")
        {
            if (!self.Schema.Schema(schemaName).Table(tableName).Exists())
            {
                return constructTableFunction(self.Create.Table(tableName));
            }
            else
            {
                return null;
            }
        }

        public static void EnsureDataBaseExists(string ticketConnectionString, string dbname)
        {            
            if (!DbExists(ticketConnectionString, dbname))
            {                
                var adjustedConnectionString = BuildCreateDbConnectionString(ticketConnectionString);
                using var conn = new NpgsqlConnection(adjustedConnectionString);
                    
                var sql = $"CREATE DATABASE {dbname};";
                using var createCommand = new NpgsqlCommand(sql, conn);
                conn.Open();
                createCommand.ExecuteNonQuery();
                conn.Close();
            }            
        }

        private static string BuildCreateDbConnectionString(string ticketConnectionString)
        {
            var connStringParts = ticketConnectionString.Split(';')
                .Select(t => t.Split(new char[] { '=' }, 2))
                .ToDictionary(t => t[0].Trim(), t => t[1].Trim(), StringComparer.InvariantCultureIgnoreCase);
            if (connStringParts.ContainsKey("Database"))
                connStringParts.Remove("Database");
            var adjustedConnectionString = string.Join(";", connStringParts.Select(x => x.Key + "=" + x.Value).ToArray());
            return adjustedConnectionString;
        }

        private static bool DbExists(string connectionStr, string dbname)
        {
            using var conn = new NpgsqlConnection(connectionStr);
            var sql = $"SELECT DATNAME FROM pg_catalog.pg_database WHERE DATNAME = '{dbname}'";
            using var command = new NpgsqlCommand(sql, conn);
            try
            {
                conn.Open();
                var i = command.ExecuteScalar();
                conn.Close();
                if (i != null && i.ToString().Equals(dbname)) //always 'true' (if it exists) or 'null' (if it doesn't)
                    return true;
                else return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}
