using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElevatorControlSystem.ControlsManager
{
    public static class CheckManager
    {
        public static bool IsNotNullOrWhiteSpace(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsDate(string value)
        {
            return DateTime.TryParse(value, out _);
        }

        public static bool IsInt(string value)
        {
            return int.TryParse(value, out _);
        }

        public static bool IsDouble(string value)
        {
            return double.TryParse(value, out _);
        }
        public static bool IsFloat(string value)
        {
            return float.TryParse(value, out _);
        }
        public static bool IsBool(string value)
        {
            return bool.TryParse(value, out _);
        }

       
    }
}
