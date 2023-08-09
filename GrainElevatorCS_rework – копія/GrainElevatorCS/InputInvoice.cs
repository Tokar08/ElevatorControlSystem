using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GrainElevatorCS
{
    // Приходная накладная.
    // ===============================
    // Содержит первичную информацию о входящей партии продукции.
    public class InputInvoice
    {
        public string InvNumber { get; set; } = string.Empty; // номер входящей накладной
        public DateTime Date { get; set; } = new DateTime(0001, 01, 01); // дата прихода 
        public string VenicleRegNumber { get; set; } = string.Empty; // гос.номер транспортного средства
        public string Supplier { get; set; } = string.Empty; // наименование предприятия Поставщика
        public string ProductTitle { get; set; } = string.Empty; // наименование Продукции
        public int PhysicalWeight { get; set; } = 0; // Физический вес Продукции


        public InputInvoice() { }

        public InputInvoice(string invNumber, DateTime date,  string venicleRegNumber, string supplier, string productTitle, int physicalWeight)
        {
            InvNumber = invNumber;
            Date = date;
            VenicleRegNumber = venicleRegNumber;
            Supplier = supplier;
            ProductTitle = productTitle;
            PhysicalWeight = physicalWeight;   
        }









        // ДЛЯ ТЕСТА НА КОНСОЛИ
        public InputInvoice RequestInvoiceInfo(InputInvoice inInv)
        {
            Console.WriteLine("\tВведите информацию для оформления приходной накладной на входящую Продукцию: \n");

            while (true)
            {
                try
                {
                    Console.Write("Номер приходной накладной:                    ");
                    inInv.InvNumber = Console.ReadLine();

                    Console.Write("Дата поступления:                             ");
                    inInv.Date = Convert.ToDateTime(Console.ReadLine());

                    Console.Write("Регистрационний номер транспортного средства: ");
                    inInv.VenicleRegNumber = Console.ReadLine();

                    Console.Write("Наименование Поставщика:                      ");
                    inInv.Supplier = Console.ReadLine();

                    Console.Write("Наименование поступившей Продукции:           ");
                    inInv.ProductTitle = Console.ReadLine();

                    Console.Write("Физический вес поступившей Продукции (кг):    ");
                    inInv.PhysicalWeight = Convert.ToInt32(Console.ReadLine());

                    return inInv;
                }
                catch (Exception)
                {
                    Console.WriteLine("Ошибка ввода. Введите корректные значения данных.");
                }
            }
        }

        public override string ToString()
        {
            return $"Приходная накладная №{InvNumber}\n" +
                   $"Дата прихода:        {Date.ToString("dd.MM.yyyy")}\n" +
                   $"Номер ТС:            {VenicleRegNumber}\n" +
                   $"Поставщик:           {Supplier}\n" +
                   $"Наименование:        {ProductTitle}\n" +
                   $"Вес нетто:           {PhysicalWeight} кг\n";
        }
    }
}
