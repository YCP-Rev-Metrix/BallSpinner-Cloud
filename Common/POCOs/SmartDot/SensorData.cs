namespace Common.POCOs;
///<Summary>
/// Placeholder (fill in this section later)
///</Summary>
public class SensorData: Poco
{
    public SensorData() { }

    public SensorData(int? sensorType, int? count, float? timestamp, float? x, float? y, float? z)
    {
        SensorType = sensorType;
        Count = count;
        TimeStamp = timestamp;
        X = x;
        Y = y;
        Z = z;
    }
        
    public int? SensorType { get; set; }

    public int? Count { get; set; }

    public float? TimeStamp { get; set; }

    public float? X { get; set; }
    public float? Y { get; set; }
    public float? Z { get; set; }
    

}