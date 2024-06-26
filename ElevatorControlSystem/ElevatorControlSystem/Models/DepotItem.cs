﻿using ElevatorControlSystem.Repozitories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace GrainElevatorCS_ef.Models;

public partial class DepotItem
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    public int ProductTitleId { get; set; }

    public virtual ICollection<DepotItemCategory> DepotItemsCategories { get; set; } = new List<DepotItemCategory>();

    public virtual ICollection<OutputInvoice> OutputInvoices { get; set; } = new List<OutputInvoice>();

    public virtual ProductTitle ProductTitle { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;


    public DepotItem() { }


    public DepotItem(Register register)
    {
        SupplierId = register.SupplierId;
        ProductTitleId = register.ProductTitleId;
        DepotItemsCategories = new List<DepotItemCategory>()
        {
            new DepotItemCategory("Кондиционная продукция", register.AccWeightReg),
            new DepotItemCategory("Отход", register.WasteReg)
        };
    }


    public DepotItem(List<Register> registers)
    {
        SupplierId = registers[0].SupplierId;
        ProductTitleId = registers[0].ProductTitleId;
        DepotItemsCategories = new List<DepotItemCategory>();

        foreach (Register r in registers)
        {
            if (SupplierId == r.SupplierId && ProductTitleId == r.ProductTitleId)
            {
                foreach (var c in DepotItemsCategories)
                {
                    if (c.CategoryTitle == "Кондиционная продукция")
                        c.CategoryValue += r.AccWeightReg;

                    if (c.CategoryTitle == "Отход")
                        c.CategoryValue += r.WasteReg;
                }
            }
            else
                Console.WriteLine("Поставщик или Наименование продукции Реестра не соответствуют Складской единице");
        }
    }

    public bool IsAddedRegister(Register register)
    {
   
            if (SupplierId == register.SupplierId && ProductTitleId == register.ProductTitleId)
            {

                SupplierId = register.SupplierId;
                ProductTitleId = register.ProductTitleId;
       
                foreach (var c in DepotItemsCategories)
                {
                    if (c.CategoryTitle == "Кондиционная продукция")
                        c.CategoryValue += register.AccWeightReg;

                    if (c.CategoryTitle == "Отход")
                        c.CategoryValue += register.WasteReg;
                }
                return true;
            }
            else { return false; }
     
    }

}