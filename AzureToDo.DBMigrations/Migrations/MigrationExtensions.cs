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

        public static void EnsureDataBaseExists(string connectionStr, string dbname)
        {            
            if (!DbExists(connectionStr, dbname))
            {
                var rawConnection = connectionStr.Replace("Database=ticketdb", string.Empty);
                using var conn = new NpgsqlConnection(rawConnection);
                var sql = $"CREATE DATABASE {dbname};";
                using var createCommand = new NpgsqlCommand(sql, conn);
                conn.Open();
                createCommand.ExecuteNonQuery();
                conn.Close();
            }            
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
