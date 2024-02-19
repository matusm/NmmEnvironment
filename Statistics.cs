using At.Matus.StatisticPod;
using Bev.IO.NmmReader;

namespace NmmEnvironment
{
    public class Statistics
    {
        public double SampleTemperature => stpTs.AverageValue;
        public double AirTemperature => stpTa.AverageValue;
        public double SampleTemperatureRange => stpTs.Range;
        public double AirTemperatureRange => stpTa.Range;

        public void Update(NmmEnvironmentData nmmPos)
        {
            if (nmmPos == null)
                return;
            double[] sSeries = nmmPos.SampleTemperatureSeries;
            double[] aSeries = nmmPos.AirTemperatureSeries;
            double[] hSeries = nmmPos.RelativeHumiditySeries;
            double[] pSeries = nmmPos.BarometricPressureSeries;
            for (int i = 0; i < sSeries.Length; i++)
            {
                Update(sSeries[i], aSeries[i], hSeries[i], pSeries[i]);
            }
        }

        private void Update(double t1, double t2, double h, double p)
        {
            stpTs.Update(t1);
            stpTa.Update(t2);
        }

        private readonly StatisticPod stpTs = new StatisticPod();
        private readonly StatisticPod stpTa = new StatisticPod();
    }
}
