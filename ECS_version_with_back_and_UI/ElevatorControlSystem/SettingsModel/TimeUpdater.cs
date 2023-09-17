using System;
using System.Windows.Controls;
using System.Windows.Threading;

public class TimeUpdater
{
    private TextBlock textBlock;
    private DispatcherTimer timer = new DispatcherTimer();
    private bool Normalformat;

    public TimeUpdater(TextBlock textBox, bool Normalformat)
    {
        this.textBlock = textBox;
        this.Normalformat = Normalformat;

        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    public bool IsRunning
    {
        get { return timer.IsEnabled; }
    }

    public void Stop()
    {
        timer.Stop();
    }

    public void Start()
    {
        timer.Start();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        string timeFormat = Normalformat ? "HH:mm:ss" : "hh:mm:ss tt";
        textBlock.Text = DateTime.Now.ToString(timeFormat);
    }


}