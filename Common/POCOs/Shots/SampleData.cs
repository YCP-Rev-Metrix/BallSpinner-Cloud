using System.Text.Json.Serialization;

namespace Common.POCOs;
///<Summary>
/// Placeholder (fill in this section later)
///</Summary>
public class SampleData: Poco
{
    public SampleData() { }

    public SampleData(string? type, float? timestamp, int count, float brightness, double? x, double? y, double? z)
    {
        Type = type;
        Logtime = timestamp;
        Count = count;
        Brightness = brightness;
        X = x;
        Y = y;
        Z = z;
    }
    [JsonPropertyName("Type")]
    public string? Type {get; set;}

    [JsonPropertyName("Logtime")]
    public double? Logtime { get; set; }

    [JsonPropertyName("Count")]
    public int? Count { get; set; }
    [JsonPropertyName("Brightness")]
    public float? Brightness { get; set; }


    [JsonPropertyName("X")]
    public double? X { get; set; }

    [JsonPropertyName("Y")]
    public double? Y { get; set; }

    [JsonPropertyName("Z")]
    public double? Z { get; set; }
    

}