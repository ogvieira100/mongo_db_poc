using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mongo_api.Migrations
{
    public partial class IdMongo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MongoId",
                table: "Endereco",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MongoId",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MongoId",
                table: "Endereco");

            migrationBuilder.DropColumn(
                name: "MongoId",
                table: "Clientes");
        }
    }
}
