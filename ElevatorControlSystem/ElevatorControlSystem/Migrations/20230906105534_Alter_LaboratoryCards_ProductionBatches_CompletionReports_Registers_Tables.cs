using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevatorCS_ef.Migrations
{
    /// <inheritdoc />
    public partial class Alter_LaboratoryCards_ProductionBatches_CompletionReports_Registers_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_laboratoryCards_inputInvoise",
                table: "laboratoryCards");

            migrationBuilder.DropForeignKey(
                name: "FK_laboratoryCards_productTitles",
                table: "laboratoryCards");

            migrationBuilder.DropForeignKey(
                name: "FK_laboratoryCards_suppliers",
                table: "laboratoryCards");

            migrationBuilder.DropForeignKey(
                name: "FK_productionBatches_productTitles",
                table: "productionBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_productionBatches_suppliers",
                table: "productionBatches");

            migrationBuilder.DropTable(
                name: "inputInvoises");

            migrationBuilder.DropIndex(
                name: "IX_productionBatches_productTitle_id",
                table: "productionBatches");

            migrationBuilder.DropIndex(
                name: "IX_productionBatches_supplier_id",
                table: "productionBatches");

            migrationBuilder.DropIndex(
                name: "IX_laboratoryCards_productTitle_id",
                table: "laboratoryCards");

            migrationBuilder.DropIndex(
                name: "IX_laboratoryCards_supplier_id",
                table: "laboratoryCards");

            migrationBuilder.DropColumn(
                name: "arrivalDate",
                table: "productionBatches");

            migrationBuilder.DropColumn(
                name: "invNumber",
                table: "productionBatches");

            migrationBuilder.DropColumn(
                name: "labCardNumber",
                table: "productionBatches");

            migrationBuilder.DropColumn(
                name: "moisture",
                table: "productionBatches");

            migrationBuilder.DropColumn(
                name: "physicalWeight",
                table: "productionBatches");

            migrationBuilder.DropColumn(
                name: "productTitle_id",
                table: "productionBatches");

            migrationBuilder.DropColumn(
                name: "supplier_id",
                table: "productionBatches");

            migrationBuilder.DropColumn(
                name: "weediness",
                table: "productionBatches");

            migrationBuilder.DropColumn(
                name: "arrivalDate",
                table: "laboratoryCards");

            migrationBuilder.DropColumn(
                name: "invNumber",
                table: "laboratoryCards");

            migrationBuilder.DropColumn(
                name: "physicalWeight",
                table: "laboratoryCards");

            migrationBuilder.DropColumn(
                name: "productTitle_id",
                table: "laboratoryCards");

            migrationBuilder.DropColumn(
                name: "supplier_id",
                table: "laboratoryCards");

            migrationBuilder.AddColumn<int>(
                name: "CompletionReportId",
                table: "registers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "inputInvoices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    invNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    arrivalDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    vehicleNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    productTitle_id = table.Column<int>(type: "int", nullable: false),
                    physicalWeight = table.Column<int>(type: "int", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__inputInv__3213E83F55A1A217", x => x.id);
                    table.ForeignKey(
                        name: "FK_inputInvoices_productTitles",
                        column: x => x.productTitle_id,
                        principalTable: "productTitles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_inputInvoices_suppliers",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_inputInvoices_users",
                        column: x => x.createdBy,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_registers_CompletionReportId",
                table: "registers",
                column: "CompletionReportId");

            migrationBuilder.CreateIndex(
                name: "IX_inputInvoices_createdBy",
                table: "inputInvoices",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_inputInvoices_productTitle_id",
                table: "inputInvoices",
                column: "productTitle_id");

            migrationBuilder.CreateIndex(
                name: "IX_inputInvoices_supplier_id",
                table: "inputInvoices",
                column: "supplier_id");

            migrationBuilder.AddForeignKey(
                name: "FK_laboratoryCards_inputInvoice",
                table: "laboratoryCards",
                column: "id",
                principalTable: "inputInvoices",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_registers_completionReports",
                table: "registers",
                column: "CompletionReportId",
                principalTable: "completionReports",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_laboratoryCards_inputInvoice",
                table: "laboratoryCards");

            migrationBuilder.DropForeignKey(
                name: "FK_registers_completionReports",
                table: "registers");

            migrationBuilder.DropTable(
                name: "inputInvoices");

            migrationBuilder.DropIndex(
                name: "IX_registers_CompletionReportId",
                table: "registers");

            migrationBuilder.DropColumn(
                name: "CompletionReportId",
                table: "registers");

            migrationBuilder.AddColumn<DateTime>(
                name: "arrivalDate",
                table: "productionBatches",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "invNumber",
                table: "productionBatches",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "labCardNumber",
                table: "productionBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "moisture",
                table: "productionBatches",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "physicalWeight",
                table: "productionBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "productTitle_id",
                table: "productionBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "supplier_id",
                table: "productionBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "weediness",
                table: "productionBatches",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "arrivalDate",
                table: "laboratoryCards",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "invNumber",
                table: "laboratoryCards",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "physicalWeight",
                table: "laboratoryCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "productTitle_id",
                table: "laboratoryCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "supplier_id",
                table: "laboratoryCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "inputInvoises",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createdBy = table.Column<int>(type: "int", nullable: true),
                    productTitle_id = table.Column<int>(type: "int", nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    arrivalDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    invNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    physicalWeight = table.Column<int>(type: "int", nullable: false),
                    vehicleNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__inputInv__3213E83F55A1A217", x => x.id);
                    table.ForeignKey(
                        name: "FK_inputInvoises_productTitles",
                        column: x => x.productTitle_id,
                        principalTable: "productTitles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_inputInvoises_suppliers",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_inputInvoises_users",
                        column: x => x.createdBy,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_productionBatches_productTitle_id",
                table: "productionBatches",
                column: "productTitle_id");

            migrationBuilder.CreateIndex(
                name: "IX_productionBatches_supplier_id",
                table: "productionBatches",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_laboratoryCards_productTitle_id",
                table: "laboratoryCards",
                column: "productTitle_id");

            migrationBuilder.CreateIndex(
                name: "IX_laboratoryCards_supplier_id",
                table: "laboratoryCards",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_inputInvoises_createdBy",
                table: "inputInvoises",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_inputInvoises_productTitle_id",
                table: "inputInvoises",
                column: "productTitle_id");

            migrationBuilder.CreateIndex(
                name: "IX_inputInvoises_supplier_id",
                table: "inputInvoises",
                column: "supplier_id");

            migrationBuilder.AddForeignKey(
                name: "FK_laboratoryCards_inputInvoise",
                table: "laboratoryCards",
                column: "id",
                principalTable: "inputInvoises",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_laboratoryCards_productTitles",
                table: "laboratoryCards",
                column: "productTitle_id",
                principalTable: "productTitles",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_laboratoryCards_suppliers",
                table: "laboratoryCards",
                column: "supplier_id",
                principalTable: "suppliers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_productionBatches_productTitles",
                table: "productionBatches",
                column: "productTitle_id",
                principalTable: "productTitles",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_productionBatches_suppliers",
                table: "productionBatches",
                column: "supplier_id",
                principalTable: "suppliers",
                principalColumn: "id");
        }
    }
}
