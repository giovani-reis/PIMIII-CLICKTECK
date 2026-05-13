using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMIII_CLICKTECK.Migrations
{
    /// <inheritdoc />
    public partial class AddCampoAparelho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aparelho",
                table: "Atendimentos", // Verifique se o nome da sua tabela é este mesmo
                type: "longtext",      // Para MySQL
                nullable: false)
        .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aparelho",
                table: "Atendimentos");
        }
    }
}
