using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

//Складская учетная единица.
//==========================
//Суммарные количественные показатели категорий Наименования продукции: Зачетный вес, Отход.

namespace GrainElevatorCS
{
    public class DepotItem : ISaveToDb
    {
        public string Supplier { get; set; } = string.Empty; // наименование предприятия Поставщика
        public string ProductTitle { get; set; } = string.Empty; // наименование Продукции

        public Dictionary<string, int>? Сategories { get; set; } // коллекция категорий хранимой продукции

        public DepotItem()
        {
            Сategories = new Dictionary<string, int>();
        }

        public DepotItem(Register register)
        {
            Supplier = register.Supplier;
            ProductTitle = register.ProductTitle;
            Сategories = new Dictionary<string, int>
            {
                { "Кондиционная продукция", register.AccWeightsReg },
                { "Отход", register.WastesReg }
            };
        }

        public DepotItem(params Register[] registers)
        {
            Supplier = registers[0].Supplier;
            ProductTitle = registers[0].ProductTitle;
            Сategories = new Dictionary<string, int>();

            foreach (Register r in registers)
            {
                if (Supplier == r.Supplier && ProductTitle == r.ProductTitle)
                {
                    Сategories.Add("Кондиционная продукция", r.AccWeightsReg);
                    Сategories.Add("Отход", r.WastesReg);
                }
            } 
        }

        public void AddCategory(string categoryTitle)
        {
            try
            {
                if (Сategories!.ContainsKey(categoryTitle))
                    return;

                Сategories.Add($"{categoryTitle}", 0);
            }
            catch (Exception)
            {
                // TODO
                throw;
            }

        }

        public void AddRegister(Register register)
        {
            if(Supplier == register.Supplier && ProductTitle == register.ProductTitle)
            {
                int weight = 0;
                Сategories?.Remove("Кондиционная продукция", out weight);
                Сategories?.Add("Кондиционная продукция", weight + register.AccWeightsReg);

                Сategories?.Remove("Отход", out weight);
                Сategories?.Add("Отход", weight + register.WastesReg);
            }
            else
            {
                DepotItem di = new DepotItem(register);
            }
        }


        public async Task SaveAllInfo(string connString, string databaseName, string tableName, params object[] objects)
        {
            string query = @"INSERT INTO" + $"{tableName}" + "(supplier, productTitle, categories)" +
                                          "VALUES (@supplier, @productTitle, @categories)";

            using SqlConnection conn = new SqlConnection(connString);

            try
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@supplier", objects[0]);
                cmd.Parameters.AddWithValue("@productTitle", objects[1]);
                cmd.Parameters.AddWithValue("@categories", objects[2]);

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








        // вивод на КОНСОЛЬ =================================================================================
        public void PrintDepotItem()
        {
            Console.WriteLine(
                   $"Поставщик:                 {Supplier}\n" +
                   $"Наименование:              {ProductTitle}");

            if(Сategories != null)
                foreach (var c in Сategories)
                    Console.WriteLine($"{c.Key}:      {c.Value}");

            Console.WriteLine();
        }



    }
}
