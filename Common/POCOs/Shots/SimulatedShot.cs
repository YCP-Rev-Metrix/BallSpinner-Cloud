namespace Common.POCOs;
///<Summary>
/// Placeholder (fill in this section later)
///</Summary>
public class SimulatedShot: Poco
{
    public SimulatedShot() { }

    public SimulatedShot(string? name, double? speed, double? angle, double? position, double? frequency)
    {
        Name = name;
        Speed = speed;
        Angle = angle;
        Position = position;
        Frequency = frequency;
    }

    public string? Name { get; set; }
    public double? Speed { get; set; }
    public double? Angle { get; set; }
    public double? Position { get; set; }
    public double? Frequency { get; set; }

}