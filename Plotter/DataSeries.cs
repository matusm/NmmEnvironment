using System.Collections.Generic;
using System.Linq;

namespace NmmEnvironment
{
    public class DataSeries
    {
        private readonly List<DataPoint> _dataPoints;

        public DataSeries()
        {
            _dataPoints = new List<DataPoint>();
        }

        public void AddDataPoint(double time, double value)
        {
            _dataPoints.Add(new DataPoint(time, value));
        }

        public void AddDataPoint(DataPoint dataPoint)
        {
            _dataPoints.Add(dataPoint);
        }

        public double[] Times => _dataPoints.Select(dp => dp.Time).ToArray();
        public double[] Values => _dataPoints.Select(dp => dp.Value).ToArray();
    }
}
