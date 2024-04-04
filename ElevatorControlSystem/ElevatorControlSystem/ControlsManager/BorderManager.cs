using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace ElevatorControlSystem.ControlsManager
{
    internal class BorderManager
    {
        public static void ShowBorder(Border border)
        {
            border.Visibility = Visibility.Visible;
        }
        public static void HideBorder(Border border)
        {
            border.Visibility = Visibility.Collapsed;
        }
    }
}
