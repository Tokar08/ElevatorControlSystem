using Microsoft.Data.SqlClient;
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
    public class Price// : ISaveToDb
    {
        public string ProductTitle { get; set; }
        public Dictionary<string, double>? PriceByOperations { get; set; } // коллекция цен на технологические операции
        public string? CreatedBy { get; set; } = string.Empty; // имя пользователя-создателя

        public Price(string productTitle)
        {
            ProductTitle = productTitle;
            PriceByOperations = new Dictionary<string, double>();

            //OperationPrices = new Dictionary<string, double>()
            //    {
            //        { "Приемка", 130.00 },
            //        { "Первичная очистка", 1290.00 },
            //        { "Сушка в шахтной сушилке", 900.00 }
            //    }; 
        }

        
        public void AddOperation(string operationTitle, double operationPrice)
        {
            try
            {
                PriceByOperations?.Add(operationTitle, operationPrice);
            }
            catch (Exception)
            {
                // TODO
                throw;
            }
        }

        public void AddOperationRange(params KeyValuePair<string, double>[] operatoinPrices)
        {
            try
            {
                foreach (KeyValuePair<string, double> op in operatoinPrices)
                    PriceByOperations?.Add(op.Key, op.Value);
            }
            catch (Exception)
            {
                // TODO
                throw;
            }
        }

        public void RemoveOperation(string operationTitle)
        {
            try
            {
                PriceByOperations?.Remove(operationTitle);
            }
            catch (Exception)
            {
                // TODO
                throw;
            }
        }

        public void ChangePriceValues(string operationTitle, double operationPrice)
        {
            if (PriceByOperations == null)
                    return;

            try
            {
                if (PriceByOperations.ContainsKey(operationTitle))
                {
                    PriceByOperations.Remove(operationTitle);
                    PriceByOperations.Add(operationTitle, operationPrice);
                }
            }
            catch (Exception)
            {
                //TODO
                throw;
            }                 
        }

        
        
    
        // вставка данних в две связанние таблици (Прайс содержит словарь Цен на Технологических операции)
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
            string query = @"INSERT INTO " + $"{tableName}" + "(productTitle, createdBy)" +
                                           "VALUES (@productTitle, @createdBy);" +
                            "SELECT scope_identity();";

            SqlCommand cmd = new SqlCommand(query, conn);

            SqlParameter productTitleParam = new SqlParameter("@productTitle", SqlDbType.VarChar, 20)
            {
                Value = ProductTitle
            };
            cmd.Parameters.Add(productTitleParam);

            cmd.Parameters.AddWithValue("@createdBy", CreatedBy);

            return cmd.ExecuteScalar();
        }

        private void insertInSecondTable(SqlConnection conn, string tableName, int lastInsertedFirstTable_id)
        {
            // добавление данних во вторую таблицу - technologicalOperations (из первого запроса получаем id вставленного Акта доработки и вставляем его во вторую таблицу)
            if (PriceByOperations is null)
                return;

            foreach (var op in PriceByOperations)
            {
                string query = @"INSERT INTO " + $"{tableName}" + "(price_id, operationTitle, operationPrice)" +
                                          "VALUES (@price_id, @operationTitle, @operationPrice)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@price_id", lastInsertedFirstTable_id);
                cmd.Parameters.AddWithValue("@operationTitle", op.Key);
                cmd.Parameters.AddWithValue("@operationPrice", op.Value);
                
                cmd.ExecuteNonQuery();
            }
        }


        // тест на КОНСОЛЬ ===============================================================================
        public void PrintPrice()
        {
            if(PriceByOperations == null)
            {
                Console.WriteLine("Прайс не сформирован.");
                return;
            }

            Console.WriteLine($"Наименование:    {ProductTitle}");

            foreach (var op in PriceByOperations) 
                Console.WriteLine($"{op.Key} || {string.Format("{0:F2}", op.Value)} грн.");

            Console.WriteLine("\n");
        }

    }
}
