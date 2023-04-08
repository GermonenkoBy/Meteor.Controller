using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Meteor.Controller.Migrations;

public partial class InitialCustomersSetup : Migration
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
                table.PrimaryKey("pk_customers", x => x.id);
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
                    name: "fk_contact_persons_customers_customer_id",
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
                table.PrimaryKey("pk_customer_settings", x => x.customer_id);
                table.ForeignKey(
                    name: "fk_customer_settings_customers_customer_id",
                    column: x => x.customer_id,
                    principalTable: "customers",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_contact_persons_customer_id_email_address",
            table: "contact_persons",
            columns: new[] { "customer_id", "email_address" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_customers_domain",
            table: "customers",
            column: "domain",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_customers_name",
            table: "customers",
            column: "name",
            unique: true);
    }

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