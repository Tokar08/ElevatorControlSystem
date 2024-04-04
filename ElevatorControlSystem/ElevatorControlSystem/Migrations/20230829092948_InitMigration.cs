using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevatorCS_ef.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "productTitles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__productT__3213E83FEAC0D5A8", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__supplier__3213E83F0337A3B9", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    birthDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    city = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__3213E83FCAE8DCA0", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "depotItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    productTitle_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__depotIte__3213E83F20E8B059", x => x.id);
                    table.ForeignKey(
                        name: "FK_depotItems_productTitles",
                        column: x => x.productTitle_id,
                        principalTable: "productTitles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_depotItems_suppliers",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "inputInvoises",
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

            migrationBuilder.CreateTable(
                name: "priceLists",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__priceLis__3213E83FA5A83BE4", x => x.id);
                    table.ForeignKey(
                        name: "FK_priceLists_users",
                        column: x => x.createdBy,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "registers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    registerNumber = table.Column<int>(type: "int", nullable: false),
                    arrivalDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    productTitle_id = table.Column<int>(type: "int", nullable: false),
                    physicalWeightReg = table.Column<int>(type: "int", nullable: false),
                    shrinkageReg = table.Column<int>(type: "int", nullable: false),
                    wasteReg = table.Column<int>(type: "int", nullable: false),
                    accWeightReg = table.Column<int>(type: "int", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__register__3213E83F04CF8E18", x => x.id);
                    table.ForeignKey(
                        name: "FK_registers_productTitles",
                        column: x => x.productTitle_id,
                        principalTable: "productTitles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_registers_suppliers",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_registers_users",
                        column: x => x.createdBy,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoryTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    categoryValue = table.Column<int>(type: "int", nullable: false),
                    depotItem_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__categori__3213E83F76BD246A", x => x.id);
                    table.ForeignKey(
                        name: "FK_categories_depotItem",
                        column: x => x.depotItem_id,
                        principalTable: "depotItems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "outputInvoices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    outInvNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    shipmentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    vehicleNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    productTitle_id = table.Column<int>(type: "int", nullable: false),
                    depotItem_id = table.Column<int>(type: "int", nullable: false),
                    category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    productWeight = table.Column<int>(type: "int", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__outputIn__3213E83F32DDA9EF", x => x.id);
                    table.ForeignKey(
                        name: "FK_outputInvoices_depotItems",
                        column: x => x.depotItem_id,
                        principalTable: "depotItems",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_outputInvoices_productTitles",
                        column: x => x.productTitle_id,
                        principalTable: "productTitles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_outputInvoices_suppliers",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_outputInvoices_users",
                        column: x => x.createdBy,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "laboratoryCards",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    labCardNumber = table.Column<int>(type: "int", nullable: false),
                    invNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    arrivalDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    productTitle_id = table.Column<int>(type: "int", nullable: false),
                    physicalWeight = table.Column<int>(type: "int", nullable: false),
                    weediness = table.Column<double>(type: "float", nullable: false),
                    moisture = table.Column<double>(type: "float", nullable: false),
                    grainImpurity = table.Column<double>(type: "float", nullable: true),
                    specialNotes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isProduction = table.Column<bool>(type: "bit", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__laborato__3213E83F44550D48", x => x.id);
                    table.ForeignKey(
                        name: "FK_LaboratoryCards_users",
                        column: x => x.createdBy,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_laboratoryCards_inputInvoise",
                        column: x => x.id,
                        principalTable: "inputInvoises",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_laboratoryCards_productTitles",
                        column: x => x.productTitle_id,
                        principalTable: "productTitles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_laboratoryCards_suppliers",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "completionReports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reportNumber = table.Column<int>(type: "int", nullable: false),
                    reportDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    productTitle_id = table.Column<int>(type: "int", nullable: false),
                    priceList_id = table.Column<int>(type: "int", nullable: true),
                    quantityesDrying = table.Column<double>(type: "float", nullable: false),
                    physicalWeightReport = table.Column<double>(type: "float", nullable: false),
                    isFinalized = table.Column<bool>(type: "bit", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__completi__3213E83FC24FF82B", x => x.id);
                    table.ForeignKey(
                        name: "FK_completionReports_priceList",
                        column: x => x.priceList_id,
                        principalTable: "priceLists",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_completionReports_productTitles",
                        column: x => x.productTitle_id,
                        principalTable: "productTitles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_completionReports_suppliers",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_completionReports_users",
                        column: x => x.createdBy,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "priceByOperations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    operationTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    operationPrice = table.Column<double>(type: "float", nullable: false),
                    priceList_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__priceByO__3213E83FE07032BB", x => x.id);
                    table.ForeignKey(
                        name: "FK_operationPrices_priceList",
                        column: x => x.priceList_id,
                        principalTable: "priceLists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productionBatches",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    arrivalDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    labCardNumber = table.Column<int>(type: "int", nullable: false),
                    invNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    productTitle_id = table.Column<int>(type: "int", nullable: false),
                    physicalWeight = table.Column<int>(type: "int", nullable: false),
                    weediness = table.Column<double>(type: "float", nullable: false),
                    moisture = table.Column<double>(type: "float", nullable: false),
                    weedinessBase = table.Column<double>(type: "float", nullable: false),
                    moistureBase = table.Column<double>(type: "float", nullable: false),
                    waste = table.Column<int>(type: "int", nullable: false),
                    shrinkage = table.Column<int>(type: "int", nullable: false),
                    accountWeight = table.Column<int>(type: "int", nullable: false),
                    register_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__producti__3213E83F49BC279D", x => x.id);
                    table.ForeignKey(
                        name: "FK_productionBatches_laboratoryCard",
                        column: x => x.id,
                        principalTable: "laboratoryCards",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productionBatches_productTitles",
                        column: x => x.productTitle_id,
                        principalTable: "productTitles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_productionBatches_register",
                        column: x => x.register_id,
                        principalTable: "registers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_productionBatches_suppliers",
                        column: x => x.supplier_id,
                        principalTable: "suppliers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "technologicalOperations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    amount = table.Column<double>(type: "float", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    totalCost = table.Column<double>(type: "float", nullable: false),
                    completionReport_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__technolo__3213E83FFD310135", x => x.id);
                    table.ForeignKey(
                        name: "FK_technologicalOperations_completionReport",
                        column: x => x.completionReport_id,
                        principalTable: "completionReports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categories_depotItem_id",
                table: "categories",
                column: "depotItem_id");

            migrationBuilder.CreateIndex(
                name: "IX_completionReports_createdBy",
                table: "completionReports",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_completionReports_priceList_id",
                table: "completionReports",
                column: "priceList_id");

            migrationBuilder.CreateIndex(
                name: "IX_completionReports_productTitle_id",
                table: "completionReports",
                column: "productTitle_id");

            migrationBuilder.CreateIndex(
                name: "IX_completionReports_supplier_id",
                table: "completionReports",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_depotItems_productTitle_id",
                table: "depotItems",
                column: "productTitle_id");

            migrationBuilder.CreateIndex(
                name: "IX_depotItems_supplier_id",
                table: "depotItems",
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

            migrationBuilder.CreateIndex(
                name: "IX_laboratoryCards_createdBy",
                table: "laboratoryCards",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_laboratoryCards_productTitle_id",
                table: "laboratoryCards",
                column: "productTitle_id");

            migrationBuilder.CreateIndex(
                name: "IX_laboratoryCards_supplier_id",
                table: "laboratoryCards",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_outputInvoices_createdBy",
                table: "outputInvoices",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_outputInvoices_depotItem_id",
                table: "outputInvoices",
                column: "depotItem_id");

            migrationBuilder.CreateIndex(
                name: "IX_outputInvoices_productTitle_id",
                table: "outputInvoices",
                column: "productTitle_id");

            migrationBuilder.CreateIndex(
                name: "IX_outputInvoices_supplier_id",
                table: "outputInvoices",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_priceByOperations_priceList_id",
                table: "priceByOperations",
                column: "priceList_id");

            migrationBuilder.CreateIndex(
                name: "IX_priceLists_createdBy",
                table: "priceLists",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_productionBatches_productTitle_id",
                table: "productionBatches",
                column: "productTitle_id");

            migrationBuilder.CreateIndex(
                name: "IX_productionBatches_register_id",
                table: "productionBatches",
                column: "register_id");

            migrationBuilder.CreateIndex(
                name: "IX_productionBatches_supplier_id",
                table: "productionBatches",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_registers_createdBy",
                table: "registers",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_registers_productTitle_id",
                table: "registers",
                column: "productTitle_id");

            migrationBuilder.CreateIndex(
                name: "IX_registers_supplier_id",
                table: "registers",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_technologicalOperations_completionReport_id",
                table: "technologicalOperations",
                column: "completionReport_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "outputInvoices");

            migrationBuilder.DropTable(
                name: "priceByOperations");

            migrationBuilder.DropTable(
                name: "productionBatches");

            migrationBuilder.DropTable(
                name: "technologicalOperations");

            migrationBuilder.DropTable(
                name: "depotItems");

            migrationBuilder.DropTable(
                name: "laboratoryCards");

            migrationBuilder.DropTable(
                name: "registers");

            migrationBuilder.DropTable(
                name: "completionReports");

            migrationBuilder.DropTable(
                name: "inputInvoises");

            migrationBuilder.DropTable(
                name: "priceLists");

            migrationBuilder.DropTable(
                name: "productTitles");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
