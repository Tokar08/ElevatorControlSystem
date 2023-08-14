using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GrainElevatorCS
{
    public class CompletionReport : ISaveToDb
    {
        public int ReportNumber { get; set; } = 0; // номер Реестра
        public DateTime Date { get; set; } = DateTime.Now; // дата создания
        public string Supplier { get; set; } = string.Empty; // наименование предприятия Поставщика
        public string ProductTitle { get; set; } = string.Empty; // наименование Продукции

        public List<TechnologicalOperation>? Operations { get; set; } // коллекция технологических операций
        
        private double quantityesDrying = 0; // кол-во тонно/процентов сушки всех ППП всех Реестров Акта
        private double physicalWeightReport = 0; // общий физический вес Акта доработки

        public bool IsFinalized { get; set; } = false; // завершен бухгалтерией
        public string? CreatedBy { get; set; } = string.Empty; // имя пользователя-создателя

        public CompletionReport(int reportNum, DateTime date, List<Register> registers)
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

                IsFinalized = true;
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
        private double CalcSumWeightReport(List<Register> registers)
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
        private double CalcDryingQuantity(List<Register> registers) 
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

        public async Task SaveAllInfo(string connString, string databaseName, string tableName, params object[] objects)
        {
            string query = @"INSERT INTO" + $"{tableName}" + "(reportNumber, date, supplier, productTitle, operations, quantityesDrying, physicalWeightReport, isFinalized, createdBy)" +
                                          "VALUES (@reportNumber, @date, @supplier, @productTitle, @operations, @quantityesDrying, @physicalWeightReport, @isFinalized, @createdBy)";

            using SqlConnection conn = new SqlConnection(connString);

            try
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlParameter reportNumberParam = new SqlParameter("@reportNumber", SqlDbType.Int)
                {
                    Value = objects[0]
                };
                cmd.Parameters.Add(reportNumberParam);

                SqlParameter dateParam = new SqlParameter("@date", SqlDbType.Date)
                {
                    Value = objects[1]
                };

                cmd.Parameters.AddWithValue("@supplier", objects[2]);
                cmd.Parameters.AddWithValue("@productTitle", objects[3]);
                cmd.Parameters.AddWithValue("@operations", objects[4]);
                cmd.Parameters.AddWithValue("@quantityesDrying", objects[5]);
                cmd.Parameters.AddWithValue("@physicalWeightReport", objects[6]);
                cmd.Parameters.AddWithValue("@isFinalized", objects[7]);
                cmd.Parameters.AddWithValue("@createdBy", objects[8]);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");  //  TODO
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }








        // вивод Акта на КОНСОЛЬ =================================================================================================================
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

