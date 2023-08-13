using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrainElevatorCS
{
    public class TechnologicalOperation 
    {
        public string Title { get; set; }  // Наименование Технологической операции 
        public double Amount { get; set; } = 0;  // Количество Продукции (тонн) обработанних на данной Техн. операции
        public double Price { get; set; } = 0; // Стоимость обработки 1 т Продукции на данной Технологической операции
        public double TotalCost { get; set; } = 0; // Общая стоимость обработки заданного количества Продукции на данной Техн. операции

        public TechnologicalOperation(string title)
        {
            Title = title;
        }
    }
}
