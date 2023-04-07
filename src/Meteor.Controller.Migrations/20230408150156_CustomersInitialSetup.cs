using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Meteor.Controller.Migrations;

/// <inheritdoc />
public partial class CustomersInitialSetup : Migration
{
    /// <inheritdoc />
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
                table.PrimaryKey("pk__contact_persons", x => x.id);
                table.ForeignKey(
                    name: "fk__customers__contact_persons",
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
                core_database_connection_string = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                encrypted = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk__customer_settings", x => x.customer_id);
                table.ForeignKey(
                    name: "fk__customers__customer_settings",
                    column: x => x.customer_id,
                    principalTable: "customers",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "uix__contact_persons__customer_id__email_address",
            table: "contact_persons",
            columns: new[] { "customer_id", "email_address" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "contact_persons");

        migrationBuilder.DropTable(
            name: "customer_settings");

        migrationBuilder.DropTable(
            name: "customers");
    }
}