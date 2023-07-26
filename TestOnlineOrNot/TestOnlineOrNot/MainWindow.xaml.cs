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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WindowsInput;

namespace TestOnlineOrNot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isUserActive;
        private DateTime lastActiveTime;
        private DispatcherTimer activityTimer;
        //Время для того что бы пользователь стал "неактивен"
        private int inactivityTimeInSeconds = 10;

        //Время для того что бы пользователь стал "не в сети"
        private int offlineTimeInSeconds = 20;

        public MainWindow()
        {
            InitializeComponent();
            isUserActive = false;
            lastActiveTime = DateTime.Now;

          
            Application.Current.MainWindow.MouseMove += MainWindow_MouseMove;
            Application.Current.MainWindow.MouseWheel += MainWindow_MouseWheel;
            Application.Current.MainWindow.PreviewKeyDown += MainWindow_PreviewKeyDown;

            // Инициализация таймера активности
            activityTimer = new DispatcherTimer();
            activityTimer.Interval = TimeSpan.FromSeconds(1); // Проверяем активность каждую секунду
            activityTimer.Tick += ActivityTimer_Tick;
            activityTimer.Start();
        }

        private void ActivityTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan timeSinceLastActive = DateTime.Now - lastActiveTime;
            if (timeSinceLastActive.TotalSeconds < inactivityTimeInSeconds)
            {
          
                StatusTextBlock.Text = "Пользователь активен";
            }
            else if (timeSinceLastActive.TotalSeconds >= inactivityTimeInSeconds && timeSinceLastActive.TotalSeconds < offlineTimeInSeconds)
            {
                StatusTextBlock.Text = "Пользователь неактивен";
            }
            else
            {
                StatusTextBlock.Text = "Пользователь не в сети";
            }
        }

        private void UpdateLastActiveTime()
        {
            lastActiveTime = DateTime.Now;
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            isUserActive = true;
            UpdateLastActiveTime();
        }

        private void MainWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            isUserActive = true;
            UpdateLastActiveTime();
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            isUserActive = true;
            UpdateLastActiveTime();
        }
    }
}
