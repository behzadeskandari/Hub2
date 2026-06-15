using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yarane.Persistance.Migrations.ReportDb
{
    /// <inheritdoc />
    public partial class mig006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AMOUNT",
                schema: "APIGW",
                table: "CARDHOLDER_TRANSACTION",
                type: "NUMBER(19,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "NUMBER(19,4)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AMOUNT",
                schema: "APIGW",
                table: "CARDHOLDER_TRANSACTION",
                type: "NUMBER(19,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "NUMBER(19,4)",
                oldNullable: true);
        }
    }
}
