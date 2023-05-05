using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meteor.Controller.Migrations
{
    /// <inheritdoc />
    public partial class FullTextSearchCredentials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "full_text_search_api_key",
                table: "customer_settings",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "full_text_search_url",
                table: "customer_settings",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "full_text_search_api_key",
                table: "customer_settings");

            migrationBuilder.DropColumn(
                name: "full_text_search_url",
                table: "customer_settings");
        }
    }
}
