using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace ElevatorControlSystem.ControlsManager
{
    internal class CalendarManager
    {
        public static void ShowCalendar(Calendar calendar)
        {
            if (calendar.Visibility == Visibility.Collapsed)
            {
                calendar.Visibility = Visibility.Visible;
            }
            else
            {
                calendar.Visibility = Visibility.Collapsed;
            }
        }

        public static void SendaDate(Calendar calendar, Button button)
        {
            DateTime selectedDate = calendar.SelectedDate ?? DateTime.Now;

            button.Content = selectedDate.ToShortDateString();

            calendar.Visibility = Visibility.Collapsed;
        }
    }
}
