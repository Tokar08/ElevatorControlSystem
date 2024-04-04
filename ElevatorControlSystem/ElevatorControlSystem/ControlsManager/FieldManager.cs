using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElevatorControlSystem.ControlsManager
{
    public static class FieldManager
    {
        public static void ClearFields(params Control[] controls)
        {
            foreach (var control in controls)
            {
                ClearControl(control);
            }
        }

        private static void ClearControl(Control control)
        {
            switch (control)
            {
                case TextBox textBox:
                    ClearTextBox(textBox);
                    break;

                case ComboBox comboBox:
                    ClearComboBox(comboBox);
                    break;

                case DatePicker datePicker:
                    ClearDatePicker(datePicker);
                    break;

               

                case CheckBox checkBox:
                    ClearCheckBox(checkBox);
                    break;

                case Button button:
                    SetButtonDefaultContent(button);
                    break;
            }
        }

        private static void ClearTextBox(TextBox textBox) => textBox.Clear();
        private static void ClearComboBox(ComboBox comboBox) => comboBox.SelectedIndex = -1;
        private static void ClearDatePicker(DatePicker datePicker) => datePicker.SelectedDate = null;
        private static void ClearCheckBox(CheckBox checkBox) => checkBox.IsChecked = false;
        private static void SetButtonDefaultContent(Button button) => button.Content = "Выбрать дату";
      

    }
}

