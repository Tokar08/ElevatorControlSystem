using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

// Итоговый Реестр. 
// ================
//содержит информацию о принятых Производственных партиях одного Наименования продукции за одни сутки, с результатами доработки. 

namespace GrainElevatorCS
{
    public class Register : ISaveToDb
    {
        public int RegNumber { get; set; } = 0; // номер Реестра
        public DateTime Date { get; set; } = DateTime.MinValue; // дата прихода 
        public string Supplier { get; set; } = string.Empty; // наименование предприятия Поставщика
        public string ProductTitle { get; set; } = string.Empty; // наименование Продукции

        public List<ProductionBatch>? ProuctionBatches { get; set; } // коллекция ППП Реестра

        public int PhysicalWeightReg { get; set; } = 0; // суммарний Физический вес Реестра
        public int AccWeightsReg { get; set; } = 0; // суммарний Зачетный вес Реестра
        public int WastesReg { get; set; } = 0; // суммарная Сорная убыль Реестра
        public int ShrinkagesReg { get; set; } = 0; // суммарная Усушка Реестра
        public string? CreatedBy { get; set; } = string.Empty; // имя пользователя-создателя


        public Register()
        {
            ProuctionBatches = new List<ProductionBatch>();
        }

        public Register(int regNum, List<LabCard>? labCards, double weedinessBase, double moistureBase)
        {
            if (labCards != null)
            {
                ProuctionBatches = new List<ProductionBatch>();

                RegNumber = regNum;
                Date = labCards[0].Date;
                Supplier = labCards[0].Supplier;
                ProductTitle = labCards[0].ProductTitle;

                foreach (var lc in labCards)
                {
                    ProductionBatch pb = new ProductionBatch(lc, weedinessBase, moistureBase);
                    ProuctionBatches.Add( pb );

                    PhysicalWeightReg += pb.PhysicalWeight;
                    AccWeightsReg += pb.AccWeight;
                    WastesReg += pb.Waste;
                    ShrinkagesReg += pb.Shrinkage;
                }
            }
        }

        public Register(int regNum, List<ProductionBatch>? prodBatches)
        {
            if (prodBatches != null)
            {
                ProuctionBatches = new List<ProductionBatch>(prodBatches);

                RegNumber = regNum;
                Date = prodBatches[0].Date;
                Supplier = prodBatches[0].Supplier;
                ProductTitle = prodBatches[0].ProductTitle;

                foreach(var pd in prodBatches)
                {
                    PhysicalWeightReg += pd.PhysicalWeight;
                    AccWeightsReg += pd.AccWeight;
                    WastesReg += pd.Waste;
                    ShrinkagesReg += pd.Shrinkage;
                }
            }
        }

        public Register(int regNum, params ProductionBatch[] prodBatches)
        {
            if (prodBatches != null)
            {
                ProuctionBatches = new List<ProductionBatch>(prodBatches);

                RegNumber = regNum;
                Date = prodBatches[0].Date;
                Supplier = prodBatches[0].Supplier;
                ProductTitle = prodBatches[0].ProductTitle;

                foreach (var pd in prodBatches)
                {
                    PhysicalWeightReg += pd.PhysicalWeight;
                    AccWeightsReg += pd.AccWeight;
                    WastesReg += pd.Waste;
                    ShrinkagesReg += pd.Shrinkage;
                }
            }
        }

        public void AddToRegister(ProductionBatch pb)
        {
            if (pb != null)
            {
                Date = pb.Date;
                ProductTitle = pb.ProductTitle;
                Supplier = pb.Supplier;
                ProuctionBatches?.Add(pb);

                PhysicalWeightReg += pb.PhysicalWeight;
                AccWeightsReg += pb.AccWeight;
                WastesReg += pb.Waste;
                ShrinkagesReg += pb.Shrinkage;
            }
        }




        // вставка данних в две связанние таблици (Акт доработки содержит список Технологических операций)
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
            string query = @"INSERT INTO " + $"{tableName}" + "(registerNumber, arrivalDate, supplier, productTitle, physicalWeightReg, accWeightReg, wasteReg, shrinkageReg, createdBy)" +
                                "VALUES (@registerNumber, @arrivalDate, @supplier, @productTitle, @physicalWeightReg, @accWeightReg, @wasteReg, @shrinkageReg, @createdBy); " +
                                "SELECT scope_identity();";

            SqlCommand cmd = new SqlCommand(query, conn);

            SqlParameter regNumberParam = new SqlParameter("@registerNumber", SqlDbType.Int)
            {
                Value = RegNumber
            };
            cmd.Parameters.Add(regNumberParam);

            cmd.Parameters.AddWithValue("@arrivalDate", Date);
            cmd.Parameters.AddWithValue("@supplier", Supplier);
            cmd.Parameters.AddWithValue("@productTitle", ProductTitle);
            cmd.Parameters.AddWithValue("@physicalWeightReg", PhysicalWeightReg);
            cmd.Parameters.AddWithValue("@accWeightReg", AccWeightsReg);
            cmd.Parameters.AddWithValue("@wasteReg", WastesReg);
            cmd.Parameters.AddWithValue("@shrinkageReg", ShrinkagesReg);
            cmd.Parameters.AddWithValue("@createdBy", CreatedBy);

            return cmd.ExecuteScalar();
        }

        private void insertInSecondTable(SqlConnection conn, string tableName, int lastInsertedFirstTable_id)
        {
            // добавление данних во вторую таблицу - technologicalOperations (из первого запроса получаем id вставленного Акта доработки и вставляем его во вторую таблицу)
            if (ProuctionBatches is null)
                return;

            foreach (var pb in ProuctionBatches)
            {
                string query = @"INSERT INTO " + $"{tableName}" + "(register_id, labCardNumber, invNumber, arrivalDate, supplier, productTitle, physicalWeight, weediness, moisture, weedinessBase, moistureBase, waste, shrinkage, accountWeight)" +
                                          "VALUES (@register_id, @labCardNumber, @invNumber, @arrivalDate, @supplier, @productTitle, @physicalWeight, @weediness, @moisture, @weedinessBase, @moistureBase, @waste, @shrinkage, @accountWeight)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@register_id", lastInsertedFirstTable_id);
                cmd.Parameters.AddWithValue("@labCardNumber", pb.LabCardNumber);
                cmd.Parameters.AddWithValue("@invNumber", pb.InvNumber);
                cmd.Parameters.AddWithValue("@arrivalDate", pb.Date);
                cmd.Parameters.AddWithValue("@supplier", Supplier);
                cmd.Parameters.AddWithValue("@productTitle", ProductTitle);
                cmd.Parameters.AddWithValue("@physicalWeight", pb.PhysicalWeight);
                cmd.Parameters.AddWithValue("@weediness", pb.Weediness);
                cmd.Parameters.AddWithValue("@moisture", pb.Moisture);

                SqlParameter weedinessBaseParam = new SqlParameter("@weedinessBase", SqlDbType.Float)
                {
                    Value = pb.WeedinessBase
                };
                cmd.Parameters.Add(weedinessBaseParam);

                SqlParameter moistureBaseParam = new SqlParameter("@moistureBase", SqlDbType.Float)
                {
                    Value = pb.MoistureBase
                };
                cmd.Parameters.Add(moistureBaseParam);

                cmd.Parameters.AddWithValue("@waste", pb.Waste);
                cmd.Parameters.AddWithValue("@shrinkage", pb.Shrinkage);
                cmd.Parameters.AddWithValue("@accountWeight", pb.AccWeight);

                cmd.ExecuteNonQuery();            
            }
        }









        //public async Task SaveAllInfo(string connString, string databaseName, string tableName, params object[] objects)
        //{
        //    string query = @"INSERT INTO" + $"{tableName}" + "(regNumber, date, supplier, productTitle, prodBatches, physicalWeightReg, accWeightReg, wasteReg, shrinkageReg, createdBy)" +
        //                                  "VALUES (@regNumber, @date, @supplier, @productTitle, @prodBatches, @physicalWeightReg, @weediness, @moisture, @weedinessBase, @moistureBase, @waste, @shrinkage, @accWeight, @createdBy)";

        //    using SqlConnection conn = new SqlConnection(connString);

        //    try
        //    {
        //        await conn.OpenAsync();

        //        SqlCommand cmd = new SqlCommand(query, conn);

        //        SqlParameter regNumberParam = new SqlParameter("@regNumber", SqlDbType.Int)
        //        {
        //            Value = objects[0]
        //        };
        //        cmd.Parameters.Add(regNumberParam);

        //        cmd.Parameters.AddWithValue("@date", objects[1]);
        //        cmd.Parameters.AddWithValue("@supplier", objects[2]);
        //        cmd.Parameters.AddWithValue("@productTitle", objects[3]);
        //        cmd.Parameters.AddWithValue("@prodBatches", objects[4]);
        //        cmd.Parameters.AddWithValue("@physicalWeightReg", objects[5]);
        //        cmd.Parameters.AddWithValue("@accWeightReg", objects[6]);
        //        cmd.Parameters.AddWithValue("@wasteReg", objects[7]);
        //        cmd.Parameters.AddWithValue("@shrinkageReg", objects[8]);
        //        cmd.Parameters.AddWithValue("@createdBy", objects[9]);

        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"ERROR: {ex.Message}");  //  TODO
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //            conn.Close();
        //    }
        //}






        // ТЕСТ ДЛЯ КОНСОЛИ ==============================================================================================================
        public void PrintReg()
        {
            Console.WriteLine($"Реестр:          №{RegNumber}");
            Console.WriteLine($"Поставщик:       {Supplier}");
            Console.WriteLine($"Наименование:    {ProductTitle}");
            Console.WriteLine(new string('=', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
            Console.WriteLine("|{0,8}|{1,10}|{2,15}|{3,10}|{4,10}|{5,10}|{6,10}|{7,15}|","Дата прихода","Номер ТТН", "Физический вес", "Влажность","Усушка","Сорность","Отход","Зачетный вес");
            Console.WriteLine(new string('=', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));

            if (ProuctionBatches != null)
            {
                foreach (var pb in ProuctionBatches)
                    pb.PrintProductionBatch();
            }

            Console.WriteLine(new string('=', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
            Console.WriteLine("|{0,12}|{1,10}|{2,15}|{3,10}|{4,10}|{5,10}|{6,10}|{7,15}|", "Итого", " ", PhysicalWeightReg, " ", ShrinkagesReg, " ", WastesReg, AccWeightsReg);
            Console.WriteLine(new string('=', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
            Console.WriteLine("\n");
        }
    }
}
