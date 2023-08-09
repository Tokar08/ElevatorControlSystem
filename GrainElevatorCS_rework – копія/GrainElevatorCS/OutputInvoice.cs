using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrainElevatorCS
{
    // Расходная накладная.
    // ===============================
    // Содержит информацию об отгружаемой продукции продукции.

    public class OutputInvoice
    {
        public DateTime Date { get; set; } = DateTime.Now;  // дата отгрузки
        public string InvNumber { get; set; } = string.Empty; // номер накладной
        public string VenicleRegNumber { get; set; } = string.Empty; // гос.номер транспортного средства
        public string Supplier { get; set; } = string.Empty; // наименование предприятия Поставщика
        public string ProductTitle { get; set; } = string.Empty; // наименование Продукции
        public string Category { get; set; } = string.Empty; // Категория Продукции
        public int Weight { get; set; } = 0; //  вес Продукции


        public OutputInvoice() { }

        public OutputInvoice(DateTime date, string invNumber, string venicleRegNumber, string supplier, string productTitle, string category, int productWeight)
        {
            Date = date;
            InvNumber = invNumber;
            VenicleRegNumber = venicleRegNumber;
            Supplier = supplier;
            ProductTitle = productTitle;
            Category = category;
            Weight = productWeight;
        }







        // ввод информации (ДЛЯ ТЕСТА НА КОНСОЛИ)
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
                    outInv.InvNumber = Console.ReadLine();

                    Console.Write("Регистрационний номер транспортного средства:        ");
                    outInv.VenicleRegNumber = Console.ReadLine();

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
            return $"\nРасходная накладная №{InvNumber}.\n" +
                   $"---------------------------\n" +
                   $"Дата отгрузки:             {Date.ToString("dd.MM.yyyy")}\n" +
                   $"Номер ТС:                  {VenicleRegNumber}\n"+
                   $"Поставщик:                 {Supplier}\n" +
                   $"Наименование продукции:    {ProductTitle}\n" +
                   $"Категория продукции:       {Category}\n" +
                   $"Вес нетто:                 {Weight} кг\n\n";
        }
    }
}
