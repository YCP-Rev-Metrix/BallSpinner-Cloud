namespace Common.POCOs;
///<Summary>
/// Placeholder (fill in this section later)
///</Summary>
public class SimulatedShot: Poco
{
    public SimulatedShot() { }

    public SimulatedShot(string? name, float? speed, float? angle, float? position, float? frequency)
    {
        Name = name;
        Speed = speed;
        Angle = angle;
        Position = position;
        Frequency = frequency;
    }

    public string? Name { get; set; }
    public float? Speed { get; set; }
    public float? Angle { get; set; }
    public float? Position { get; set; }
    public float? Frequency { get; set; }

}