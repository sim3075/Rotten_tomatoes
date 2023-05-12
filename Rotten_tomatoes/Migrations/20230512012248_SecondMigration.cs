using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rotten_tomatoes.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "TvShow",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "plataformas",
                table: "TvShow",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "Pelicula",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "plataformas",
                table: "Pelicula",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "TvShow");

            migrationBuilder.DropColumn(
                name: "plataformas",
                table: "TvShow");

            migrationBuilder.DropColumn(
                name: "Img",
                table: "Pelicula");

            migrationBuilder.DropColumn(
                name: "plataformas",
                table: "Pelicula");
        }
    }
}
