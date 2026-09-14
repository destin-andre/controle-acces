using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ControleAcces.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjoutBadge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    IdBadge = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UidBadge = table.Column<string>(type: "text", nullable: false),
                    DateAttribution = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Actif = table.Column<bool>(type: "boolean", nullable: false),
                    IdUtilisateur = table.Column<int>(type: "integer", nullable: false),
                    UtilisateurIdUtilisateur = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.IdBadge);
                    table.ForeignKey(
                        name: "FK_Badges_Utilisateurs_UtilisateurIdUtilisateur",
                        column: x => x.UtilisateurIdUtilisateur,
                        principalTable: "Utilisateurs",
                        principalColumn: "IdUtilisateur");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Badges_UtilisateurIdUtilisateur",
                table: "Badges",
                column: "UtilisateurIdUtilisateur");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Badges");
        }
    }
}
