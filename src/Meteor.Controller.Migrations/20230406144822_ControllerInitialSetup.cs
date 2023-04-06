#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Meteor.Controller.Migrations;

public partial class ControllerInitialSetup : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "customers",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                domain = table.Column<string>(type: "text", nullable: false),
                created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                status = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk__customers", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "contact_persons",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                customer_id = table.Column<int>(type: "integer", nullable: false),
                full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                email_address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_contact_persons", x => x.id);
                table.ForeignKey(
                    name: "fk__customers_contact_persons",
                    column: x => x.customer_id,
                    principalTable: "customers",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "customer_settings",
            columns: table => new
            {
                customer_id = table.Column<int>(type: "integer", nullable: false),
                core_database_connection_string = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk__customer_settings", x => x.customer_id);
                table.ForeignKey(
                    name: "fk__customers_customer_settings",
                    column: x => x.customer_id,
                    principalTable: "customers",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("contact_persons");

        migrationBuilder.DropTable("customer_settings");

        migrationBuilder.DropTable("customers");
    }
}