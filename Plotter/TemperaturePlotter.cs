using System;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NmmEnvironment
{
    public class TemperaturePlotter
    {
        private static readonly int _chartWidth = 1000;
        private static readonly int _chartHeight = 600;
        private readonly DataSeries[] _data;
        private readonly double _XStart;
        private readonly double _xStopX;
        private readonly double _yStart;
        private readonly double _yStop;
        private readonly double _xInterval = 50;
        private readonly double _yInterval;

        public TemperaturePlotter(DataSeries[] data)
        {
            EstimatePlotParameters();
            _data = data;

        }

        private void EstimatePlotParameters()
        {
            for (int index = 0; index < _data.Length; index++)
            {
                double xMin = 0;
                double xMax = _data[index].Times.Length;
                double yMax = _data[index].Values.Max();
                double yMin = _data[index].Values.Min();
            }

            throw new NotImplementedException();
        }

        private Form CreateTransmissionChartForm(string titleText)
        {
            int formWidth = _chartWidth + 30;
            int formHeight = _chartHeight + 30;
            Form form = new Form();
            Chart chart = new Chart();
            ChartArea chartArea = new ChartArea();
            Series[] series = new Series[_data.Length];
            Title title = new Title();
            Label label = new Label();
            form.Controls.Add(chart);
            form.Text = "Filter Transmission Plot";
            form.Size = new Size(formWidth, formHeight);
            title.Text = titleText;
            title.Font = new Font("Arial", 14, FontStyle.Bold);
            chart.ChartAreas.Add(chartArea);
            chart.Titles.Add(title);
            chart.Dock = DockStyle.Fill;
            for (int index = 0; index < series.Length; index++)
            {
                series[index] = new Series();
                chart.Series.Add(series[index]);
                series[index].Points.DataBindXY(_data[index].Times, _data[index].Values);
                series[index].Name = $"Sample {index + 1}";
                series[index].Color = Color.FromArgb((index * 70) % 256, (index * 130) % 256, (index * 200) % 256);
            }
            foreach (var s in chart.Series)
            {
                s.ChartType = SeriesChartType.Line;
                s.MarkerSize = 5;
                s.BorderWidth = 3;
                s.XValueType = ChartValueType.Int32;
                s.IsVisibleInLegend = true;
                s.LegendText = s.Name;
            }
            // x-Axis settings
            chartArea.Axes[0].Title = "Index";
            chartArea.Axes[0].TitleFont = new Font("Arial", 12, FontStyle.Regular);
            chartArea.Axes[0].Minimum = _XStart;
            chartArea.Axes[0].Maximum = _xStopX;
            chartArea.Axes[0].Interval = _xInterval;
            chartArea.Axes[0].MajorGrid.Interval = _xInterval;
            chartArea.Axes[0].MajorTickMark.Interval = _xInterval;
            // y-Axis settings
            chartArea.Axes[1].Title = "Temperature / °C";
            chartArea.Axes[1].TitleFont = new Font("Arial", 12, FontStyle.Regular);
            chartArea.Axes[1].Minimum = _yStart;
            chartArea.Axes[1].Maximum = _yStop;
            chartArea.Axes[1].Interval = _yInterval;
            chartArea.Axes[1].MajorGrid.Interval = _yInterval;
            chartArea.Axes[1].MajorTickMark.Interval = _yInterval;
            return form;
        }

        public void ShowTransmissionChart(string titleText)
        {
            Form form = CreateTransmissionChartForm(titleText);
            form.ShowDialog();
        }

        public void SaveTransmissionChart(string titleText, string filePath)
        {
            Form form = CreateTransmissionChartForm(titleText);
            form.FormBorderStyle = FormBorderStyle.None;
            form.Show();
            form.Update();
            form.PerformLayout();
            Thread.Sleep(100); // Allow time for the form to render
            Bitmap bitmap = new Bitmap(form.ClientSize.Width, form.ClientSize.Height);
            form.DrawToBitmap(bitmap, new Rectangle(0, 0, form.ClientSize.Width, form.ClientSize.Height));
            form.Close();
            // Save the bitmap to a file as PNG
            bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            form.Close();
        }

    }
}
