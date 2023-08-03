using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ElevatorControlSystem
{
    public class UserActivityMonitor
    {
        private bool isUserActive;
        private DateTime lastActiveTime;
        private DispatcherTimer activityTimer;
        //Время для того что бы пользователь стал "неактивен"
        private int inactivityTimeInSeconds = 10;
        //Время для того что бы пользователь стал "не в сети"
        private int offlineTimeInSeconds = 20;
        private Ellipse ellipse;

        public UserActivityMonitor(Ellipse ellipse)
        {
            isUserActive = false;
            lastActiveTime = DateTime.Now;
            this.ellipse = ellipse;

            Application.Current.MainWindow.MouseMove += MainWindow_MouseMove;
            Application.Current.MainWindow.MouseWheel += MainWindow_MouseWheel;
            Application.Current.MainWindow.PreviewKeyDown += MainWindow_PreviewKeyDown;

            // Инициализация таймера активности
            activityTimer = new DispatcherTimer();
            // Проверяем активность каждую секунду
            activityTimer.Interval = TimeSpan.FromSeconds(1);  
            activityTimer.Tick += ActivityTimer_Tick; 
            activityTimer.Start();
        }

        private void ActivityTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan timeSinceLastActive = DateTime.Now - lastActiveTime;
            if (timeSinceLastActive.TotalSeconds < inactivityTimeInSeconds)
            {
                ellipse.Fill = Brushes.Green;
            }
            else if (timeSinceLastActive.TotalSeconds >= inactivityTimeInSeconds && timeSinceLastActive.TotalSeconds < offlineTimeInSeconds)
            {
                ellipse.Fill = Brushes.Orange;
            }
            else
            {
                ellipse.Fill = Brushes.Gray;
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
