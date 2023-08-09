using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GrainElevatorCS
{
    public class CompletionReport
    {
        public int ReportNumber { get; set; } = 0; // номер Реестра
        public DateTime Date { get; set; } = DateTime.Now; // дата прихода 
        public string Supplier { get; set; } = string.Empty; // наименование предприятия Поставщика
        public string ProductTitle { get; set; } = string.Empty; // наименование Продукции

        public List<TechnologicalOperation>? Operations { get; set; } // коллекция технологических операций

        private double quantityesDrying = 0; // кол-во тонно/процентов сушки всех ППП всех Реестров Акта
        private double physicalWeightReport = 0; // общий физический вес Акта доработки

        public CompletionReport(int reportNum, DateTime date, params Register[] registers)
        {
            if (registers != null)
            {
                ReportNumber = reportNum;
                Date = date;
                Supplier = registers[0].Supplier;
                ProductTitle = registers[0].ProductTitle;

                physicalWeightReport = CalcSumWeightReport(registers);
                quantityesDrying = CalcDryingQuantity(registers); 
            }

            Operations = new List<TechnologicalOperation>()
            { 
                new TechnologicalOperation("Приемка"),
                new TechnologicalOperation("Первичная очистка"),
                new TechnologicalOperation("Сушка в шахтной сушилке"),    
            };

            initOperationsValue();
        }

        // присвоение технологическим операциям переменних количественних значений
        private void initOperationsValue()
        {
            try
            {
                Operations?.ForEach(op =>
                {
                    switch (op.Title)
                    {
                        case "Приемка":
                            op.Amount = physicalWeightReport;
                            break;

                        case "Первичная очистка":
                            op.Amount = physicalWeightReport;
                            break;

                        case "Сушка в шахтной сушилке":
                            op.Amount = quantityesDrying;
                            break;

                    }
                });
            }
            catch (Exception)
            {
                // TODO
                throw;
            }

        }

        // рассчет Акта доработка по заданному Прайсу
        public void CalcByPrice(Price price)
        {
            if (price.OperationPrices == null)
                return;

            try
            {
                Operations?.ForEach(op =>
                {
                    foreach (KeyValuePair<string, double> p in price.OperationPrices)
                        if (op.Title == p.Key)
                        {
                            op.Price = p.Value;
                            op.TotalCost = op.Amount * op.Price;
                        }
                });
            }
            catch (Exception)
            {
                // TODO
                throw;
            }
        }

        // добавление в Акт доработки Технологических операций по одной
        private void addOperations (TechnologicalOperation operation)
        {
            try
            {
                Operations?.Add(operation);
            }
            catch (Exception)
            {
                // TODO
                throw;
            }
        }
        // добавление в Акт доработки Технологических операций перечнем
        private void addOperationsRange(params TechnologicalOperation[] operation)
        {
            try
            {
                Operations?.AddRange(operation);
            }
            catch (Exception)
            {
                // TODO
                throw;
            }
        }



        // рассчет общего Сумми Физического веса всех Реестров
        private double CalcSumWeightReport(params Register[] registers)
        {
            try
            {
                foreach (Register reg in registers) 
                    physicalWeightReport += (double)reg.PhysicalWeightReg/1000;

                return physicalWeightReport;
            }
            catch (Exception)
            {
                // TODO
                throw;
            }
        }

        // расчет тонно/процентов сушки по каждой ППП всех Реестров Акта
        private double CalcDryingQuantity(params Register[] registers) 
        {
            if (registers == null)
                return 0.0;

            try
            {
                foreach (Register reg in registers)
                {
                    if (reg.prodBatches == null)
                        return 0.0;

                    reg.prodBatches.ForEach(p =>
                        {
                            if (p.Shrinkage != 0)
                                quantityesDrying += ((p.PhysicalWeight - p.Waste) * (p.Moisture - p.MoistureBase) / 1000);
                        });
                }
                return quantityesDrying;
            }
            catch (Exception)
            {
                // TODO
                throw;
            }
        }










        // вивод Акта на КОНСОЛЬ
        public void PrintReport()
        {
            Console.WriteLine($"Акт доработки     №{ReportNumber}");
            Console.WriteLine($"Дата составления: {Date.ToString("dd.MM.yyyy")}");
            Console.WriteLine($"Поставщик:        {Supplier}");
            Console.WriteLine($"Наименование:     {ProductTitle}\n");

            Console.WriteLine($"Технологические операции:");
            Operations?.ForEach(op => Console.WriteLine($"{op.Title}|| {string.Format("{0:F2}", op.Amount)}  || {op.Price} грн/т || {string.Format("{0:F2}", op.TotalCost)} грн. "));

            Console.WriteLine("\n");
        }
    }


}

