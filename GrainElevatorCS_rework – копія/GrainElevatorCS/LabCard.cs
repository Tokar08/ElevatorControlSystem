using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

// Лабораторная карточка (карточка анализа).
// =========================================
// Содержит первичную информацию о партии Продукции из товарно-транспортной накладной,
// результаты анализа ее качественных показателей.

namespace GrainElevatorCS
{
    public class LabCard
    {
        //входящие данные (из вх.накладной InputInvoice)
        public DateTime Date { get; set; } = new DateTime(0001, 01, 01); // дата прихода 
        public string InvNumber { get; set; } = string.Empty; // номер входящей накладной
        public string Supplier { get; set; } = string.Empty; // наименование предприятия Поставщика
        public string ProductTitle { get; set; } = string.Empty; // наименование Продукции
        public int PhysicalWeight { get; set; } = 0; // Физический вес Продукции


        // данные, вводимие лаборантом при создании LabCard
        public int LabCardNumber { get; set; } = 0; // номер Карточки анализа
        public double Weediness { get; set; } = 0; // Сорная примесь Продукции
        public double Moisture { get; set; } = 0;  // Влажность Продукции
        public bool IsProduction { get; set; } = false; // напрвляется ли в Производство

        // необязательние поля
        public string? GrainImpurity { get; set; } = string.Empty; // зерновая/масличная примесь
        public string? SpecialNotes { get; set; } = string.Empty; // особие отметки


        public LabCard() { }
        public LabCard(int labCardNumber, DateTime date, string invNumber,  string supplier, string productTitle, int physicalWeight, double weediness, double moisture, string grainImpurity = "", string specialNotes = "")
        {
            LabCardNumber = labCardNumber;
            Date = date;
            InvNumber = invNumber;
            Supplier = supplier;
            ProductTitle = productTitle;
            PhysicalWeight = physicalWeight;
            Weediness = weediness;
            Moisture = moisture;
            GrainImpurity = grainImpurity;
            SpecialNotes = specialNotes;
        }

        public LabCard(InputInvoice inv, int labCardNumber, double weediness, double moisture, string grainImpurity = "", string specialNotes = "") 
        {
            Date = inv.Date;
            InvNumber = inv.InvNumber;
            Supplier = inv.Supplier;
            ProductTitle = inv.ProductTitle;
            PhysicalWeight = inv.PhysicalWeight;

            LabCardNumber = labCardNumber;
            Weediness = weediness;
            Moisture = moisture;
            GrainImpurity = grainImpurity;
            SpecialNotes = specialNotes;
        }







        // для теста на КОНСОЛИ
        public LabCard RequestLabInfo(LabCard lc)
        {
            Console.WriteLine("\tВведите результаты лабораторного анализа входящей Продукции: \n");            
            while (true)
            {
                try
                {
                    NumberFormatInfo numberFormatInfo = new NumberFormatInfo() // установка типа разделителя "." в числах с плавающей запятой
                    {
                        NumberDecimalSeparator = ".",
                    };

                    Console.Write("Сорная Примесь (%):    ");
                    lc.Weediness = double.Parse(Console.ReadLine(), numberFormatInfo);
                    
                    Console.Write("Влажность (%):         ");
                    lc.Moisture = double.Parse(Console.ReadLine(), numberFormatInfo);

                    if (lc.Weediness < 0 || lc.Weediness > 100 || lc.Moisture < 0 || lc.Moisture > 100) // провеока полученних данних на диапазон 0-100%
                        throw new Exception();

                    return lc;
                }
                catch (Exception)
                {
                    Console.WriteLine("Ошибка ввода. Введите корректние значения данних в диапазоне от 0 до 100");
                }
            }
        }
        public override string ToString()
        {
            return $"Номер Карточки анализа: {LabCardNumber}\n" +
                   $"Дата прихода:           {Date.ToString("dd.MM.yyyy")}\n" +
                   $"Приходная накладная     №{InvNumber}\n" +
                   
                   $"Поставщик:              {Supplier}\n" +
                   $"Наименование:           {ProductTitle}\n" +
                   $"Физический вес:         {PhysicalWeight} кг\n"+

                   $"Сорная примесь:         {Weediness} %\n" +
                   $"Влажность:              {Moisture} %\n";
        }
    };    
}
