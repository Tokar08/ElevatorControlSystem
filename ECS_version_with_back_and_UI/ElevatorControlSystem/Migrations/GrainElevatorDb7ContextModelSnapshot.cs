using System;
using ElevatorControlSystem;
using GrainElevatorCS_ef.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GrainElevatorCS_ef.Migrations
{
    [DbContext(typeof(Db))]
    partial class GrainElevatorDb7ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GrainElevatorCS_ef.Models.CompletionReport", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("id");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<int?>("CreatedBy")
                    .HasColumnType("int")
                    .HasColumnName("createdBy");

                b.Property<bool>("IsFinalized")
                    .HasColumnType("bit")
                    .HasColumnName("isFinalized");

                b.Property<double>("PhysicalWeightReport")
                    .HasColumnType("float")
                    .HasColumnName("physicalWeightReport");

                b.Property<int?>("PriceListId")
                    .HasColumnType("int")
                    .HasColumnName("priceList_id");

                b.Property<int>("ProductTitleId")
                    .HasColumnType("int")
                    .HasColumnName("productTitle_id");

                b.Property<double>("QuantityesDrying")
                    .HasColumnType("float")
                    .HasColumnName("quantityesDrying");

                b.Property<DateTime>("ReportDate")
                    .HasColumnType("datetime")
                    .HasColumnName("reportDate");

                b.Property<int>("ReportNumber")
                    .HasColumnType("int")
                    .HasColumnName("reportNumber");

                b.Property<int>("SupplierId")
                    .HasColumnType("int")
                    .HasColumnName("supplier_id");

                b.HasKey("Id")
                    .HasName("PK__completi__3213E83FC24FF82B");

                b.HasIndex("CreatedBy");

                b.HasIndex("PriceListId");

                b.HasIndex("ProductTitleId");

                b.HasIndex("SupplierId");

                b.ToTable("completionReports", (string)null);
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.DepotItem", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("id");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<int>("ProductTitleId")
                    .HasColumnType("int")
                    .HasColumnName("productTitle_id");

                b.Property<int>("SupplierId")
                    .HasColumnType("int")
                    .HasColumnName("supplier_id");

                b.HasKey("Id")
                    .HasName("PK__depotIte__3213E83F20E8B059");

                b.HasIndex("ProductTitleId");

                b.HasIndex("SupplierId");

                b.ToTable("depotItems", (string)null);
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.DepotItemCategory", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("id");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("CategoryTitle")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("categoryTitle");

                b.Property<int>("CategoryValue")
                    .HasColumnType("int")
                    .HasColumnName("categoryValue");

                b.Property<int>("DepotItemId")
                    .HasColumnType("int")
                    .HasColumnName("depotItem_id");

                b.HasKey("Id")
                    .HasName("PK__categori__3213E83F76BD246A");

                b.HasIndex("DepotItemId");

                b.ToTable("depotItemCategories", (string)null);
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.InputInvoice", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("id");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<DateTime>("ArrivalDate")
                    .HasColumnType("datetime")
                    .HasColumnName("arrivalDate");

                b.Property<int?>("CreatedBy")
                    .HasColumnType("int")
                    .HasColumnName("createdBy");

                b.Property<string>("InvNumber")
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnType("nvarchar(8)")
                    .HasColumnName("invNumber");

                b.Property<int>("PhysicalWeight")
                    .HasColumnType("int")
                    .HasColumnName("physicalWeight");

                b.Property<int>("ProductTitleId")
                    .HasColumnType("int")
                    .HasColumnName("productTitle_id");

                b.Property<int>("SupplierId")
                    .HasColumnType("int")
                    .HasColumnName("supplier_id");

                b.Property<string>("VehicleNumber")
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnType("nvarchar(8)")
                    .HasColumnName("vehicleNumber");

                b.HasKey("Id")
                    .HasName("PK__inputInv__3213E83F55A1A217");

                b.HasIndex("CreatedBy");

                b.HasIndex("ProductTitleId");

                b.HasIndex("SupplierId");

                b.ToTable("inputInvoices", (string)null);
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.LaboratoryCard", b =>
            {
                b.Property<int>("Id")
                    .HasColumnType("int")
                    .HasColumnName("id");

                b.Property<int?>("CreatedBy")
                    .HasColumnType("int")
                    .HasColumnName("createdBy");

                b.Property<double?>("GrainImpurity")
                    .HasColumnType("float")
                    .HasColumnName("grainImpurity");

                b.Property<bool>("IsProduction")
                    .HasColumnType("bit")
                    .HasColumnName("isProduction");

                b.Property<int>("LabCardNumber")
                    .HasColumnType("int")
                    .HasColumnName("labCardNumber");

                b.Property<double>("Moisture")
                    .HasColumnType("float")
                    .HasColumnName("moisture");

                b.Property<string>("SpecialNotes")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("specialNotes");

                b.Property<double>("Weediness")
                    .HasColumnType("float")
                    .HasColumnName("weediness");

                b.HasKey("Id")
                    .HasName("PK__laborato__3213E83F44550D48");

                b.HasIndex("CreatedBy");

                b.ToTable("laboratoryCards", (string)null);
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.OutputInvoice", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("id");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Category")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("category");

                b.Property<int?>("CreatedBy")
                    .HasColumnType("int")
                    .HasColumnName("createdBy");

                b.Property<int>("DepotItemId")
                    .HasColumnType("int")
                    .HasColumnName("depotItem_id");

                b.Property<string>("OutInvNumber")
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnType("nvarchar(8)")
                    .HasColumnName("outInvNumber");

                b.Property<int>("ProductTitleId")
                    .HasColumnType("int")
                    .HasColumnName("productTitle_id");

                b.Property<int>("ProductWeight")
                    .HasColumnType("int")
                    .HasColumnName("productWeight");

                b.Property<DateTime>("ShipmentDate")
                    .HasColumnType("datetime")
                    .HasColumnName("shipmentDate");

                b.Property<int>("SupplierId")
                    .HasColumnType("int")
                    .HasColumnName("supplier_id");

                b.Property<string>("VehicleNumber")
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnType("nvarchar(8)")
                    .HasColumnName("vehicleNumber");

                b.HasKey("Id")
                    .HasName("PK__outputIn__3213E83F32DDA9EF");

                b.HasIndex("CreatedBy");

                b.HasIndex("DepotItemId");

                b.HasIndex("ProductTitleId");

                b.HasIndex("SupplierId");

                b.ToTable("outputInvoices", (string)null);
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.PriceByOperation", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("id");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<double>("OperationPrice")
                    .HasColumnType("float")
                    .HasColumnName("operationPrice");

                b.Property<string>("OperationTitle")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("operationTitle");

                b.Property<int>("PriceListId")
                    .HasColumnType("int")
                    .HasColumnName("priceList_id");

                b.HasKey("Id")
                    .HasName("PK__priceByO__3213E83FE07032BB");

                b.HasIndex("PriceListId");

                b.ToTable("priceByOperations", (string)null);
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.PriceList", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("id");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<int?>("CreatedBy")
                    .HasColumnType("int")
                    .HasColumnName("createdBy");

                b.Property<string>("ProductTitle")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("productTitle");

                b.HasKey("Id")
                    .HasName("PK__priceLis__3213E83FA5A83BE4");

                b.HasIndex("CreatedBy");

                b.ToTable("priceLists", (string)null);
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.ProductTitle", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("id");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("title");

                b.HasKey("Id")
                    .HasName("PK__productT__3213E83FEAC0D5A8");

                b.ToTable("productTitles", (string)null);
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.ProductionBatch", b =>
            {
                b.Property<int>("Id")
                    .HasColumnType("int")
                    .HasColumnName("id");

                b.Property<int>("AccountWeight")
                    .HasColumnType("int")
                    .HasColumnName("accountWeight");

                b.Property<double>("MoistureBase")
                    .HasColumnType("float")
                    .HasColumnName("moistureBase");

                b.Property<int>("RegisterId")
                    .HasColumnType("int")
                    .HasColumnName("register_id");

                b.Property<int>("Shrinkage")
                    .HasColumnType("int")
                    .HasColumnName("shrinkage");

                b.Property<int>("Waste")
                    .HasColumnType("int")
                    .HasColumnName("waste");

                b.Property<double>("WeedinessBase")
                    .HasColumnType("float")
                    .HasColumnName("weedinessBase");

                b.HasKey("Id")
                    .HasName("PK__producti__3213E83F49BC279D");

                b.HasIndex("RegisterId");

                b.ToTable("productionBatches", (string)null);
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.Register", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("id");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<int>("AccWeightReg")
                    .HasColumnType("int")
                    .HasColumnName("accWeightReg");

                b.Property<DateTime>("ArrivalDate")
                    .HasColumnType("datetime")
                    .HasColumnName("arrivalDate");

                b.Property<int?>("CompletionReportId")
                    .HasColumnType("int");

                b.Property<int?>("CreatedBy")
                    .HasColumnType("int")
                    .HasColumnName("createdBy");

                b.Property<int>("PhysicalWeightReg")
                    .HasColumnType("int")
                    .HasColumnName("physicalWeightReg");

                b.Property<int>("ProductTitleId")
                    .HasColumnType("int")
                    .HasColumnName("productTitle_id");

                b.Property<int>("RegisterNumber")
                    .HasColumnType("int")
                    .HasColumnName("registerNumber");

                b.Property<int>("ShrinkageReg")
                    .HasColumnType("int")
                    .HasColumnName("shrinkageReg");

                b.Property<int>("SupplierId")
                    .HasColumnType("int")
                    .HasColumnName("supplier_id");

                b.Property<int>("WasteReg")
                    .HasColumnType("int")
                    .HasColumnName("wasteReg");

                b.HasKey("Id")
                    .HasName("PK__register__3213E83F04CF8E18");

                b.HasIndex("CompletionReportId");

                b.HasIndex("CreatedBy");

                b.HasIndex("ProductTitleId");

                b.HasIndex("SupplierId");

                b.ToTable("registers", (string)null);
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.Supplier", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("id");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("title");

                b.HasKey("Id")
                    .HasName("PK__supplier__3213E83F0337A3B9");

                b.ToTable("suppliers", (string)null);
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.TechnologicalOperation", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("id");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<double>("Amount")
                    .HasColumnType("float")
                    .HasColumnName("amount");

                b.Property<int>("CompletionReportId")
                    .HasColumnType("int")
                    .HasColumnName("completionReport_id");

                b.Property<double>("Price")
                    .HasColumnType("float")
                    .HasColumnName("price");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("title");

                b.Property<double>("TotalCost")
                    .HasColumnType("float")
                    .HasColumnName("totalCost");

                b.HasKey("Id")
                    .HasName("PK__technolo__3213E83FFD310135");

                b.HasIndex("CompletionReportId");

                b.ToTable("technologicalOperations", (string)null);
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.User", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("id");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<DateTime>("BirthDate")
                    .HasColumnType("datetime")
                    .HasColumnName("birthDate");

                b.Property<string>("City")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnType("nvarchar(30)")
                    .HasColumnName("city");

                b.Property<string>("Country")
                    .HasMaxLength(30)
                    .HasColumnType("nvarchar(30)")
                    .HasColumnName("country");

                b.Property<string>("Email")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnType("nvarchar(30)")
                    .HasColumnName("email");

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnType("nvarchar(30)")
                    .HasColumnName("firstName");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnType("nvarchar(30)")
                    .HasColumnName("lastName");

                b.Property<string>("Phone")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnType("nvarchar(15)")
                    .HasColumnName("phone");

                b.HasKey("Id")
                    .HasName("PK__users__3213E83FCAE8DCA0");

                b.ToTable("users", (string)null);
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.CompletionReport", b =>
            {
                b.HasOne("GrainElevatorCS_ef.Models.User", "CreatedByNavigation")
                    .WithMany("CompletionReports")
                    .HasForeignKey("CreatedBy")
                    .HasConstraintName("FK_completionReports_users");

                b.HasOne("GrainElevatorCS_ef.Models.PriceList", "PriceList")
                    .WithMany("CompletionReports")
                    .HasForeignKey("PriceListId")
                    .HasConstraintName("FK_completionReports_priceList");

                b.HasOne("GrainElevatorCS_ef.Models.ProductTitle", "ProductTitle")
                    .WithMany("CompletionReports")
                    .HasForeignKey("ProductTitleId")
                    .IsRequired()
                    .HasConstraintName("FK_completionReports_productTitles");

                b.HasOne("GrainElevatorCS_ef.Models.Supplier", "Supplier")
                    .WithMany("CompletionReports")
                    .HasForeignKey("SupplierId")
                    .IsRequired()
                    .HasConstraintName("FK_completionReports_suppliers");

                b.Navigation("CreatedByNavigation");

                b.Navigation("PriceList");

                b.Navigation("ProductTitle");

                b.Navigation("Supplier");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.DepotItem", b =>
            {
                b.HasOne("GrainElevatorCS_ef.Models.ProductTitle", "ProductTitle")
                    .WithMany("DepotItems")
                    .HasForeignKey("ProductTitleId")
                    .IsRequired()
                    .HasConstraintName("FK_depotItems_productTitles");

                b.HasOne("GrainElevatorCS_ef.Models.Supplier", "Supplier")
                    .WithMany("DepotItems")
                    .HasForeignKey("SupplierId")
                    .IsRequired()
                    .HasConstraintName("FK_depotItems_suppliers");

                b.Navigation("ProductTitle");

                b.Navigation("Supplier");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.DepotItemCategory", b =>
            {
                b.HasOne("GrainElevatorCS_ef.Models.DepotItem", "DepotItem")
                    .WithMany("DepotItemsCategories")
                    .HasForeignKey("DepotItemId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("FK_categories_depotItem");

                b.Navigation("DepotItem");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.InputInvoice", b =>
            {
                b.HasOne("GrainElevatorCS_ef.Models.User", "CreatedByNavigation")
                    .WithMany("InputInvoices")
                    .HasForeignKey("CreatedBy")
                    .HasConstraintName("FK_inputInvoices_users");

                b.HasOne("GrainElevatorCS_ef.Models.ProductTitle", "ProductTitle")
                    .WithMany("InputInvoices")
                    .HasForeignKey("ProductTitleId")
                    .IsRequired()
                    .HasConstraintName("FK_inputInvoices_productTitles");

                b.HasOne("GrainElevatorCS_ef.Models.Supplier", "Supplier")
                    .WithMany("InputInvoices")
                    .HasForeignKey("SupplierId")
                    .IsRequired()
                    .HasConstraintName("FK_inputInvoices_suppliers");

                b.Navigation("CreatedByNavigation");

                b.Navigation("ProductTitle");

                b.Navigation("Supplier");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.LaboratoryCard", b =>
            {
                b.HasOne("GrainElevatorCS_ef.Models.User", "CreatedByNavigation")
                    .WithMany("LaboratoryCards")
                    .HasForeignKey("CreatedBy")
                    .HasConstraintName("FK_LaboratoryCards_users");

                b.HasOne("GrainElevatorCS_ef.Models.InputInvoice", "IdNavigation")
                    .WithOne("LaboratoryCard")
                    .HasForeignKey("GrainElevatorCS_ef.Models.LaboratoryCard", "Id")
                    .IsRequired()
                    .HasConstraintName("FK_laboratoryCards_inputInvoice");

                b.Navigation("CreatedByNavigation");

                b.Navigation("IdNavigation");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.OutputInvoice", b =>
            {
                b.HasOne("GrainElevatorCS_ef.Models.User", "CreatedByNavigation")
                    .WithMany("OutputInvoices")
                    .HasForeignKey("CreatedBy")
                    .HasConstraintName("FK_outputInvoices_users");

                b.HasOne("GrainElevatorCS_ef.Models.DepotItem", "DepotItem")
                    .WithMany("OutputInvoices")
                    .HasForeignKey("DepotItemId")
                    .IsRequired()
                    .HasConstraintName("FK_outputInvoices_depotItems");

                b.HasOne("GrainElevatorCS_ef.Models.ProductTitle", "ProductTitle")
                    .WithMany("OutputInvoices")
                    .HasForeignKey("ProductTitleId")
                    .IsRequired()
                    .HasConstraintName("FK_outputInvoices_productTitles");

                b.HasOne("GrainElevatorCS_ef.Models.Supplier", "Supplier")
                    .WithMany("OutputInvoices")
                    .HasForeignKey("SupplierId")
                    .IsRequired()
                    .HasConstraintName("FK_outputInvoices_suppliers");

                b.Navigation("CreatedByNavigation");

                b.Navigation("DepotItem");

                b.Navigation("ProductTitle");

                b.Navigation("Supplier");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.PriceByOperation", b =>
            {
                b.HasOne("GrainElevatorCS_ef.Models.PriceList", "PriceList")
                    .WithMany("PriceByOperations")
                    .HasForeignKey("PriceListId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("FK_operationPrices_priceList");

                b.Navigation("PriceList");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.PriceList", b =>
            {
                b.HasOne("GrainElevatorCS_ef.Models.User", "CreatedByNavigation")
                    .WithMany("PriceLists")
                    .HasForeignKey("CreatedBy")
                    .HasConstraintName("FK_priceLists_users");

                b.Navigation("CreatedByNavigation");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.ProductionBatch", b =>
            {
                b.HasOne("GrainElevatorCS_ef.Models.LaboratoryCard", "IdNavigation")
                    .WithOne("ProductionBatch")
                    .HasForeignKey("GrainElevatorCS_ef.Models.ProductionBatch", "Id")
                    .IsRequired()
                    .HasConstraintName("FK_productionBatches_laboratoryCard");

                b.HasOne("GrainElevatorCS_ef.Models.Register", "Register")
                    .WithMany("ProductionBatches")
                    .HasForeignKey("RegisterId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("FK_productionBatches_register");

                b.Navigation("IdNavigation");

                b.Navigation("Register");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.Register", b =>
            {
                b.HasOne("GrainElevatorCS_ef.Models.CompletionReport", "CompletionReport")
                    .WithMany("Registers")
                    .HasForeignKey("CompletionReportId")
                    .HasConstraintName("FK_registers_completionReports");

                b.HasOne("GrainElevatorCS_ef.Models.User", "CreatedByNavigation")
                    .WithMany("Registers")
                    .HasForeignKey("CreatedBy")
                    .HasConstraintName("FK_registers_users");

                b.HasOne("GrainElevatorCS_ef.Models.ProductTitle", "ProductTitle")
                    .WithMany("Registers")
                    .HasForeignKey("ProductTitleId")
                    .IsRequired()
                    .HasConstraintName("FK_registers_productTitles");

                b.HasOne("GrainElevatorCS_ef.Models.Supplier", "Supplier")
                    .WithMany("Registers")
                    .HasForeignKey("SupplierId")
                    .IsRequired()
                    .HasConstraintName("FK_registers_suppliers");

                b.Navigation("CompletionReport");

                b.Navigation("CreatedByNavigation");

                b.Navigation("ProductTitle");

                b.Navigation("Supplier");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.TechnologicalOperation", b =>
            {
                b.HasOne("GrainElevatorCS_ef.Models.CompletionReport", "CompletionReport")
                    .WithMany("TechnologicalOperations")
                    .HasForeignKey("CompletionReportId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("FK_technologicalOperations_completionReport");

                b.Navigation("CompletionReport");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.CompletionReport", b =>
            {
                b.Navigation("Registers");

                b.Navigation("TechnologicalOperations");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.DepotItem", b =>
            {
                b.Navigation("DepotItemsCategories");

                b.Navigation("OutputInvoices");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.InputInvoice", b =>
            {
                b.Navigation("LaboratoryCard");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.LaboratoryCard", b =>
            {
                b.Navigation("ProductionBatch");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.PriceList", b =>
            {
                b.Navigation("CompletionReports");

                b.Navigation("PriceByOperations");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.ProductTitle", b =>
            {
                b.Navigation("CompletionReports");

                b.Navigation("DepotItems");

                b.Navigation("InputInvoices");

                b.Navigation("OutputInvoices");

                b.Navigation("Registers");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.Register", b =>
            {
                b.Navigation("ProductionBatches");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.Supplier", b =>
            {
                b.Navigation("CompletionReports");

                b.Navigation("DepotItems");

                b.Navigation("InputInvoices");

                b.Navigation("OutputInvoices");

                b.Navigation("Registers");
            });

            modelBuilder.Entity("GrainElevatorCS_ef.Models.User", b =>
            {
                b.Navigation("CompletionReports");

                b.Navigation("InputInvoices");

                b.Navigation("LaboratoryCards");

                b.Navigation("OutputInvoices");

                b.Navigation("PriceLists");

                b.Navigation("Registers");
            });
#pragma warning restore 612, 618
        }
    }
}
