using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ElevatorControlSystem
{
    public class TimeUpdater
    {
        private TextBlock textBlock;
        private DispatcherTimer timer  = new DispatcherTimer();
        private bool Normalformat;

        public TimeUpdater(TextBlock textBox, bool Normalformat)
        {
            this.textBlock = textBox;
            this.Normalformat = Normalformat;

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_TickAsync;
            timer.Start();
        }

        private void Timer_TickAsync(object sender, EventArgs e)
        {
            string timeFormat = Normalformat ? "HH:mm:ss" : "hh:mm:ss tt";
            textBlock.Text = DateTime.Now.ToString(timeFormat);
        }
    }
}
