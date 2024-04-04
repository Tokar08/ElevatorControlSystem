using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для ContactUsWindow.xaml
    /// </summary>
    public partial class ContactUsWindow : Window
    {
        public ContactUsWindow()
        {
            InitializeComponent();
        }

        private void btnOpenMail_Click(object sender, RoutedEventArgs e)
        {
            string pathURL = "https://mail.google.com/mail/u/0/#inbox?compose=new";
            Process.Start(new ProcessStartInfo
            {
                FileName = pathURL,
                UseShellExecute = true
            });
        }

        private void btnExitFromContactUs_Click(object sender, RoutedEventArgs e)
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

        private async void btcCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(tbForMail.Text);
            tbCopiedNotification.Visibility = Visibility.Visible;
            await Task.Delay(TimeSpan.FromSeconds(1.5));
            tbCopiedNotification.Visibility = Visibility.Collapsed;
        }
    }
}
