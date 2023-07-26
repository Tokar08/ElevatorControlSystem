using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GrainElevatorCS
{
    public class Application
    {

        #region Modify_cod

        // накладная (входящие данные)
        public string? Date { get; set; }
        public string? ProductTitle { get; set; }
        public int ProductWeight { get; set; }
        public string? InvNumber { get; set; }
        public string? VenicleRegNumber { get; set; }

        // карточка анализа (входящие данные)
        public double Weediness { get; set; }
        public double Moisture { get; set; }


        // производственная партия
        // (входящие данные)
        public double WeedinessBase { get; set; } = 0;
        public double MoistureBase { get; set; } = 0;
        // (вычисляемые данные)
        public int Waste { get; set; } = 0;
        public int Shrinkage { get; set; } = 0;
        public int AccWeight { get; set; } = 0;

        // реестр
        public List<ProductionBatch>? prodBatches;
        public int AccWeightsReg { get; set; } = 0;
        public int WastesReg { get; set; } = 0;
        public int ShrinkagesReg { get; set; } = 0;

        // DepotItem
        public List<Register>? registers;
        public int AccWeightItem { get; set; }  //общий Зачетный вес(AccWeight) 
        public int WasteItem { get; set; }      //общий Отход(Waste)			
        public int ShrinkageItem { get; set; }	//общая Усушка(Shrinkage)	

        // завод
        public Dictionary<string, DepotItem> depot = new Dictionary<string, DepotItem>();
        public DataBase<InputInvoice> inInvDB = new DataBase<InputInvoice>("InputInvoice");
        public DataBase<LabCard> labDB = new DataBase<LabCard>("LabCard");
        public DataBase<OutputInvoice> outInvDB = new DataBase<OutputInvoice>("OutputInvoice");

       

        public void Execute()
        {
            InputInvoice inInv = new();
            LabCard lc = new();
            ProductionBatch pb = new();
            Register reg = new();


            //Приемка, доработка и передача на склад входящей партии продукции
            // создание Реестра
            reg = new();

            while (true)
            {
                Console.Clear();

                //создание приходной накладной               
                inInv = RequestInvoiceInfo(inInv); // запрос пользовательской информации по приемке

                // проверка соответствия дати и наименования продукции в Накладной текущему Реестру
                if (reg.prodBatches?.Count != 0 && (reg.ProductTitle != inInv.ProductTitle || reg.Date != inInv.Date))
                {
                    Console.WriteLine($"\nДата поступления или Наименование входящей продукции не соответствуют данному Реестру.\n" +
                        $"Можете ввести их в соответствующий Реестр или создать новый Реестр после закрытия текущего.\n");
                }

                // запуск Накладной в работу
                else
                {
                    // добавление прих.Накл. в список накладних
                    Console.Clear();
                    Console.WriteLine($"\nСоздана и добавлена в базу данных\n{inInv}\n");
                    inInvDB?.items?.Add(inInv);

                    //создание Карточки анализа на основе прих.Накладной
                    lc = new(inInv);
                    lc = RequestLabInfo(lc); // запрос пользовательской информации по лабораторному анализу

                    Console.Clear();
                    Console.WriteLine($"\nСоздана и добавлена в базу данных\nЛабораторная карточка анализа:\n{lc}\n");
                    labDB?.items?.Add(lc);

                    // создание Производственной Партии
                    pb = new(lc);
                    pb = RequestBaseQuilityInfo(pb); // запрос пользовательской информации по базовим показателям качества для данного типа продукции
                    CalcResultProduction(pb);     // рассчет результатов доработки продукции
                    reg.prodBatches?.Add(pb);

                    Console.Clear();
                    Console.WriteLine("\nПроизводственная партия сформирована и доработана до базовых показателей качества:\n\n");
                    PrintProductionBatch(pb);

                    //добавление Производственной партии в Реестр

                    AddToRegister(reg, pb);
                    Console.WriteLine($"\nВходящие данные и результаты доработки внесены в Реестр" +
                                      $" Продукции:  {reg.ProductTitle}  за  {reg.Date}\n\n");

                }
                Console.WriteLine($"Для продолжения работы введите:\n" +
                                  $"                                  0 - Продолжить внесение информации в Реестр по данной Дате и Наименованию.\n" +
                                  $"                                  1 - Закрыть Реестр.\n");

                string? stop = Console.ReadLine();
                if (stop == "0")
                    continue;
                else
                {
                    Console.Clear();
                    PrintReg(reg);
                    // добавление Реестра на Склад
                    PushToDepot(reg);

                    Console.WriteLine($"\nРеестр Продукции:{reg.ProductTitle} за {reg.Date} добавлен на Склад.\n");

                    break;
                }
            }

            Console.ReadLine();
            Console.Clear();
            PrintAllReg();

        }
       








        // запрос информации для создания приходной Накладной
        public InputInvoice RequestInvoiceInfo(Invoice inInv)
        {
            Console.WriteLine("\tВведите информацию для оформления приходной накладной на входящую Продукцию: \n");

            while (true)
            {
                try
                {
                    Console.Write("Дата поступления:                             ");
                    inInv.Date = Console.ReadLine();

                    Console.Write("Номер приходной накладной:                    ");
                    inInv.InvNumber = Console.ReadLine();

                    Console.Write("Регистрационний номер транспортного средства: ");
                    inInv.VenicleRegNumber = Console.ReadLine();

                    Console.Write("Наименование поступившей Продукции:           ");
                    inInv.ProductTitle = Console.ReadLine();

                    Console.Write("Физический вес поступившей Продукции (кг):    ");
                    inInv.ProductWeight = Convert.ToInt32(Console.ReadLine());

                    return (InputInvoice)inInv;
                }
                catch (Exception)
                {
                    Console.WriteLine("Ошибка ввода. Введите корректные значения данных.");
                }
            }
        }

        // запрос информации для создания Карточки анализа
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

        // запрос информации по Базовим показателям для Производственной партии
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

        // рассчет результатов доработки Производственной партии
        public void CalcResultProduction(ProductionBatch pb)
        {
            if (pb.Weediness <= pb.WeedinessBase)
                pb.Waste = 0;
            else
                pb.Waste = (int)(pb.ProductWeight * (1 - (100 - pb.Weediness) / (100 - pb.WeedinessBase)));

            if (pb.Moisture <= pb.MoistureBase)
                pb.Shrinkage = 0;
            else
                pb.Shrinkage = (int)((pb.ProductWeight - pb.Waste) * (1 - (100 - pb.Moisture) / (100 - pb.MoistureBase)));

            pb.AccWeight = pb.ProductWeight - pb.Waste - pb.Shrinkage;
        }

        // печать Производственной партии
        public void PrintProductionBatch(ProductionBatch pb)
        {
            if (this != null)
            {
                Console.WriteLine(new string('-', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
                Console.WriteLine("|{0,12}|{1,10}|{2,15}|{3,10}|{4,10}|{5,10}|{6,10}|{7,15}|", pb.Date, pb.InvNumber, pb.ProductWeight, pb.Moisture, pb.Shrinkage, pb.Weediness, pb.Waste, pb.AccWeight);
                Console.WriteLine(new string('-', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));

            }
        }

        // добавление новой Производственной партии в Реестр
        public void AddToRegister(Register reg,  ProductionBatch pb)
        {
            if (pb != null)
            {
                reg.Date = pb.Date;
                reg.ProductTitle = pb.ProductTitle;

                reg.AccWeightsReg += pb.AccWeight;
                reg.WastesReg += pb.Waste;
                reg.ShrinkagesReg += pb.Shrinkage;
            }
        }

        // печать Реестра
        public void PrintReg(Register reg)
        {
            Console.WriteLine($"Реестр\nНаименование:      {reg.ProductTitle}");
            Console.WriteLine(new string('=', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
            Console.WriteLine("|{0,8}|{1,10}|{2,15}|{3,10}|{4,10}|{5,10}|{6,10}|{7,15}|", "Дата прихода", "Номер ТТН", "Физический вес", "Влажность", "Усушка", "Сорность", "Отход", "Зачетный вес");
            Console.WriteLine(new string('=', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));

            if (reg.prodBatches.Count == 0)
                return;
                           
            foreach (var pb in reg.prodBatches)
                    pb.PrintProductionBatch();
            
            Console.WriteLine(new string('=', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
            Console.WriteLine("|{0,12}|{1,10}|{2,15}|{3,10}|{4,10}|{5,10}|{6,10}|{7,15}|", "Итого", " ", " ", " ", reg.ShrinkagesReg, " ", reg.WastesReg, reg.AccWeightsReg);
            Console.WriteLine(new string('=', 12 + 10 + 15 + 10 + 10 + 10 + 10 + 15 + 9));
            Console.WriteLine("\n");
        }

        // добавление Продукции на Склад
        public void PushToDepot(Register register)
        {
            if (depot != null && register.ProductTitle != null)
            {
                if (depot.ContainsKey(register.ProductTitle)) // добавление Реестра в существующий словарь
                    depot[register.ProductTitle].AddRegister(register);
                else
                    depot.Add(register.ProductTitle, new DepotItem(register)); // добавление реестра в новый словарь
            }
        }

        // добавление реестра в Складскую единицу DepotItem
        public void AddRegister(Register register)
        {
            if (registers != null)
            {
                registers.Add(register);

                AccWeightItem += register.AccWeightsReg;
                WasteItem += register.WastesReg;
                ShrinkageItem += register.ShrinkagesReg;
            }
        }

        // Печать всех Реестров поступивших на Склад
        public void PrintAllReg()
        {
            if (depot != null && depot.Count != 0)
                foreach (var depotItem in depot)
                {
                    Console.WriteLine($"Перечень реестров по Наименованию: {depotItem.Key}\n");

                    foreach (var register in depotItem.Value.registers!)
                        register.PrintReg();

                    Console.WriteLine("\n\n");
                }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Склад пуст");
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }

        #endregion




        #region Original_cod
        //public static void Execute()
        //{
        //    Factory factory = new();

        //    // тестовое заполнение пустого Склада 

        //    if (factory.depot.Count == 0)
        //        factory.Init(factory.inInvDB, factory.labDB, factory);

        //    bool exit = true;
        //    while (exit)
        //    {
        //        switch (Menu.ChoiceOperation())
        //        {
        //            case 0: // загрузка последней сохраненной версии Склада из файла
        //                Console.Clear();
        //                factory?.Load();
        //                break;

        //            case 1: //Приемка, доработка и передача на склад входящей партии продукции

        //                // создание Реестра
        //                Register reg = new();

        //                while (true)
        //                {
        //                    Console.Clear();
        //                    //создание приходной накладной
        //                    InputInvoice inInv = new();
        //                    inInv.RequestInvoiceInfo(inInv); // запрос пользовательской информации по приемке

        //                    if (reg.prodBatches?.Count != 0 && (reg.ProductTitle != inInv.ProductTitle || reg.Date != inInv.Date))
        //                    {
        //                        Console.ForegroundColor = ConsoleColor.Red;
        //                        Console.WriteLine($"\nДата поступления или Наименование входящей продукции не соответствуют данному Реестру.\n" +
        //                           $"Можете ввести их в соответствующий Реестр или создать новый Реестр после закрытия текущего.\n");
        //                        Console.ForegroundColor = ConsoleColor.Green;
        //                    }
        //                    else
        //                    {
        //                        Console.Clear();
        //                        Console.WriteLine($"\nСоздана и добавлена в базу данных\n{inInv}\n");
        //                        factory?.inInvDB?.items?.Add(inInv);

        //                        //создание карточки анализа
        //                        LabCard lc = new(inInv);
        //                        lc.RequestLabInfo(lc); // запрос пользовательской информации по лабораторному анализу

        //                        Console.Clear();
        //                        Console.WriteLine($"\nСоздана и добавлена в базу данных\nЛабораторная карточка анализа:\n{lc}\n");
        //                        factory?.labDB?.items?.Add(lc);

        //                        // создание производственной партии
        //                        ProductionBatch pb = new(lc);
        //                        pb.RequestBaseQuilityInfo(pb); // запрос пользовательской информации по базовим показателям качества для данного типа продукции
        //                        pb.CalcResultProduction();     // рассчет результатов доработки продукции

        //                        Console.Clear();
        //                        Console.WriteLine("\nПроизводственная партия сформирована и доработана до базовых показателей качества:\n\n");
        //                        pb.PrintProductionBatch();

        //                        //добавление Производственной партии в Реестр

        //                        reg.AddToRegister(pb);
        //                        Console.WriteLine($"\nВходящие данные и результаты доработки внесены в Реестр" +
        //                                          $" Продукции:  {reg.ProductTitle}  за  {reg.Date}\n\n");

        //                    }
        //                    Console.WriteLine($"Для продолжения работы введите:\n" +
        //                                      $"                                  0 - Продолжить внесение информации в Реестр по данной Дате и Наименованию.\n" +
        //                                      $"                                  1 - Закрыть Реестр.\n");

        //                    string? stop = Console.ReadLine();
        //                    if (stop == "0")
        //                        continue;
        //                    else
        //                    {
        //                        Console.Clear();
        //                        reg.PrintReg();
        //                        // добавление Реестра на Склад
        //                        factory?.PushToDepot(reg);

        //                        Console.ForegroundColor = ConsoleColor.Yellow;
        //                        Console.WriteLine($"\nРеестр Продукции:{reg.ProductTitle} за {reg.Date} добавлен на Склад.\n");
        //                        Console.ForegroundColor = ConsoleColor.Green;
        //                        break;
        //                    }
        //                }

        //                break;

        //            case 2: // Печать всех Реестров по Наименованию
        //                Console.Clear();
        //                Console.Write($"Введите Наименование продукции: ");
        //                string? titlePrint = Console.ReadLine();

        //                factory?.PrintByTitle(titlePrint!);
        //                break;

        //            case 3: // Печать итоговой количественной информации по Наименованию на текущий момент
        //                Console.Clear();
        //                Console.Write($"Введите Наименование продукции: ");
        //                string? titleInfo = Console.ReadLine();

        //                factory?.PrintInfoByTitle(titleInfo!);
        //                break;

        //            case 4: // Отгрузка со склада категорий продукции по Наименованию

        //                Console.Clear();
        //                if (factory?.depot.Count() != 0)
        //                {
        //                    OutputInvoice outInv = new(); // создание расходной накладной
        //                    outInv.RequestInvoiceInfo(outInv); // запрос информации по отгрузке
        //                    Console.Clear();

        //                    bool flag = false;
        //                    factory?.ShipByTitle(outInv, out flag);

        //                    if (flag)
        //                    {
        //                        Console.WriteLine($"\nСоздана и добавлена в базу данных\n{outInv}");
        //                        factory?.outInvDB?.items?.Add(outInv);
        //                    }
        //                }
        //                else
        //                {
        //                    Console.ForegroundColor = ConsoleColor.Red;
        //                    Console.WriteLine("Склад пуст");
        //                    Console.ForegroundColor = ConsoleColor.Green;
        //                }
        //                break;

        //            case 5: // Удаление всей информации по Наименованию
        //                Console.Clear();
        //                Console.Write($"Введите Наименование продукции: ");
        //                string? titleDel = Console.ReadLine();

        //                factory?.RemoveByTitle(titleDel!);
        //                break;

        //            case 6: // Печать всех Реестров поступивших на Склад
        //                Console.Clear();
        //                factory?.PrintAllReg();
        //                break;

        //            case 7: // Печать итоговой количественной информации по всем Наименованиям на текущий момент
        //                Console.Clear();
        //                factory?.PrintAllInfo();
        //                break;

        //            case 8: // Очистка склада
        //                Console.Clear();
        //                factory?.ClearDepot();
        //                break;

        //            case 9: // загрузка последнних сохраненних версий Баз данных первичных документов из файлов
        //                Console.Clear();
        //                if (factory?.LoadDB(factory.labDB!) != null)
        //                    factory.labDB = (DataBase<LabCard>)factory.LoadDB(factory.labDB);

        //                if (factory?.LoadDB(factory.inInvDB!) != null)
        //                    factory.inInvDB = (DataBase<InputInvoice>)factory.LoadDB(factory.inInvDB);

        //                if (factory?.LoadDB(factory.outInvDB!) != null)
        //                    factory.outInvDB = (DataBase<OutputInvoice>)factory.LoadDB(factory.outInvDB);


        //                // factory?.LoadDB(factory.outInvDB);

        //                break;

        //            case 10:
        //                Console.Clear();
        //                factory?.ShowDB();
        //                break;

        //            case 11: // Сохранение информацию по Базам данных первичных документов в файл
        //                Console.Clear();
        //                if (factory?.labDB != null)
        //                    factory?.SaveDB(factory.labDB);
        //                if (factory?.inInvDB != null)
        //                    factory?.SaveDB(factory.inInvDB);
        //                if (factory?.outInvDB != null)
        //                    factory?.SaveDB(factory.outInvDB);
        //                break;

        //            case 12: // Сохранение состояния Склада в файл
        //                Console.Clear();
        //                factory?.SaveReg();
        //                factory?.SaveInfo();
        //                break;

        //            case 13: //Завершение работы
        //                Console.Clear();
        //                exit = false;
        //                Console.ForegroundColor = ConsoleColor.Yellow;
        //                Console.WriteLine($"Работа завершена");
        //                Console.SetCursorPosition(0, 25);
        //                break;
        //        }
        //    }
        //}
        #endregion
    }
}
