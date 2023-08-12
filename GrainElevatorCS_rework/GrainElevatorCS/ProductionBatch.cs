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

// Промежуточная Производственная партия(ППП).
// ========================
// содержит первичную информацию о партии Продукции (Приходная накладная),
// информацию лаборатории: входящие качественные показатели (Карточка анализа),
// запрашивает значения качественных показателей, которые необходимо достигнуть в процессе доработки (Производство),
// рассчитывает результаты доработки (изменение качественно-количественных показателей)


namespace GrainElevatorCS
{
    public class ProductionBatch : ISaveToDb
    {
        // входящие данные (из LabCard)
        public DateTime Date { get; set; } = DateTime.Now; // дата прихода 
        public int LabCardNumber { get; set; } = 0; // номер Карточки анализа
        public string InvNumber { get; set; } = string.Empty; // номер входящей накладной
        public string Supplier { get; set; } = string.Empty; // наименование предприятия Поставщика
        public string ProductTitle { get; set; } = string.Empty; // наименование Продукции
        public int PhysicalWeight { get; set; } = 0; // Физический вес Продукции
        public double Weediness { get; set; } = 0; // Сорная примесь Продукции
        public double Moisture { get; set; } = 0;  // Влажность Продукции

        // данные, вносимые при Производстве 
        public double WeedinessBase { get; set; } = 0; // Базовая Сорная примесь для данного типа Продукции
        public double MoistureBase { get; set; } = 0;  // Базовая Влажность для данного типа Продукции

        // вычисляемые данные
        public int Waste { get; set; } = 0; // Сорная убыль (Отход)
        public int Shrinkage { get; set; } = 0; // Усушка
        public int AccWeight { get; set; } = 0;  // Зачетный вес 



        public ProductionBatch() { }

        public ProductionBatch( int labCardNumber,
                                DateTime date,
                                string invNumber, 
                                string supplier, 
                                string productTitle, 
                                int physicalWeight, 
                                double weediness, 
                                double moisture, 
                                double weedinessBase, 
                                double moistureBase)
        {
            LabCardNumber = labCardNumber;
            Date = date;
            InvNumber = invNumber;
            Supplier = supplier;
            ProductTitle = productTitle;
            PhysicalWeight = physicalWeight;
            Weediness = weediness;
            Moisture = moisture;

            WeedinessBase = weedinessBase;
            MoistureBase = moistureBase;

            CalcResultProduction();
        }

        public ProductionBatch(LabCard lc, double weedinessBase, double moistureBase)
        {
            LabCardNumber = lc.LabCardNumber;
            Date = lc.Date;
            InvNumber = lc.InvNumber;
            Supplier = lc.Supplier;
            ProductTitle = lc.ProductTitle;
            PhysicalWeight = lc.PhysicalWeight;
            Weediness = lc.Weediness;
            Moisture = lc.Moisture;

            WeedinessBase = weedinessBase;
            MoistureBase = moistureBase;

            CalcResultProduction();
        }

        public void CalcResultProduction()
        {
            if (Weediness <= WeedinessBase)
                Waste = 0;
            else
                Waste = (int)(PhysicalWeight * (1 - (100 - Weediness) / (100 - WeedinessBase)));

            if (Moisture <= MoistureBase)
                Shrinkage = 0;
            else
                Shrinkage = (int)((PhysicalWeight - Waste) * (1 - (100 - Moisture) / (100 - MoistureBase)));

            AccWeight = PhysicalWeight - Waste - Shrinkage;
        }

        public async Task SaveAllInfo(string connString, string databaseName, string tableName, params object[] objects)
        {
            string query = @"INSERT INTO" + $"{tableName}" + "(labCardNumber, invNumber, date, supplier, productTitle, physicalWeight, weediness, moisture, weedinessBase, moistureBase, waste, shrinkage, accWeight)" +
                                          "VALUES (@labCardNumber, @invNumber, @date, @supplier, @productTitle, @physicalWeight, @weediness, @moisture, @weedinessBase, @moistureBase, @waste, @shrinkage, @accWeight)";

            using SqlConnection conn = new SqlConnection(connString);

            try
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@labCardNumber", objects[0]);
                cmd.Parameters.AddWithValue("@invNumber", objects[1]);
                cmd.Parameters.AddWithValue("@date", objects[2]);
                cmd.Parameters.AddWithValue("@supplier", objects[3]);
                cmd.Parameters.AddWithValue("@productTitle", objects[4]);
                cmd.Parameters.AddWithValue("@physicalWeight", objects[5]);
                cmd.Parameters.AddWithValue("@weediness", objects[6]);
                cmd.Parameters.AddWithValue("@moisture", objects[7]);

                SqlParameter weedinessBaseParam = new SqlParameter("@weedinessBase", SqlDbType.Float)
                {
                    Value = objects[8]
                };
                cmd.Parameters.Add(weedinessBaseParam);

                SqlParameter moistureBaseParam = new SqlParameter("@moistureBase", SqlDbType.Float)
                {
                    Value = objects[9]
                };
                cmd.Parameters.Add(moistureBaseParam);

                cmd.Parameters.AddWithValue("@waste", objects[10]);
                cmd.Parameters.AddWithValue("@shrinkage", objects[11]);
                cmd.Parameters.AddWithValue("@accWeight", objects[12]);


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









        // запрос базових значений качества (ДЛЯ ТЕСТА НА КОНСОЛИ)
        public ProductionBatch RequestBaseQuilityInfo(ProductionBatch pb)
        {
            Console.WriteLine("\tВведите базовые показатели качества для входящей Продукции: \n");
            while (true)
            {
                try
                {
                    Console.Write("Базовая Сорная примесь (%):    ");
                    pb.WeedinessBase = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Базовая Влажность (%):         ");
                    pb.MoistureBase = Convert.ToInt32(Console.ReadLine());

                    if (pb.Weediness < 0 || pb.WeedinessBase > 100 || pb.MoistureBase < 0 || pb.MoistureBase > 100) // провеока полученних данних на диапазон 0-100%
                        throw new Exception();

                    return pb;
                }
                catch (Exception)
                {
                    Console.WriteLine("Ошибка ввода. Введите корректные значения данных в диапазоне от 0 до 100");
                }
            }
        }


        // вивод информации (ДЛЯ ТЕСТА НА КОНСОЛИ)
        public override string ToString()
        {
            return $"Дата прихода:              {Date.ToString("dd.MM.yyyy")}\n" +
                   $"Номер Карточки анализа:    {LabCardNumber}\n" +
                   $"Поставщик:                 {Supplier}\n" +
                   $"Наименование:              {ProductTitle}\n" +
                   $"физический вес:            {PhysicalWeight} кг\n" +
                   
                   $"Сорная примесь:    {Weediness} %\n" +
                   $"Влажность:         {Moisture} %\n\n" +

                   $"Базовая сорность:  {WeedinessBase} %\n" +
                   $"Базовая Влажность: {MoistureBase} %\n\n" +

                   $"Усушка:            {Shrinkage} кг\n" +
                   $"Сорная убыль:      {Waste} кг\n" +
                   $"Зачетный вес:      {AccWeight} кг\n";
        }

        public void PrintProductionBatch()
        {
            if (this != null)
            {
                Console.WriteLine(new string('-', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
                Console.WriteLine("|{0,12}|{1,10}|{2,15}|{3,10}|{4,10}|{5,10}|{6,10}|{7,15}|", Date.ToString("dd.MM.yyyy"), InvNumber, PhysicalWeight, Moisture, Shrinkage, Weediness, Waste, AccWeight);
                Console.WriteLine(new string('-', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
            }
        }
    }
}
