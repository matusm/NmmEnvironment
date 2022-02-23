using System.Text;
using Bev.IO.NmmReader;

namespace NmmEnvironment
{
    public class Csv
    {
        private const string celsiusSymbol = "°C";
        private readonly StringBuilder sb = new StringBuilder();
        private OutputStyle outputStyle;

        public Csv(OutputStyle outputStyle)
        {
            this.outputStyle = outputStyle;
            RunningIndex = 0;
            AddCsvHeader();
        }

        public int RunningIndex { get; private set; }

        public void Add(NmmEnvironmentData nmmPos, int scanIndex)
        {
            if (nmmPos == null)
                return;
            double[] sSeries = nmmPos.SampleTemperatureSeries;
            double[] aSeries = nmmPos.AirTemperatureSeries;
            double[] hSeries = nmmPos.RelativeHumiditySeries;
            double[] pSeries = nmmPos.BarometricPressureSeries;
            for (int i = 0; i < sSeries.Length; i++)
            {
                RunningIndex++;
                AddCsvLine(RunningIndex, scanIndex, sSeries[i], aSeries[i], hSeries[i], pSeries[i]);
            }
        }

        public string GetCsvString()
        {
            return sb.ToString();
        }

        private void AddCsvLine(int runningIndex, int scanIndex, double t1, double t2, double h, double p)
        {
            switch (outputStyle)
            {
                case OutputStyle.Pretty:
                    sb.AppendLine($"{runningIndex,5} , {scanIndex,3} , {t1:F3} , {t2:F3} , {h:F2} , {p:F0}");
                    break;
                case OutputStyle.Plain:
                    sb.AppendLine($"{runningIndex},{scanIndex},{t1:F4},{t2:F4},{h:F3},{p}");
                    break;
                default:
                    break;
            }
        }

        private void AddCsvHeader()
        {
            switch (outputStyle)
            {
                case OutputStyle.Pretty:
                    sb.AppendLine($"index , scan , sample temperature ({celsiusSymbol}) , air temperature ({celsiusSymbol}) , humidity (%) , pressure (Pa)");
                    break;
                case OutputStyle.Plain:
                    sb.AppendLine($"index,scan,sample temperature ({celsiusSymbol}),air temperature ({celsiusSymbol}),humidity (%),pressure (Pa)");
                    break;
                default:
                    break;
            }
        }
    }

    public enum OutputStyle
    {
        Plain,
        Pretty
    }
}
