namespace classwork
{
    public class ControlRules
    {
        public HeaterRules Heater { get; set; } = new();
        public FanRules Fan { get; set; } = new();
        public PumpRules Pump { get; set; } = new();
    }

    public class HeaterRules
    {
        public double OnBelow { get; set; }
        public double OffAbove { get; set; }
    }

    public class FanRules
    {
        public double OnAboveTemp { get; set; }
        public double OffBelowTemp { get; set; }
        public double OnAboveHumidity { get; set; }
        public double OffBelowHumidity { get; set; }
    }

    public class PumpRules
    {
        public double OnBelowSoil { get; set; }
        public double OffAboveSoil { get; set; }
    }
}
