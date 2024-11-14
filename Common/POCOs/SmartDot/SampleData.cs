namespace Common.POCOs;
///<Summary>
/// Placeholder (fill in this section later)
///</Summary>
public class SampleData: Poco
{
    public SampleData() { }

    public SampleData(int? sensorType, int? count, float? timestamp, float? x, float? y, float? z)
    {
        SensorType = sensorType;
        Count = count;
        Timestamp = timestamp;
        X = x;
        Y = y;
        Z = z;
    }
        
    public int? SensorType { get; set; }

    public int? Count { get; set; }

    public float? Timestamp { get; set; }

    public float? X { get; set; }
    public float? Y { get; set; }
    public float? Z { get; set; }
    

}