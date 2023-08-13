using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GrainElevatorCS
{
    // Расходная накладная.
    // ===============================
    // Содержит информацию об отгружаемой продукции продукции.

    public class OutputInvoice : ISaveToDb
    {
        public string OutInvNumber { get; set; } = string.Empty; // номер накладной
        public DateTime Date { get; set; } = DateTime.Now;  // дата отгрузки
        public string VenicleNumber { get; set; } = string.Empty; // гос.номер транспортного средства
        public string Supplier { get; set; } = string.Empty; // наименование предприятия Поставщика
        public string ProductTitle { get; set; } = string.Empty; // наименование Продукции
        public string Category { get; set; } = string.Empty; // Категория Продукции
        public int Weight { get; set; } = 0; //  вес Продукции
        public string? CreatedBy { get; set; } = string.Empty; // имя пользователя-создателя


        public OutputInvoice() { }

        public OutputInvoice(string outInvNumber, DateTime date, string venicleNumber, string supplier, string productTitle, string category, int productWeight)
        {
            OutInvNumber = outInvNumber;
            Date = date;
            VenicleNumber = venicleNumber;
            Supplier = supplier;
            ProductTitle = productTitle;
            Category = category;
            Weight = productWeight;
        }

        public OutputInvoice(string outInvNumber, DateTime date, string venicleNumber, DepotItem depotItem, string category, int productWeight)
        {
            OutInvNumber = outInvNumber;
            Date = date;
            VenicleNumber = venicleNumber;
            Supplier = depotItem.Supplier;
            ProductTitle = depotItem.ProductTitle;
            Category = category;
            Weight = productWeight;
        }

        public async Task SaveAllInfo(string connString, string databaseName, string tableName, params object[] objects)
        {
            string query = @"INSERT INTO" + $"{tableName}" + "(outInvNumber, date, venicleNumber, supplier, productTitle, category, productWeight, createdBy)" +
                                          "VALUES (@outInvNumber, @date, @venicleNumber , @supplier, @productTitle, @category, @productWeight, @createdBy)";

            using SqlConnection conn = new SqlConnection(connString);

            try
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlParameter outInvNumberParam = new SqlParameter("@outInvNumber", SqlDbType.VarChar, 10)
                {
                    Value = objects[0]
                };
                cmd.Parameters.Add(outInvNumberParam);

                SqlParameter dateParam = new SqlParameter("@date", SqlDbType.Date)
                {
                    Value = objects[1]
                };

                SqlParameter venicleNumberParam = new SqlParameter("@venicleNumber", SqlDbType.VarChar, 10)
                {
                    Value = objects[2]
                };
                cmd.Parameters.Add(venicleNumberParam);

                cmd.Parameters.AddWithValue("@supplier", objects[3]);
                cmd.Parameters.AddWithValue("@productTitle", objects[4]);

                SqlParameter categoryParam = new SqlParameter("@category", SqlDbType.VarChar, 30)
                {
                    Value = objects[5]
                };
                cmd.Parameters.Add(categoryParam);

                SqlParameter productWeightParam = new SqlParameter("@productWeight", SqlDbType.Int)
                {
                    Value = objects[6]
                };
                cmd.Parameters.Add(productWeightParam);

                cmd.Parameters.AddWithValue("@createdBy", objects[7]);

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





        // ввод информации (ДЛЯ ТЕСТА НА КОНСОЛИ) =====================================================================
        public OutputInvoice RequestInvoiceInfo(OutputInvoice outInv)
        {
            Console.WriteLine("Введите информацию для оформления расходной накладной на отгружаемую Продукцию:\n" + 
                              "--------------------------------------------------------------------------------\n");

            while (true)
            {
                try
                {
                    outInv.Date = DateTime.Now;

                    Console.Write("Номер расходной накладной:                           ");
                    outInv.OutInvNumber = Console.ReadLine();

                    Console.Write("Регистрационний номер транспортного средства:        ");
                    outInv.VenicleNumber = Console.ReadLine();

                    Console.Write("Поставщик Продукции:                                 ");
                    outInv.Supplier = Console.ReadLine();

                    Console.Write("Наименование отгружаемой Продукции:                  ");
                    outInv.ProductTitle = Console.ReadLine();

                    Console.Write("Категория отгружаемой Продукции:                     ");
                    outInv.Category = Console.ReadLine();
    
                    Console.Write("Вес отгружаемой Продукции (кг):                      ");
                    outInv.Weight = Convert.ToInt32(Console.ReadLine());

                    return outInv;
                }
                catch (Exception)
                {
                    Console.WriteLine("Ошибка ввода. Введите корректные значения данных.");  // TODO
                }
            }
        }

        // вивод на консоль
        public override string ToString()
        {
            return $"\nРасходная накладная №{OutInvNumber}.\n" +
                   $"---------------------------\n" +
                   $"Дата отгрузки:             {Date.ToString("dd.MM.yyyy")}\n" +
                   $"Номер ТС:                  {VenicleNumber}\n"+
                   $"Поставщик:                 {Supplier}\n" +
                   $"Наименование продукции:    {ProductTitle}\n" +
                   $"Категория продукции:       {Category}\n" +
                   $"Вес нетто:                 {Weight} кг\n\n";
        }
    }
}
