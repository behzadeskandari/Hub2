using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yarane.Persistance.Migrations.ReportDb
{
    /// <inheritdoc />
    public partial class mig005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TRACKING_NUMBER",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION_HISTORY",
                type: "NVARCHAR2(64)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TRACKING_NUMBER",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION",
                type: "NVARCHAR2(64)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "STATUS",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION",
                type: "NUMBER(2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(5)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AMOUNT",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION",
                type: "NUMBER(19)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "NUMBER(19,4)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TRACKING_NUMBER",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION_HISTORY",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(64)");

            migrationBuilder.AlterColumn<string>(
                name: "TRACKING_NUMBER",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(64)");

            migrationBuilder.AlterColumn<int>(
                name: "STATUS",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION",
                type: "NUMBER(5)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AMOUNT",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION",
                type: "NUMBER(19,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "NUMBER(19)",
                oldNullable: true);
        }
    }
}
