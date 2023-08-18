using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections;
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

        public List<TechnologicalOperation>? TechnologicalOperations { get; set; } // коллекция технологических операций
        
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

            TechnologicalOperations = new List<TechnologicalOperation>()
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
                TechnologicalOperations?.ForEach(op =>
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
            if (price.PriceByOperations == null)
                return;

            try
            {
                TechnologicalOperations?.ForEach(op =>
                {
                    foreach (KeyValuePair<string, double> p in price.PriceByOperations)
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
                TechnologicalOperations?.Add(operation);
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
                TechnologicalOperations?.AddRange(operation);
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
                    if (reg.ProuctionBatches == null)
                        return 0.0;

                    reg.ProuctionBatches.ForEach(p =>
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



        // вставка данних в две связанние таблици (Акт доработки содержит список Технологических операций)
        public async Task SaveAllInfo(string connString, string databaseName, params string[] tableNames)
        {
            using SqlConnection conn = new SqlConnection(connString);

            try
            {
                await conn.OpenAsync();

                int lastInsertedReport_id = Convert.ToInt32(insertInFirstTable(conn, tableNames[0]));
                insertInSecondTable(conn, tableNames[1], lastInsertedReport_id);
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

       
        private object insertInFirstTable(SqlConnection conn, string tableName)
        {
            // добавление данних в первую таблицу - сompletionReports
            string query = @"INSERT INTO " + $"{tableName} " + "(reportNumber, reportDate, supplier, productTitle, quantityesDrying, physicalWeightReport, isFinalized, createdBy) " +
                                      "VALUES (@reportNumber, @reportDate, @supplier, @productTitle, @quantityesDrying, @physicalWeightReport, @isFinalized, @createdBy);" +
                             "SELECT scope_identity();";

            SqlCommand cmd = new SqlCommand(query, conn);

            SqlParameter reportNumberParam = new SqlParameter("@reportNumber", SqlDbType.Int)
            {
                Value = ReportNumber
            };
            cmd.Parameters.Add(reportNumberParam);

            SqlParameter dateParam = new SqlParameter("@reportDate", SqlDbType.Date)
            {
                Value = Date
            };
            cmd.Parameters.Add(dateParam);

            cmd.Parameters.AddWithValue("@supplier", Supplier);
            cmd.Parameters.AddWithValue("@productTitle", ProductTitle);
            cmd.Parameters.AddWithValue("@quantityesDrying", quantityesDrying);
            cmd.Parameters.AddWithValue("@physicalWeightReport", physicalWeightReport);
            cmd.Parameters.AddWithValue("@isFinalized", IsFinalized);
            cmd.Parameters.AddWithValue("@createdBy", CreatedBy);

            return cmd.ExecuteScalar();
        }

        private void insertInSecondTable(SqlConnection conn, string tableName, int lastInsertedReport_id)
        {
            // добавление данних во вторую таблицу - technologicalOperations (из первого запроса получаем id вставленного Акта доработки и вставляем его во вторую таблицу)
            if (TechnologicalOperations == null)
                return;

            foreach (var op in TechnologicalOperations)
            {
                string query = @"INSERT INTO " + $"{tableName}" + "(completionReport_id, title, amount, price, totalCost) " +
                                             "VALUES (@completionReport_id, @title, @amount, @price, @totalCost)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@completionReport_id", lastInsertedReport_id);
                cmd.Parameters.AddWithValue("@title", op.Title);
                cmd.Parameters.AddWithValue("@amount", op.Amount);
                cmd.Parameters.AddWithValue("@price", op.Price);
                cmd.Parameters.AddWithValue("@totalCost", op.TotalCost);

                cmd.ExecuteNonQuery();
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
            TechnologicalOperations?.ForEach(op => Console.WriteLine($"{op.Title}|| {string.Format("{0:F2}", op.Amount)}  || {op.Price} грн/т || {string.Format("{0:F2}", op.TotalCost)} грн. "));

            Console.WriteLine("\n");
        }
    }


}

