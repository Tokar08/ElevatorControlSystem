using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElevatorControlSystem.ControlsManager
{
    class NotificationManager
    {
        public static void ShowSuccessMessageBox(string message)
        {
            MessageBox.Show(message, "Успех!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        public static void ShowErrorMessageBox(string message)
        {
            MessageBox.Show(message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static void ShowInfoMessageBox(string message)
        {
            MessageBox.Show(message, "Обратите внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

     
    }
}
