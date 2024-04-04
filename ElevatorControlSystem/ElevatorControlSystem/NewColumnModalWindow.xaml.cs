using ElevatorControlSystem.MyControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ElevatorControlSystem
{
    /// <summary>
    /// Логика взаимодействия для NewColumnModalWindow.xaml
    /// </summary>
    public partial class NewColumnModalWindow : Window
    {
        BarChartControl barChartControl;
        public NewColumnModalWindow(BarChartControl barChart)
        {
            this.barChartControl = barChart;
            InitializeComponent();
        }

        private void btnExitFromHelp_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation closeAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            closeAnimation.Completed += (s, _) => Close();

            BeginAnimation(OpacityProperty, closeAnimation);
        }

        private void bntCreateNewcolumn_Click(object sender, RoutedEventArgs e)
        {
         
            if (string.IsNullOrEmpty(tbCreateNewTickValues.Text) ||
                string.IsNullOrEmpty(tbCreateNewCategories.Text) ||
                string.IsNullOrEmpty(tbCreateNewData.Text))
            {
                 MessageBox.Show("Пожалуйста, заполните все поля.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(tbCreateNewTickValues.Text, out double tickValue) ||
                !double.TryParse(tbCreateNewData.Text, out double dataValue))
            {
                MessageBox.Show("Неверный цифровой ввод. Пожалуйста, введите действительные числовые значения.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            barChartControl.Add
            (
                new double[] { tickValue },
                new string[] { tbCreateNewCategories.Text },
                new double[] { dataValue }
            );
        }

    }
}
