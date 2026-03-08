using System.Text;
using At.Matus.IO.NmmReader;

namespace NmmEnvironment
{
    public class Csv
    {
        private const string celsiusSymbol = "°C";
        private readonly StringBuilder sb = new StringBuilder();
        private OutputStyle outputStyle;
        private readonly DataSeries dsSampleTemperature = new DataSeries();
        private readonly DataSeries dsAirTemperature = new DataSeries();

        public Csv(OutputStyle outputStyle)
        {
            this.outputStyle = outputStyle;
            RunningIndex = 0;
            AddCsvHeader();
        }

        public int RunningIndex { get; private set; }
        public DataSeries SampleTemperatureSeries => dsSampleTemperature;
        public DataSeries AirTemperatureSeries => dsAirTemperature;

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
                dsSampleTemperature.AddDataPoint(RunningIndex, sSeries[i]);
                dsAirTemperature.AddDataPoint(RunningIndex, aSeries[i]);
                AddCsvLine(RunningIndex, scanIndex, sSeries[i], aSeries[i], hSeries[i], pSeries[i]);
            }
        }

        public string GetCsvString() => sb.ToString();

        private void AddCsvLine(int runningIndex, int scanIndex, double t1, double t2, double h, double p)
        {
            switch (outputStyle)
            {
                case OutputStyle.Pretty:
                    sb.AppendLine($"{runningIndex,5} , {scanIndex,3} , {t1,6:F3} , {t2,6:F3} , {h,5:F2} , {p,6:F0}");
                    break;
                case OutputStyle.Plain:
                    sb.AppendLine($"{runningIndex},{scanIndex},{t1:F3},{t2:F3},{h:F2},{p:F0}");
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
