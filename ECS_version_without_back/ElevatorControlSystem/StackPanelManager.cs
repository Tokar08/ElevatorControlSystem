using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;

namespace ElevatorControlSystem
{
    public static class StackPanelManager
    {
        public static void ShowStackPanel(StackPanel stackPanel)
        {
            stackPanel.Visibility = Visibility.Visible;

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                double targetHeight = stackPanel.Children.Cast<UIElement>().Sum(child => child.DesiredSize.Height);

                stackPanel.Height = 1;

                var animation = new DoubleAnimation
                {
                    From = 1,
                    To = targetHeight + 10,
                    Duration = TimeSpan.FromSeconds(0.8)
                };

                stackPanel.BeginAnimation(StackPanel.HeightProperty, animation);
            }), System.Windows.Threading.DispatcherPriority.Render);
        }

        public static void HideStackPanel(StackPanel stackPanel)
        {
            var animation = new DoubleAnimation
            {
                From = stackPanel.ActualHeight,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.8),
                EasingFunction = new QuadraticEase()
            };

            animation.Completed += (s, e) =>
            {
                stackPanel.Visibility = Visibility.Collapsed;
            };

            stackPanel.BeginAnimation(StackPanel.HeightProperty, animation);
        }
    }
}
