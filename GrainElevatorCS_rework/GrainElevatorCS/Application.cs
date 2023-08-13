using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GrainElevatorCS
{
    public class Application
    {
        #region 2_Modify

        //public List<InputInvoice> inputInvDB = new List<InputInvoice>();
        //public List<LabCard> labCardDB = new List<LabCard>();

        //public List<ProductionBatch> prodBatDB = new List<ProductionBatch>();
        //public List<Register> registerDB = new List<Register>();

        //public List<DepotItem> depotItemDB = new List<DepotItem>();
        //public List<Price> priceDB = new List<Price>();
        //public List<CompletionReport> complReportDB = new List<CompletionReport>();
        //public List<OutputInvoice> outInvDB = new List<OutputInvoice>();

        //// ЛАБОРАТОРИЯ ======================================================================================

        //// Создание ВХОДЯЩЕЙ НАКЛАДНОЙ
        //// данние из текст-боксов:  string invNumber = txbInvNum.Text и т.д.
        //public void CreateInputInvoice(params object[] objects)
        //{
        //    string? invNumber = (string)objects[0];
        //    DateTime date = (DateTime)objects[1];
        //    string venicleRegNumber = (string)objects[2];
        //    string supplier = (string)objects[3];
        //    string productTitle = (string)objects[4];
        //    int physicalWeight = (int)objects[5];


        //    InputInvoice inInv = new InputInvoice(invNumber, date, venicleRegNumber, supplier, productTitle, physicalWeight);

        //    inputInvDB.Add(inInv); // TODO SaveToDb
        //}

        //// Создание КАРТОЧКИ АНАЛИЗА
        //// из контрола вибирается накладная и добавляется информация из тект-боксов
        //public void CreateLabCard(InputInvoice inv, int labCardNumber, double weediness, double moisture)
        //{
        //    LabCard lc = new LabCard(inv, labCardNumber, weediness, moisture);

        //    labCardDB.Add(lc); // TODO SaveToDb
        //}

        //// ПРОИЗВОДСТВО =======================================================================================

        //// Создание РЕЕСТРА
        //// чекается список карточек (в контроле), обязательно фильтровать по трем полям (три текс-бокса для условий фильтрации): date, supplier, productTitle
        //// добавляется информация по базовим показателям (из двух текст-боксов)
        //// создается Реестр (ППП создаются "под капотом")

        //public List<ProductionBatch> CreateProductionBatch(List<LabCard> labCards, double weedinessBase, double moistureBase)
        //{
        //    List<ProductionBatch> productionBatches = new List<ProductionBatch>();
        //    labCards.ForEach(labCard =>
        //    {
        //        ProductionBatch pb = new ProductionBatch(labCard, weedinessBase, moistureBase);
        //        productionBatches.Add(pb);

        //        prodBatDB.Add(pb); // TODO SaveToDb 
        //    });
        //    return productionBatches;
        //}
        //public Register CreateRegister(int regNum, List<ProductionBatch> productionBatches)
        //{
        //    Register reg = new Register(regNum, productionBatches);

        //    registerDB.Add(reg); // TODO SaveToDb

        //    return reg;
        //}


        //// Добавление Реестра на Склад (автоматически после создания Реестра)
        //public void PushRegisterToDepot(Register reg)
        //{
        //    if (depotItemDB.Count == 0)
        //    {
        //        depotItemDB.Add(new DepotItem(reg));  // если не существует, то создается новий
        //        return;
        //    }

        //    bool isAdded = false;
        //    depotItemDB.ForEach(di =>
        //    {
        //        if (di.Supplier == reg.Supplier && di.ProductTitle == reg.ProductTitle) // если такой DepotItem существует, то добавлен Register
        //        {
        //            di.AddRegister(reg);
        //            isAdded = true;
        //            return;
        //        }
        //    });

        //    if (!isAdded)
        //        depotItemDB.Add(new DepotItem(reg));  // если не добавлен ни в один DepotItem, то создается новий
        //}


        //// Создание АКТА ДОРАБОТКИ
        //// чекается список Реестров (в контроле),
        //// обязательно фильтровать по двум полям: supplier,productTitle (два текс-бокса для условий фильтрации)
        //// два текст-бокса для добавляемой информации

        //public void CreateCompletionReport(int reportNum, DateTime date, List<Register> registers)
        //{
        //    CompletionReport report = new CompletionReport(reportNum, date, registers.ToArray());

        //    complReportDB.Add(report); // TODO SaveToDb
        //}

        //// БУХГАЛТЕРИЯ ==============================================================================

        //// Работа с Прайсом
        //// 1. Создание ПРАЙСА
        ////    Текст-бокс : Наименование продукции для которой создается Прайс
        ////    Button : создать Прайс

        //public void CreatePrice(string productTitle)
        //{
        //    Price price = new Price(productTitle);

        //}

        //// 2. Добавить/удалить пару ( Техн.операция - Цена )
        ////      два Текст-бокс и Button для Добавления
        ////      Контрол для просмотра Прайса (списка операций) и Button для Удаления

        //public void FillPrice(Price price, string operationTitle, double operationPrice)
        //{
        //    price.AddOperation(operationTitle, operationPrice);
        //}

        //public void RemoveOperation(Price price, string operationTitle)
        //{
        //    price.RemoveOperation(operationTitle);
        //}

        //// Расчет АКТА ДОРАБОТКИ

        //public void CalcReportByPrice(CompletionReport report, Price price)
        //{
        //    report.CalcByPrice(price);
        //}


        //// ОТГРУЗКА Продукции:

        //// Создание РАСХОДНОЙ НАКЛАДНОЙ
        //// данние из текст-боксов:  string invNumber = txbInvNum.Text и т.д.
        //public void CreateOutputInvoice(params object[] objects)
        //{
        //    DateTime date = (DateTime)objects[0];
        //    string? invNumber = (string)objects[1];
        //    string venicleRegNumber = (string)objects[2];
        //    string supplier = (string)objects[3];
        //    string productTitle = (string)objects[4];
        //    string category = (string)objects[5];
        //    int physicalWeight = (int)objects[6];


        //    OutputInvoice outInv = new OutputInvoice(date, invNumber, venicleRegNumber, supplier, productTitle, category, physicalWeight);

        //    outInvDB.Add(outInv); // TODO SaveToDb
        //}

        //public void MakeShipment(OutputInvoice outInv)
        //{
        //    // уменьшение веса соответствующей категории хранимой в Depotitem продукции на величину веса в Расходной накладной)
        //    foreach (var di in depotItemDB)
        //    {
        //        if (di.Сategories != null && outInv.ProductTitle == di.ProductTitle && di.Сategories.ContainsKey(outInv.Category))
        //        {
        //            int weight;
        //            di.Сategories.Remove(outInv.Category, out weight);
        //            di.Сategories.Add(outInv.Category, weight - outInv.Weight);
        //        }
        //    }
        //}

        #endregion




        #region CONSOLE_APP_TEST

        public List<InputInvoice> inputInvDB = new List<InputInvoice>();
        public List<LabCard> labCardDB = new List<LabCard>();
        public List<ProductionBatch> prodBatDB = new List<ProductionBatch>();
        public List<Register> registerDB = new List<Register>();
        public List<DepotItem> depotItemDB = new List<DepotItem>();

        public List<Price> priceDB = new List<Price>();
        public List<CompletionReport> complReportDB = new List<CompletionReport>();

        public List<OutputInvoice> outInvDB = new List<OutputInvoice>();

        public void Execute()
        {
            FirstInit();

            /*
            InputInvoice inInv;
            LabCard lc;
            ProductionBatch pb;
            Register reg;

            //Приемка, доработка и передача на склад входящей партии продукции
            // создание Реестра
            reg = new();

                        while (true)
                        {
                            Console.Clear();

                            //создание приходной накладной
                            inInv = new();                           // сейчас в консоли: создание конструктором без параметров
                            inInv = inInv.RequestInvoiceInfo(inInv); // сейчас в консоли: запрос пользовательской информации по приемке

                            // будет в WPF: создание через конструктор с параметрами с извлечением информации из textBox окна Лаборатории

                            // проверка соответствия дати и наименования продукции в Накладной текущему Реестру
                            if (reg.prodBatches?.Count != 0 && (reg.ProductTitle != inInv.ProductTitle || reg.Date != inInv.Date))
                            {
                                Console.WriteLine($"\nДата поступления или Наименование входящей продукции не соответствуют данному Реестру.\n" +
                                    $"Можете ввести их в соответствующий Реестр или создать новый Реестр после закрытия текущего.\n");
                            }

                            // запуск Накладной в работу
                            else
                            {
                                // создание приходной Накладной с добавлением в соответствующий list(DB)
                                Console.Clear();
                                Console.WriteLine($"\nСоздана и добавлена в базу данных\n{inInv}\n");

                                inputInvDB?.Add(inInv);


                                //создание Карточки анализа на основе приходной Накладной с добавлением в соответствующий list(DB)

                                lc = new(inInv, 37, 0, 0);  // будет в WPF: извлечение информации (сорность, влажность) из textBox окна Лаборатории

                                lc = lc.RequestLabInfo(lc); // сейчас в консоли: запрос пользовательской информации по лабораторному анализу                                
                                Console.Clear();
                                Console.WriteLine($"\nСоздана и добавлена в базу данных\nЛабораторная карточка анализа:\n{lc}\n");

                                labCardDB?.Add(lc);         // будет в WPF: добавление в список Карточек анализа




                                // создание Производственной Партии с добавлением в соответствующий list(DB)
                                pb = new(lc, 0, 0);                 // будет в WPF: извлечение информации (сорность, влажность) из textBox окна Производство и рассчет в конструкторе

                                pb = pb.RequestBaseQuilityInfo(pb); // сейчас в консоли: запрос пользовательской информации по базовим показателям качества для данного типа продукции
                                pb.CalcResultProduction();          // сейчас в консоли: рассчет результатов доработки продукции
                                reg.AddToRegister(pb);              // сейчас в консоли: добавление ППП в Реестр

                                prodBatDB?.Add(pb);                 // будет в WPF: добавление в список ППП

                                // консоль
                                Console.Clear();
                                Console.WriteLine("\nПроизводственная партия сформирована и доработана до базовых показателей качества:\n\n");
                                pb.PrintProductionBatch();

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
                                reg.PrintReg();

                                registerDB.Add(reg);  // будет в WPF: добавление в список Реестров 

                                Console.WriteLine($"\nРеестр Продукции:{reg.ProductTitle} за {reg.Date.ToString("dd.MM.yyyy")} " + "добавлен на Склад.\n");

                                break;
                            }
                        }

*/



            // Показать все Реестри
            Console.Clear();
            Console.WriteLine("Показать все раннее созданние Реестри:");
            Console.ReadLine();

            registerDB.ForEach(reg => reg.PrintReg());

            // Передача всех Реестров на склад с распределением по DepotItem
            Console.ReadLine();
            Console.WriteLine("Передача всех Реестров на Склад с распределением по DepotItem\n");

            PushRegistersToDepot();

            // Показать все DepotItem
            Console.ReadLine();
            Console.WriteLine("Показать все DepotItem:\n");
            Console.ReadLine();

            Console.WriteLine("Склад:");
            depotItemDB.ForEach(di => di.PrintDepotItem());

            // создание Прайса
            Console.ReadLine();
            Console.WriteLine("Создание Прайса\n");

            Price price1 = new Price("пшеница");
            price1.AddOperation("Приемка", 90.00);
            price1.AddOperation("Первичная очистка", 780.00);
            price1.AddOperation("Сушка в шахтной сушилке", 450.00);

            priceDB.Add(price1);
            price1.PrintPrice();

            // Изменение Прайса
            price1.ChangePriceValues("Приемка", 100); // изменение цени технологической операции в Прайсе
            price1.ChangePriceValues("Первичная очистка", 590);
            price1.ChangePriceValues("Сушка в шахтной сушилке", 400);

            Console.WriteLine($"Прайс изменен");
            price1.PrintPrice();


            Price price2 = new Price("рапс");
            price2.AddOperation("Приемка", 130.00);
            price2.AddOperation("Первичная очистка", 1290.00);
            price2.AddOperation("Сушка в шахтной сушилке", 900.00);

            priceDB.Add(price2);


            // Показать все Прайси
            Console.WriteLine("Показать все Прайси:");
            priceDB.ForEach(p => p.PrintPrice());


            // создание Акта доработки
            Console.ReadLine();
            Console.WriteLine("Создание Акта доработки c расчетом стоимости работ по Прайсу\n");
            Console.ReadLine();


            List<Register> list = new List<Register>() { registerDB[0] };
            CompletionReport cr1 = new CompletionReport(101, DateTime.Now, list);
            cr1.CalcByPrice(price1);
            complReportDB.Add(cr1);

            list = new List<Register>() { registerDB[3] };
            CompletionReport cr2 = new CompletionReport(102, DateTime.Now, list);
            cr2.CalcByPrice(price2);
            complReportDB.Add(cr2);

            // Показать все Акти доработки
            Console.WriteLine("Показать все Акти доработки:");
            complReportDB.ForEach(rp => rp.PrintReport());

            // отгрузка
            MakeShipment();
            Console.WriteLine("Склад:");
            depotItemDB.ForEach(di => di.PrintDepotItem());



        }

        // отгрузка продукции
        public void MakeShipment()
        {
            //создание расходной накладной
            OutputInvoice outInv = new();                           // сейчас в консоли: создание конструктором без параметров
            outInv = outInv.RequestInvoiceInfo(outInv); // сейчас в консоли: запрос пользовательской информации по приемке
                                                        // будет в WPF: создание через конструктор с параметрами с извлечением информации из textBox окна Бухгалтерии

            outInvDB.Add(outInv);
            Console.WriteLine(outInv);

            // уменьшение веса соответствующей категории хранимой в Depotitem продукции на величину веса в Расходной накладной)
            foreach (var di in depotItemDB)
            {
                if (di.Сategories != null && outInv.ProductTitle == di.ProductTitle && di.Сategories.ContainsKey(outInv.Category))
                {
                    int weight;
                    di.Сategories.Remove(outInv.Category, out weight);
                    di.Сategories.Add(outInv.Category, weight - outInv.Weight);

                }

            }
        }

        // передача Реестров на Склад
        public void PushRegistersToDepot()
        {
            foreach (var reg in registerDB)
            {
                if (depotItemDB.Count == 0)
                {
                    depotItemDB.Add(new DepotItem(reg));  // если не существует, то создается новий
                    continue;
                }

                bool isAdded = false;
                depotItemDB.ForEach(di =>
                {
                    if (di.Supplier == reg.Supplier && di.ProductTitle == reg.ProductTitle) // если такой DepotItem существует, то добавлен Register
                    {
                        di.AddRegister(reg);
                        isAdded = true;
                    }
                });

                if (!isAdded)
                    depotItemDB.Add(new DepotItem(reg));  // если не добавлен ни в один DepotItem, то создается новий
            };
        }

        // первоначальное тестовое заполнение
        public void FirstInit()
        {
            InputInvoice invoice1 = new("205365", DateTime.Today, "AA1111BH", "Хлебодар", "пшеница", 35570);
            InputInvoice invoice2 = new("205365", DateTime.Today, "AА2222BH", "Хлебодар", "кукуруза", 30750);
            InputInvoice invoice3 = new("205365", DateTime.Today, "AA3333BH", "Хлебодар", "ячмень", 27250);
            InputInvoice invoice4 = new("205365", DateTime.Today, "АА4444ВН", "Хлебодар", "рапс", 28380);
            InputInvoice invoice5 = new("205365", DateTime.Today, "AA5555BH", "Хлебодар", "пшеница", 25350);
            InputInvoice invoice6 = new("205365", DateTime.Today, "AА6666BH", "Хлебодар", "кукуруза", 33570);
            InputInvoice invoice7 = new("205365", DateTime.Today, "AA7777BH", "Хлебодар", "ячмень", 28750);
            InputInvoice invoice8 = new("205365", DateTime.Today, "АА8888ВН", "Хлебодар", "рапс", 25250);

            inputInvDB?.Add(invoice1);
            inputInvDB?.Add(invoice2);
            inputInvDB?.Add(invoice3);
            inputInvDB?.Add(invoice4);
            inputInvDB?.Add(invoice5);
            inputInvDB?.Add(invoice6);
            inputInvDB?.Add(invoice7);
            inputInvDB?.Add(invoice8);

            LabCard labCard1 = new(invoice1, 1, 25.2, 15.9);
            LabCard labCard2 = new(invoice2, 2, 20.2, 12.9);
            LabCard labCard3 = new(invoice3, 3, 15.2, 10.9);
            LabCard labCard4 = new(invoice4, 4, 17.9, 13.1);
            LabCard labCard5 = new(invoice5, 5, 23.2, 13.9);
            LabCard labCard6 = new(invoice6, 6, 18.2, 10.9);
            LabCard labCard7 = new(invoice7, 7, 13.2, 8.9);
            LabCard labCard8 = new(invoice8, 8, 15.9, 11.1);

            labCardDB.Add(labCard1);
            labCardDB.Add(labCard2);
            labCardDB.Add(labCard3);
            labCardDB.Add(labCard4);
            labCardDB.Add(labCard5);
            labCardDB.Add(labCard6);
            labCardDB.Add(labCard7);
            labCardDB.Add(labCard8);

            ProductionBatch pb1 = new(labCard1, 1, 13);
            ProductionBatch pb2 = new(labCard2, 1, 14);
            ProductionBatch pb3 = new(labCard3, 1, 13);
            ProductionBatch pb4 = new(labCard4, 1, 8);
            ProductionBatch pb5 = new(labCard5, 1, 13);
            ProductionBatch pb6 = new(labCard6, 1, 14);
            ProductionBatch pb7 = new(labCard7, 1, 13);
            ProductionBatch pb8 = new(labCard8, 1, 8);

            Register reg1 = new(1, pb1, pb5);
            Register reg2 = new(2, pb2, pb6);
            Register reg3 = new(3, pb3, pb7);
            Register reg4 = new(4, pb4, pb8);

            registerDB.Add(reg1);
            registerDB.Add(reg2);
            registerDB.Add(reg3);
            registerDB.Add(reg4);
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
