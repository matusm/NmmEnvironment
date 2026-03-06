namespace NmmEnvironment
{
    public class DataPoint
    {
        public double Time { get; }
        public double Value { get; }

        public DataPoint(double time, double temperature)
        {
            Time = time;
            Value = temperature;
        }
    }
}
