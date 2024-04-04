using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using ElevatorControlSystem.ControlsManager;

namespace ElevatorControlSystem.MyControl
{
    public class BarChartControl : ContentControl
    {
        private ToolTip barToolTip;
        private bool isMouseOverChart;

        private ScrollViewer scrollViewer;

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(double[]), typeof(BarChartControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public double[] Data
        {
            get { return (double[])GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public static readonly DependencyProperty CategoriesProperty =
            DependencyProperty.Register("Categories", typeof(string[]), typeof(BarChartControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public string[] Categories
        {
            get { return (string[])GetValue(CategoriesProperty); }
            set { SetValue(CategoriesProperty, value); }
        }

        public static readonly DependencyProperty TickValuesProperty =
       DependencyProperty.Register("TickValues", typeof(double[]), typeof(BarChartControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public double[] TickValues
        {
            get { return (double[])GetValue(TickValuesProperty); }
            set { SetValue(TickValuesProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty =
      DependencyProperty.Register("Foreground", typeof(Brush), typeof(BarChartControl), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(BarChartControl), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public BarChartControl()
        {
            barToolTip = new ToolTip();
            this.ToolTip = barToolTip;
            this.MouseEnter += BarChartControl_MouseEnter;
            this.MouseLeave += BarChartControl_MouseLeave;
            this.isMouseOverChart = false;
            barToolTip.Background = Brushes.White;
            barToolTip.BorderBrush = Brushes.Black; 
            barToolTip.BorderThickness = new Thickness(1); 
            barToolTip.Padding = new Thickness(8); 
            barToolTip.FontSize = 16;
      
        }


        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
                throw new ArgumentOutOfRangeException();

            return scrollViewer;
        }
        [System.Obsolete]

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            double padding = -50;
            double cornerRadius = 10;
            double drawingWidth = Math.Max(RenderSize.Width - padding * 2, 0);
            double drawingHeight = Math.Max(RenderSize.Height - padding * 2, 0);

            double offsetX = padding / 2 - 30;
            double offsetY = padding / 2 - 30;

            drawingContext.DrawRoundedRectangle(null, new Pen(Brushes.Black, 1), new Rect(offsetX, offsetY, drawingWidth, drawingHeight), cornerRadius, cornerRadius);

            if (Data == null || Data.Length == 0 || TickValues == null || TickValues.Length == 0 || Categories == null || Categories.Length != Data.Length)
                return;

            double width = ActualWidth;
            double height = ActualHeight;

            double barWidth = width / (Data.Length * 2);
            double maxTickValueWidth = 30;
            double maxTickValueY = 20;
            double tickLineOffsetX = 5;

            Pen barPen = new Pen(Brushes.Black, 1);
            Brush grayBrush = Brushes.Gray;

            for (int i = 0; i < Data.Length; i++)
            {
                double barHeight;

                if (Data[i] > TickValues[i])
                {
                    barHeight = (TickValues[i] / TickValues[i]) * (height - maxTickValueY - 40) + 30;
                }
                else
                {
                    barHeight = (Data[i] / TickValues[i]) * (height - maxTickValueY - 40) + 30;
                }


                double x = i * barWidth * 2;
                double y = height - barHeight;

                 y = Math.Min(y, height - maxTickValueY);

                if (Data[i] > TickValues[i])
                {
                    double excessPercentage = ((Data[i] - TickValues[i]) / TickValues[i]) * 100;
                    string excessText = $"+{excessPercentage:F1}%";

                    FormattedText excessPercentageText = new FormattedText(
                        excessText,
                        System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"), 14, Brushes.Red);

                    double percentageX = x + barWidth / 2 - excessPercentageText.Width / 2;
                    double percentageY = y - 50;
                    drawingContext.DrawText(excessPercentageText, new Point(percentageX, percentageY));
                }

                Rect barRect = new Rect(x, y, barWidth, barHeight);
                drawingContext.DrawRectangle(Brushes.Black, barPen, barRect);

                FormattedText maxTickValueText = new FormattedText(
                    TickValues[i].ToString(),
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"), 12, Brushes.Black);

                double maxTickValueX = x - maxTickValueWidth - tickLineOffsetX;
                drawingContext.DrawText(maxTickValueText, new Point(maxTickValueX, maxTickValueY));

                double tickLineX = maxTickValueX + maxTickValueWidth + tickLineOffsetX;
                drawingContext.DrawLine(new Pen(grayBrush, 1), new Point(tickLineX, maxTickValueY), new Point(tickLineX, y));

                FormattedText categoryText = new FormattedText(
                    Categories[i],
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"), 14, Brushes.Black);

                double categoryX = x + barWidth / 2 - categoryText.Width / 2;
                double categoryY = height + 5;
                drawingContext.DrawText(categoryText, new Point(categoryX, categoryY));

                FormattedText valueText = new FormattedText(
                    Data[i].ToString(),
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"), 12, Brushes.Black);

                double valueX = x + barWidth / 2 - valueText.Width / 2;
                double valueY = y - 20;
                drawingContext.DrawText(valueText, new Point(valueX, valueY));
            }
        }






        private void BarChartControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isMouseOverChart = true;
            int column = DetermineColumnFromMousePosition(e.GetPosition(this).X);
            if (column >= 0 && column < Data.Length)
            {
                barToolTip.Content = "Значение: " + Data[column];
            }
            else
            {
                barToolTip.Content = null;
            }
        }

        private void BarChartControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isMouseOverChart = false;
            barToolTip.Content = null;
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!isMouseOverChart)
            {
                barToolTip.Content = null;
            }
        }

        private int DetermineColumnFromMousePosition(double mouseX)
        {
            try
            {
                if (Data == null || ActualWidth == null || Data.Length == 0 || ActualWidth == 0)
                {
                    return -1; 
                }

                double barWidth = ActualWidth / Data.Length;
                int column = (int)(mouseX / barWidth);
                return column;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при определении столбца из положения мыши.", ex);
            }
        }


        public void Reset()
        {
            Data = null;
            Categories = null;
            TickValues = null;
        }

        public void Add(double[] newTickValues, string[] newCategories, double[] newData)
        {
            if (Data != null && Data.Length >= 10)
            {
                NotificationManager.ShowErrorMessageBox("Простите, но Ваш пакет разрешает только 10 графиков.");
                return;
            }

            if (newData == null || newCategories == null || newTickValues == null || newCategories.Length != newData.Length || newCategories.Length != newTickValues.Length)
                return;

            if (Data != null)
            {
                 Data = Data.Concat(newData).ToArray();
            }
            else
            {
                Data = newData.ToArray();
            }

            if (Categories != null)
            {
                Categories = Categories.Concat(newCategories).ToArray();
            }
            else
            {
                Categories = newCategories.ToArray();
            }

            if (TickValues != null)
            {
                TickValues = TickValues.Concat(newTickValues).ToArray();
            }
            else
            {
                TickValues = newTickValues.ToArray();
            }

            InvalidateVisual();
        }

        public void SortDataAscending()
        {
            if (Data != null && Data.Length > 0)
            {
                var sortedData = Data
                    .Select((value, index) => new { Value = value, Index = index })
                    .OrderBy(item => TickValues[item.Index])
                    .ThenBy(item => item.Value)
                    .ToArray();

                Data = sortedData.Select(item => item.Value).ToArray();
                Categories = sortedData.Select(item => Categories[item.Index]).ToArray();
                TickValues = sortedData.Select(item => TickValues[item.Index]).ToArray();
            }
            else
            {
                NotificationManager.ShowErrorMessageBox("Нет данных для сортировки.");

            }
        }


        public void SortDataDescending()
        {
            if (Data != null && Data.Length > 0)
            {
                var sortedData = Data
                    .Select((value, index) => new { Value = value, Index = index })
                    .OrderByDescending(item => TickValues[item.Index])
                    .ThenByDescending(item => item.Value)
                    .ToArray();

                Data = sortedData.Select(item => item.Value).ToArray();
                Categories = sortedData.Select(item => Categories[item.Index]).ToArray();
                TickValues = sortedData.Select(item => TickValues[item.Index]).ToArray();
            }
            else
            {
                NotificationManager.ShowErrorMessageBox("Нет данных для сортировки.");
              }
        }

        public void Refresh()
        {
            InvalidateVisual();
        }




    }
}


