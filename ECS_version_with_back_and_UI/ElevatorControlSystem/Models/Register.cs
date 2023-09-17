using ElevatorControlSystem.Models;
using System;
using System.Collections.Generic;

namespace GrainElevatorCS_ef.Models;

public partial class Register
{
    public int Id { get; set; }

    public int RegisterNumber { get; set; }

    public DateTime ArrivalDate { get; set; }

    public int SupplierId { get; set; }

    public int ProductTitleId { get; set; }

    public int PhysicalWeightReg { get; set; }

    public int ShrinkageReg { get; set; }

    public int WasteReg { get; set; }

    public int AccWeightReg { get; set; }

    public int? CompletionReportId { get; set; }
    public virtual CompletionReport? CompletionReport { get; set; }

    public int? CreatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ProductTitle ProductTitle { get; set; } = null!;

    public virtual ICollection<ProductionBatch> ProductionBatches { get; set; } = new List<ProductionBatch>();

    public virtual Supplier Supplier { get; set; } = null!;


    public Register()
    { }

    public Register(int regNum, double weedinessBase, double moistureBase, List<LaboratoryCard> laboratoryCards)
    {
        if (laboratoryCards.Count != 0)
        {
            ProductionBatches = new List<ProductionBatch>();

            RegisterNumber = regNum;
            ArrivalDate = laboratoryCards[0].IdNavigation.ArrivalDate;
            SupplierId = laboratoryCards[0].IdNavigation.SupplierId;
            ProductTitleId = laboratoryCards[0].IdNavigation.ProductTitleId;

            InitProductionBatches(weedinessBase, moistureBase, laboratoryCards);
        }
    }

    private void InitProductionBatches(double weedinessBase, double moistureBase, List<LaboratoryCard> laboratoryCards)
    {
        foreach (var lc in laboratoryCards!)
        {
            try
            {
                ProductionBatch pb = new ProductionBatch(lc.IdNavigation, lc, weedinessBase, moistureBase);
                ProductionBatches.Add(pb);

                PhysicalWeightReg += lc.IdNavigation.PhysicalWeight;
                AccWeightReg += pb.AccountWeight;
                WasteReg += pb.Waste;
                ShrinkageReg += pb.Shrinkage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }















    // ТЕСТ ДЛЯ КОНСОЛИ ==============================================================================================================
    public void PrintReg()
    {
        Console.WriteLine($"Реестр:          №{RegisterNumber}");
        Console.WriteLine($"Поставщик:       {SupplierId}");
        Console.WriteLine($"Наименование:    {ProductTitleId}");
        Console.WriteLine(new string('=', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
        Console.WriteLine("|{0,8}|{1,10}|{2,15}|{3,10}|{4,10}|{5,10}|{6,10}|{7,15}|", "Дата прихода", "Номер ТТН", "Физический вес", "Влажность", "Усушка", "Сорность", "Отход", "Зачетный вес");
        Console.WriteLine(new string('=', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));

        if (ProductionBatches != null)
        {
            foreach (var pb in ProductionBatches)
                pb.PrintProductionBatch(pb.IdNavigation.IdNavigation, pb.IdNavigation);
        }

        Console.WriteLine(new string('=', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
        Console.WriteLine("|{0,12}|{1,10}|{2,15}|{3,10}|{4,10}|{5,10}|{6,10}|{7,15}|", "Итого", " ", PhysicalWeightReg, " ", ShrinkageReg, " ", WasteReg, AccWeightReg);
        Console.WriteLine(new string('=', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
        Console.WriteLine("\n");
    }
}
