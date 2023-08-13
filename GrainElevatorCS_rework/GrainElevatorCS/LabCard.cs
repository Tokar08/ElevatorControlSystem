using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class LabCard : ISaveToDb
    {
        //входящие данные (из вх.накладной InputInvoice)
        public DateTime Date { get; set; } = DateTime.Now; // дата прихода 
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
        public string? CreatedBy { get; set; } = string.Empty; // имя пользователя-создателя


        public LabCard() { }
        public LabCard(int labCardNumber, string invNumber, DateTime date, string supplier, string productTitle, int physicalWeight, double weediness, double moisture, string grainImpurity = "", string specialNotes = "")
        {
            LabCardNumber = labCardNumber;
            InvNumber = invNumber;
            Date = date;
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

        public async Task SaveAllInfo(string connString, string databaseName, string tableName, params object[] objects)
        {
            string query = @"INSERT INTO" + $"{tableName}" + "(labCardNumber, invNumber, date, venicleNumber, supplier, productTitle, physicalWeight, weediness, moisture, grainImpurity, specialNotes, isProduction, createdBy)" +
                                          "VALUES (@labCardNumber, @invNumber, @date, @venicleNumber, @supplier, @productTitle, @physicalWeight, @weediness, @moisture, @grainImpurity, @specialNotes, @isProduction, @createdBy)";

            using SqlConnection conn = new SqlConnection(connString);

            try
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlParameter labCardNumberParam = new SqlParameter("@labCardNumber", SqlDbType.Int)
                {
                    Value = objects[0]
                };
                cmd.Parameters.Add(labCardNumberParam);

                SqlParameter invNumberParam = new SqlParameter("@invNumber", SqlDbType.VarChar, 10)
                {
                    Value = objects[1]
                };
                cmd.Parameters.Add(invNumberParam);

                SqlParameter dateParam = new SqlParameter("@date", SqlDbType.Date)
                {
                    Value = objects[2]
                };
                cmd.Parameters.Add(dateParam);

                SqlParameter venicleNumberParam = new SqlParameter("@venicleNumber", SqlDbType.VarChar, 12)
                {
                    Value = objects[3]
                };
                cmd.Parameters.Add(venicleNumberParam);

                SqlParameter supplierParam = new SqlParameter("@supplier", SqlDbType.VarChar, 50)
                {
                    Value = objects[4]
                };
                cmd.Parameters.Add(supplierParam);

                SqlParameter productTitleParam = new SqlParameter("@productTitle", SqlDbType.VarChar, 30)
                {
                    Value = objects[5]
                };
                cmd.Parameters.Add(productTitleParam);

                SqlParameter physicalWeightParam = new SqlParameter("@physicalWeight", SqlDbType.Int)
                {
                    Value = objects[6]
                };
                cmd.Parameters.Add(physicalWeightParam);

                SqlParameter weedinessParam = new SqlParameter("@weediness", SqlDbType.Float)
                {
                    Value = objects[7]
                };
                cmd.Parameters.Add(weedinessParam);

                SqlParameter moistureParam = new SqlParameter("@moisture", SqlDbType.Float)
                { 
                    Value = objects[8]
                };
                cmd.Parameters.Add(moistureParam);

                SqlParameter grainImpurityParam = new SqlParameter("@grainImpurity", SqlDbType.Float)
                {
                    Value = objects[9]
                };
                cmd.Parameters.Add(grainImpurityParam);

                SqlParameter specialNotesParam = new SqlParameter("@specialNotes", SqlDbType.VarChar, 50)
                {
                    Value = objects[10]
                };
                cmd.Parameters.Add(specialNotesParam);

                SqlParameter isProductionParam = new SqlParameter("@isProduction", SqlDbType.Bit)
                {
                    Value = objects[11]
                };
                cmd.Parameters.Add(isProductionParam);

                cmd.Parameters.AddWithValue("@createdBy", objects[12]);

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





        // для теста на КОНСОЛИ===================================================================================================
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
