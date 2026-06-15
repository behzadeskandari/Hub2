using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yarane.Persistance.Migrations.ReportDb
{
    /// <inheritdoc />
    public partial class mig00 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "APIGW");

            migrationBuilder.CreateTable(
                name: "CARDHOLDER_TRANSACTION",
                schema: "APIGW",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TRACKING_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SOURCE_PAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DESTINATION_PAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TERMINAL_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACCEPTOR_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AMOUNT = table.Column<decimal>(type: "NUMBER(19,4)", nullable: false),
                    REFERENCE_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TERMINAL_TYPE = table.Column<int>(type: "NUMBER(2)", nullable: true),
                    LOCALIZATION = table.Column<int>(type: "NUMBER(2)", nullable: true),
                    SOURCE_ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REGISTRATION_DATE = table.Column<long>(type: "NUMBER(14)", nullable: true),
                    TRANSACTION_DATE = table.Column<long>(type: "NUMBER(14)", nullable: true),
                    RRN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADDITIONAL_RESPONSE_DATA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CARD_HOLDER_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    APPROVAL_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STATUS = table.Column<int>(type: "NUMBER(5)", nullable: false),
                    TWO_PHASE_REFERENCE_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARDHOLDER_TRANSACTION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CARDHOLDER_TRANSACTION_HISTORY",
                schema: "APIGW",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TRACKING_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SOURCE_PAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DESTINATION_PAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TERMINAL_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACCEPTOR_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AMOUNT = table.Column<decimal>(type: "NUMBER(19,4)", nullable: false),
                    REFERENCE_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TERMINAL_TYPE = table.Column<int>(type: "NUMBER(2)", nullable: true),
                    LOCALIZATION = table.Column<int>(type: "NUMBER(2)", nullable: true),
                    SOURCE_ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REGISTRATION_DATE = table.Column<long>(type: "NUMBER(14)", nullable: true),
                    TRANSACTION_DATE = table.Column<long>(type: "NUMBER(14)", nullable: true),
                    RRN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADDITIONAL_RESPONSE_DATA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CARD_HOLDER_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    APPROVAL_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STATUS = table.Column<int>(type: "NUMBER(5)", nullable: false),
                    TWO_PHASE_REFERENCE_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARDHOLDER_TRANSACTION_HISTORY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CLIENT_CONFIG",
                schema: "APIGW",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CONSUMER_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TERMINAL_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TERMINAL_TYPE = table.Column<int>(type: "int", nullable: false),
                    ACCEPTOR_CODE = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENT_CONFIG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TRANSFERCARD_TRANSACTION",
                schema: "APIGW",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TRACKING_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SOURCE_PAN = table.Column<int>(type: "NVARCHAR2(19)", nullable: true),
                    DESTINATION_PAN = table.Column<int>(type: "NVARCHAR2(19)", nullable: true),
                    TERMINAL_NUMBER = table.Column<long>(type: "NVARCHAR2(10)", nullable: true),
                    AcceptorCode = table.Column<long>(type: "bigint", nullable: true),
                    AMOUNT = table.Column<decimal>(type: "NUMBER(19,4)", nullable: true),
                    REFERENCE_NUMBER = table.Column<long>(type: "NVARCHAR2(30)", nullable: true),
                    TERMINAL_TYPE = table.Column<long>(type: "NUMBER(2)", nullable: true),
                    LOCALIZATION = table.Column<string>(type: "NUMBER(2)", nullable: true),
                    REGISTRATION_DATE = table.Column<long>(type: "NUMBER(14)", nullable: true),
                    TRANSACTION_DATE = table.Column<long>(type: "NUMBER(14)", nullable: true),
                    STAN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    RRN = table.Column<string>(type: "NVARCHAR2(12)", nullable: true),
                    ADDITIONAL_RESPONSE_DATA = table.Column<string>(type: "NVARCHAR2(100)", nullable: true),
                    APPROVAL_CODE = table.Column<string>(type: "NVARCHAR2(64)", nullable: true),
                    STATUS = table.Column<int>(type: "NUMBER(5)", nullable: true),
                    ERRORS_JSON = table.Column<string>(type: "CLOB", nullable: true),
                    SOURCE_ADDRESS = table.Column<string>(type: "NVARCHAR2(15)", nullable: true),
                    TRANSACTION_TYPE = table.Column<string>(type: "INTEGER", nullable: true),
                    MERCHANT_ID = table.Column<string>(type: "NVARCHAR2(15)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSFERCARD_TRANSACTION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TRANSFERCARD_TRANSACTION_HISTORY",
                schema: "APIGW",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TRACKING_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SOURCE_PAN = table.Column<int>(type: "NVARCHAR2(19)", nullable: true),
                    DESTINATION_PAN = table.Column<int>(type: "NVARCHAR2(19)", nullable: true),
                    TERMINAL_NUMBER = table.Column<long>(type: "NVARCHAR2(10)", nullable: true),
                    AcceptorCode = table.Column<long>(type: "bigint", nullable: true),
                    AMOUNT = table.Column<decimal>(type: "NUMBER(19,4)", nullable: true),
                    REFERENCE_NUMBER = table.Column<long>(type: "NVARCHAR2(30)", nullable: true),
                    TERMINAL_TYPE = table.Column<long>(type: "NUMBER(2)", nullable: true),
                    LOCALIZATION = table.Column<string>(type: "NUMBER(2)", nullable: true),
                    REGISTRATION_DATE = table.Column<long>(type: "NUMBER(14)", nullable: true),
                    TRANSACTION_DATE = table.Column<long>(type: "NUMBER(14)", nullable: true),
                    STAN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    RRN = table.Column<string>(type: "NVARCHAR2(12)", nullable: true),
                    ADDITIONAL_RESPONSE_DATA = table.Column<string>(type: "NVARCHAR2(100)", nullable: true),
                    APPROVAL_CODE = table.Column<string>(type: "NVARCHAR2(64)", nullable: true),
                    STATUS = table.Column<int>(type: "NUMBER(5)", nullable: true),
                    ERRORS_JSON = table.Column<string>(type: "CLOB", nullable: true),
                    SOURCE_ADDRESS = table.Column<string>(type: "NVARCHAR2(15)", nullable: true),
                    TRANSACTION_TYPE = table.Column<string>(type: "INTEGER", nullable: true),
                    MERCHANT_ID = table.Column<string>(type: "NVARCHAR2(15)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSFERCARD_TRANSACTION_HISTORY", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CARDHOLDER_TRANSACTION",
                schema: "APIGW");

            migrationBuilder.DropTable(
                name: "CARDHOLDER_TRANSACTION_HISTORY",
                schema: "APIGW");

            migrationBuilder.DropTable(
                name: "CLIENT_CONFIG",
                schema: "APIGW");

            migrationBuilder.DropTable(
                name: "TRANSFERCARD_TRANSACTION",
                schema: "APIGW");

            migrationBuilder.DropTable(
                name: "TRANSFERCARD_TRANSACTION_HISTORY",
                schema: "APIGW");
        }
    }
}
