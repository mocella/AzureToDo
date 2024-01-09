using FluentMigrator;

namespace AzureToDo.DBMigrations.Migrations
{
    [Migration(202401090001)]
    public class CreateSupportTicket : Migration
    {
        private const string TableName = "SupportTickets";

        public override void Up()
        {
            this.CreateTableIfNotExists(TableName,
                table =>
                    table
                        .WithColumn("Id").AsInt32().NotNullable().PrimaryKey("PK_SupportTicket_Id").Identity()
                        .WithColumn("Title").AsString()
                        .WithColumn("Description").AsString()
                    );
        }

        public override void Down()
        {
            Delete.Table(TableName);
        }
    }
}
