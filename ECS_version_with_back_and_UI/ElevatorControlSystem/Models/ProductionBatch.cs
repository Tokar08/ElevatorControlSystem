using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrainElevatorCS_ef.Models;

public partial class ProductionBatch
{
    public int Id { get; set; }
    public double WeedinessBase { get; set; }

    public double MoistureBase { get; set; }

    public int Waste { get; set; }

    public int Shrinkage { get; set; }

    public int AccountWeight { get; set; }

    public int RegisterId { get; set; }

    public virtual LaboratoryCard IdNavigation { get; set; } = null!;

    public virtual Register Register { get; set; } = null!;



    public ProductionBatch() { }

    public ProductionBatch(InputInvoice inv, LaboratoryCard lc, double weedinessBase, double moistureBase)
    {
        Id = lc.Id;
        WeedinessBase = weedinessBase;
        MoistureBase = moistureBase;

        ProductionButchCalculator pbCalculator = new(inv, lc, this);

        pbCalculator.CalcResultProduction();
    }

























    // вывод информации (ДЛЯ ТЕСТА НА КОНСОЛИ)
    public override string ToString()
    {
        return //$"Дата прихода:              {ArrivalDate.ToString("dd.MM.yyyy")}\n" +
               //$"Номер Карточки анализа:    {LabCardNumber}\n" +
               //$"Поставщик:                 {Supplier}\n" +
               //$"Наименование:              {ProductTitle}\n" +
               //$"физический вес:            {PhysicalWeight} кг\n" +

               //$"Сорная примесь:    {Weediness} %\n" +
               //$"Влажность:         {Moisture} %\n\n" +

               $"Базовая сорность:  {WeedinessBase} %\n" +
               $"Базовая Влажность: {MoistureBase} %\n\n" +

               $"Усушка:            {Shrinkage} кг\n" +
               $"Сорная убыль:      {Waste} кг\n" +
               $"Зачетный вес:      {AccountWeight} кг\n";
    }


    public void PrintProductionBatch(InputInvoice inv, LaboratoryCard lc)
    {
        if (this != null)
        {
            Console.WriteLine(new string('-', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
            Console.WriteLine("|{0,12}|{1,10}|{2,15}|{3,10}|{4,10}|{5,10}|{6,10}|{7,15}|", inv.ArrivalDate.ToString("dd.MM.yyyy"), inv.InvNumber, inv.PhysicalWeight, lc.Moisture, Shrinkage, lc.Weediness, Waste, AccountWeight);
            Console.WriteLine(new string('-', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
        }
    }
}
