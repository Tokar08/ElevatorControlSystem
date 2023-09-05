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
    public class DepotItem// : ISaveToDb
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


        // вставка данних в две связанние таблици (Depotitem содержит словарь Категорий продукции)
        public async Task SaveAllInfo(string connString, string databaseName, params string[] tableNames)
        {
            using SqlConnection conn = new SqlConnection(connString);

            try
            {
                conn.Open();

                int lastInsertedFirstTable_id = Convert.ToInt32(insertInFirstTable(conn, tableNames[0]));
                insertInSecondTable(conn, tableNames[1], lastInsertedFirstTable_id);
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
            string query = @"INSERT INTO " + $"{tableName}" + " (supplier, productTitle)" +
                                          "VALUES (@supplier, @productTitle);" +
                            "SELECT scope_identity();";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@supplier", Supplier);
            cmd.Parameters.AddWithValue("@productTitle", ProductTitle);

            return cmd.ExecuteScalar();
        }

        private void insertInSecondTable(SqlConnection conn, string tableName, int lastInsertedFirstTable_id)
        {
            // добавление данних во вторую таблицу - technologicalOperations (из первого запроса получаем id вставленного Акта доработки и вставляем его во вторую таблицу)
            if (Сategories is null)
                return;

            foreach (var cat in Сategories)
            {
                string query = @"INSERT INTO " + $"{tableName}" + "(depotItem_id, categoryTitle, categoryValue)" +
                                          "VALUES (@depotItem_id, @categoryTitle, @categoryValue)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@depotItem_id", lastInsertedFirstTable_id);
                cmd.Parameters.AddWithValue("@categoryTitle", cat.Key);
                cmd.Parameters.AddWithValue("@categoryValue", cat.Value);

                cmd.ExecuteNonQuery();
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
