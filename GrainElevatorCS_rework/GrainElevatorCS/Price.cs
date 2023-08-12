using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrainElevatorCS
{
    public class Price : ISaveToDb
    {
        public string ProductTitle { get; set; }
        public Dictionary<string, double>? OperationPrices { get; set; } // коллекция цен на технологические операции
        public string? CreatedBy { get; set; } = string.Empty; // имя пользователя-создателя

        public Price(string productTitle)
        {
            ProductTitle = productTitle;
            OperationPrices = new Dictionary<string, double>();

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
                OperationPrices?.Add(operationTitle, operationPrice);
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
                    OperationPrices?.Add(op.Key, op.Value);
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
                OperationPrices?.Remove(operationTitle);
            }
            catch (Exception)
            {
                // TODO
                throw;
            }
        }

        public void ChangePriceValues(string operationTitle, double operationPrice)
        {
            if (OperationPrices == null)
                    return;

            try
            {
                if (OperationPrices.ContainsKey(operationTitle))
                {
                    OperationPrices.Remove(operationTitle);
                    OperationPrices.Add(operationTitle, operationPrice);
                }
            }
            catch (Exception)
            {
                //TODO
                throw;
            }                 
        }

        public async Task SaveAllInfo(string connString, string databaseName, string tableName, params object[] objects)
        {
            string query = @"INSERT INTO" + $"{tableName}" + "(productTitle, operationPrices, createdBy)" +
                                          "VALUES (@productTitle, @operationPrices, @createdBy)";

            using SqlConnection conn = new SqlConnection(connString);

            try
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlParameter productTitleParam = new SqlParameter("@productTitle", SqlDbType.VarChar, 20)
                {
                    Value = objects[0]
                };
                cmd.Parameters.Add(productTitleParam);

                cmd.Parameters.AddWithValue("@operationPrices", objects[1]);
                cmd.Parameters.AddWithValue("@createdBy", objects[2]);

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


        // тест на КОНСОЛЬ ===============================================================================
        public void PrintPrice()
        {
            if(OperationPrices == null)
            {
                Console.WriteLine("Прайс не сформирован.");
                return;
            }

            Console.WriteLine($"Наименование:    {ProductTitle}");

            foreach (var op in OperationPrices) 
                Console.WriteLine($"{op.Key} || {string.Format("{0:F2}", op.Value)} грн.");

            Console.WriteLine("\n");
        }

    }
}
