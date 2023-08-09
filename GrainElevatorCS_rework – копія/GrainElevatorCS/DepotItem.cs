using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

//Складская учетная единица.
//==========================
//Суммарные количественные показатели категорий Наименования продукции: Зачетный вес, Отход.

namespace GrainElevatorCS
{
    public class DepotItem
    {
        public string Supplier { get; set; } = string.Empty; // наименование предприятия Поставщика
        public string ProductTitle { get; set; } = string.Empty; // наименование Продукции

        public Dictionary<string, int>? Сategories { get; set; } // коллекция категорий хранимой продукции

        public DepotItem() { }

        public DepotItem(params Register[] registers)
        {
            Supplier = registers[0].Supplier;
            ProductTitle = registers[0].ProductTitle;
            Сategories = new Dictionary<string, int>();

            foreach (Register r in registers)
            {
                if (Supplier == r.Supplier && ProductTitle == r.ProductTitle)
                {
                    Сategories.Add("Кондиционная продукция", r.AccWeightsReg);
                    Сategories.Add("Отход", r.WastesReg);
                }
            } 
        }

        public void AddRegister(Register register)
        {
            if(Supplier == register.Supplier && ProductTitle == register.ProductTitle)
            {
                int weight = 0;
                Сategories?.Remove("Кондиционная продукция", out weight);
                Сategories?.Add("Кондиционная продукция", weight + register.AccWeightsReg);

                Сategories?.Remove("Отход", out weight);
                Сategories?.Add("Отход", weight + register.WastesReg);
            }
            else
            {
                DepotItem di = new DepotItem(register);
            }
        }






        // вивод на КОНСОЛЬ
        public void PrintDepotItem()
        {
            Console.WriteLine(
                   $"Поставщик:                 {Supplier}\n" +
                   $"Наименование:              {ProductTitle}");

            if(Сategories != null)
                foreach (var c in Сategories)
                    Console.WriteLine($"{c.Key}:      {c.Value}");

            Console.WriteLine();
        }



    }
}
