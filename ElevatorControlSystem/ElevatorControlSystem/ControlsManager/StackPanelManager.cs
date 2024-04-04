using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Shapes;

namespace ElevatorControlSystem.ControlsManager
{
    public static class StackPanelManager
    {
        public static void ShowStackPanel(StackPanel stackpanel)
        {
            stackpanel.Visibility = Visibility.Visible;
        }
        public static void HideStackPanel(StackPanel stackpanel)
        {
            stackpanel.Visibility = Visibility.Collapsed;
        }
        public static void ShowStackPanelWithAnimation(StackPanel stackPanel)
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

                stackPanel.BeginAnimation(FrameworkElement.HeightProperty, animation);
            }), System.Windows.Threading.DispatcherPriority.Render);
        }

        public static void HideStackPanelWithAnimation(StackPanel stackPanel)
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

            stackPanel.BeginAnimation(FrameworkElement.HeightProperty, animation);
        }


    }

    public class StackPanelWithEllipse
    {
        public StackPanel StackPanel { get; }
        public Ellipse Ellipse { get; }

        public StackPanelWithEllipse(StackPanel stackPanel, Ellipse ellipse)
        {
            StackPanel = stackPanel;
            Ellipse = ellipse;
        }

        public void Highlight()
        {
            Ellipse.Visibility = Visibility.Visible;
        }

        public void Unhighlight()
        {
            Ellipse.Visibility = Visibility.Collapsed;
        }

    }
}
