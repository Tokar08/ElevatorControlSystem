using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GrainElevatorCS
{
    // Расходная накладная.
    // ===============================
    // Содержит информацию об отгружаемой продукции продукции.

    public class OutputInvoice// : ISaveToDb
    {
        public string OutInvNumber { get; set; } = string.Empty; // номер накладной
        public DateTime Date { get; set; } = DateTime.Now;  // дата отгрузки
        public string VehicleNumber { get; set; } = string.Empty; // гос.номер транспортного средства
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
            VehicleNumber = venicleNumber;
            Supplier = supplier;
            ProductTitle = productTitle;
            Category = category;
            Weight = productWeight;
        }

        public OutputInvoice(string outInvNumber, DateTime date, string vehicleNumber, DepotItem depotItem, string category, int productWeight)
        {
            OutInvNumber = outInvNumber;
            Date = date;
            VehicleNumber = vehicleNumber;
            Supplier = depotItem.Supplier;
            ProductTitle = depotItem.ProductTitle;
            Category = category;
            Weight = productWeight;
        }

        public async Task SaveAllInfo(string connString, string databaseName, params string[] tableNames)
        {
            string query = @"INSERT INTO " + $"{tableNames[0]}" + "(outInvNumber, shipmentDate, vehicleNumber, supplier, productTitle, category, productWeight, createdBy)" +
                                         "VALUES (@outInvNumber, @shipmentDate, @vehicleNumber , @supplier, @productTitle, @category, @productWeight, @createdBy)";

            using SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlParameter outInvNumberParam = new SqlParameter("@outInvNumber", SqlDbType.VarChar, 10)
                {
                    Value = OutInvNumber
                };
                cmd.Parameters.Add(outInvNumberParam);

                SqlParameter dateParam = new SqlParameter("@shipmentDate", SqlDbType.Date)
                {
                    Value = Date
                };
                cmd.Parameters.Add(dateParam);

                SqlParameter venicleNumberParam = new SqlParameter("@vehicleNumber", SqlDbType.VarChar, 10)
                {
                    Value = VehicleNumber
                };
                cmd.Parameters.Add(venicleNumberParam);

                cmd.Parameters.AddWithValue("@supplier", Supplier);
                cmd.Parameters.AddWithValue("@productTitle", ProductTitle);

                SqlParameter categoryParam = new SqlParameter("@category", SqlDbType.VarChar, 30)
                {
                    Value = Category
                };
                cmd.Parameters.Add(categoryParam);

                SqlParameter productWeightParam = new SqlParameter("@productWeight", SqlDbType.Int)
                {
                    Value = Weight
                };
                cmd.Parameters.Add(productWeightParam);

                cmd.Parameters.AddWithValue("@createdBy", CreatedBy);

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
                    outInv.VehicleNumber = Console.ReadLine();

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
                   $"Номер ТС:                  {VehicleNumber}\n"+
                   $"Поставщик:                 {Supplier}\n" +
                   $"Наименование продукции:    {ProductTitle}\n" +
                   $"Категория продукции:       {Category}\n" +
                   $"Вес нетто:                 {Weight} кг\n\n";
        }
    }
}
