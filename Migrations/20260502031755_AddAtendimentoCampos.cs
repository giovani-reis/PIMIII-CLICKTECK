using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMIII_CLICKTECK.Migrations
{
    /// <inheritdoc />
    public partial class AddAtendimentoCampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ObservacaoTecnico",
                table: "Atendimentos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorOrcamento",
                table: "Atendimentos",
                type: "decimal(65,30)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObservacaoTecnico",
                table: "Atendimentos");

            migrationBuilder.DropColumn(
                name: "ValorOrcamento",
                table: "Atendimentos");
        }
    }
}
