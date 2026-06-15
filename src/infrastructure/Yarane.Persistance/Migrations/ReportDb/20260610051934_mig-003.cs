using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yarane.Persistance.Migrations.ReportDb
{
    /// <inheritdoc />
    public partial class mig003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AcceptorCode",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION_HISTORY",
                newName: "ACCEPTOR_CODE");

            migrationBuilder.RenameColumn(
                name: "AcceptorCode",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION",
                newName: "ACCEPTOR_CODE");

            migrationBuilder.AlterColumn<long>(
                name: "ACCEPTOR_CODE",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION_HISTORY",
                type: "NVARCHAR2(15)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ACCEPTOR_CODE",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION",
                type: "NVARCHAR2(15)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ACCEPTOR_CODE",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION_HISTORY",
                newName: "AcceptorCode");

            migrationBuilder.RenameColumn(
                name: "ACCEPTOR_CODE",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION",
                newName: "AcceptorCode");

            migrationBuilder.AlterColumn<long>(
                name: "AcceptorCode",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION_HISTORY",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "NVARCHAR2(15)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AcceptorCode",
                schema: "APIGW",
                table: "TRANSFERCARD_TRANSACTION",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "NVARCHAR2(15)",
                oldNullable: true);
        }
    }
}
