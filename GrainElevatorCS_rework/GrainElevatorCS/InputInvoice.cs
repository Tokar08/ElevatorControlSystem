﻿using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GrainElevatorCS
{
    // Приходная накладная.
    // ===============================
    // Содержит первичную информацию о входящей партии продукции.
    public class InputInvoice : ISaveToDb
    {
        public string InvNumber { get; set; } = string.Empty; // номер входящей накладной
        public DateTime Date { get; set; } = new DateTime(0001, 01, 01); // дата прихода 
        public string VenicleNumber { get; set; } = string.Empty; // гос.номер транспортного средства
        public string Supplier { get; set; } = string.Empty; // наименование предприятия Поставщика
        public string ProductTitle { get; set; } = string.Empty; // наименование Продукции
        public int PhysicalWeight { get; set; } = 0; // Физический вес Продукции
        public string? CreatedBy { get; set; } = string.Empty; // имя пользователя-создателя


        public InputInvoice() { }

        public InputInvoice(string invNumber, DateTime date,  string venicleNumber, string supplier, string productTitle, int physicalWeight)
        {
            InvNumber = invNumber;
            Date = date;
            VenicleNumber = venicleNumber;
            Supplier = supplier;
            ProductTitle = productTitle;
            PhysicalWeight = physicalWeight;   
        }


        public async Task SaveAllInfo(string connString, string databaseName, string tableName, params object[] objects)
        {
            string query = @"INSERT INTO" + $"{tableName}" + "(invNumber, date, venicleNumber, supplier, productTitle, physicalWeight, createdBy)" +
                                          "VALUES (@invNumber, @date, @venicleNumber, @supplier, @productTitle, @physicalWeight, @createdBy)";

            using SqlConnection conn = new SqlConnection(connString);

            try
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlParameter invNumberParam = new SqlParameter("@invNumber", SqlDbType.VarChar, 10)
                {
                    Value = objects[0]
                };
                cmd.Parameters.Add(invNumberParam);

                SqlParameter dateParam = new SqlParameter("@date", SqlDbType.Date)
                {
                    Value = objects[1]
                };
                cmd.Parameters.Add(dateParam);

                SqlParameter venicleNumberParam = new SqlParameter("@venicleNumber", SqlDbType.VarChar, 8)
                {
                    Value = objects[2]
                };
                cmd.Parameters.Add(venicleNumberParam);

                SqlParameter supplierParam = new SqlParameter("@supplier", SqlDbType.VarChar, 50)
                {
                    Value = objects[3]
                };
                cmd.Parameters.Add(supplierParam);

                SqlParameter productTitleParam = new SqlParameter("@productTitle", SqlDbType.VarChar, 30)
                {
                    Value = objects[4]
                };
                cmd.Parameters.Add(productTitleParam);

                SqlParameter physicalWeightParam = new SqlParameter("@physicalWeight", SqlDbType.Int)
                {
                    Value = objects[5]
                };
                cmd.Parameters.Add(physicalWeightParam);

                cmd.Parameters.AddWithValue("@createdBy", objects[6]);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");  //  TODO MessageBox
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }







        // ДЛЯ ТЕСТА НА КОНСОЛИ===================================================================================
        public InputInvoice RequestInvoiceInfo(InputInvoice inInv)
        {
            Console.WriteLine("\tВведите информацию для оформления приходной накладной на входящую Продукцию: \n");

            while (true)
            {
                try
                {
                    Console.Write("Номер приходной накладной:                    ");
                    inInv.InvNumber = Console.ReadLine();

                    Console.Write("Дата поступления:                             ");
                    inInv.Date = Convert.ToDateTime(Console.ReadLine());

                    Console.Write("Регистрационний номер транспортного средства: ");
                    inInv.VenicleNumber = Console.ReadLine();

                    Console.Write("Наименование Поставщика:                      ");
                    inInv.Supplier = Console.ReadLine();

                    Console.Write("Наименование поступившей Продукции:           ");
                    inInv.ProductTitle = Console.ReadLine();

                    Console.Write("Физический вес поступившей Продукции (кг):    ");
                    inInv.PhysicalWeight = Convert.ToInt32(Console.ReadLine());

                    return inInv;
                }
                catch (Exception)
                {
                    Console.WriteLine("Ошибка ввода. Введите корректные значения данных.");
                }
            }
        }

        public override string ToString()
        {
            return $"Приходная накладная №{InvNumber}\n" +
                   $"Дата прихода:        {Date.ToString("dd.MM.yyyy")}\n" +
                   $"Номер ТС:            {VenicleNumber}\n" +
                   $"Поставщик:           {Supplier}\n" +
                   $"Наименование:        {ProductTitle}\n" +
                   $"Вес нетто:           {PhysicalWeight} кг\n";
        }
    }
}
