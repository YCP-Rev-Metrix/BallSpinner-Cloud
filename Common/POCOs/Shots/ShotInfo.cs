using System.Text.Json.Serialization;

namespace Common.POCOs;
///<Summary>
/// Placeholder (fill in this section later)
///</Summary>
public class ShotInfo
{
    public ShotInfo() { }

    public ShotInfo(string? name, double? speed, double? angle, double? position, double? frequency)
    {
        Name = name;
        Speed = speed;
        Angle = angle;
        Position = position;
        Frequency = frequency;
    }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }
    [JsonPropertyName("Speed")]
    public double? Speed { get; set; }
    [JsonPropertyName("Angle")]
    public double? Angle { get; set; }
    [JsonPropertyName("Position")]
    public double? Position { get; set; }
    [JsonPropertyName("Frequency")]
    public double? Frequency { get; set; }

}