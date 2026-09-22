using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleAcces.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjoutRaisonsLogAcces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Raisons",
                table: "LogAcces",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Raisons",
                table: "LogAcces");
        }
    }
}
