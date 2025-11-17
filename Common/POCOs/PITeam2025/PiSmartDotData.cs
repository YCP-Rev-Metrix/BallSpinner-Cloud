namespace Common.POCOs.PITeam2025;

public class PiSmartDotData : Poco
{
    public PiSmartDotData() {}

    public PiSmartDotData(int id, int sessionId, float time, int dataSelector,
        float xl_x, float xl_y, float xl_z,
        float gy_x, float gy_y, float gy_z,
        float mg_x, float mg_y, float mg_z,
        float lt)
    {
        Id = id;
        SessionId = sessionId;
        Time = time;
        DataSelector = dataSelector;
        XL_X = xl_x;
        XL_Y = xl_y;
        XL_Z = xl_z;
        GY_X = gy_x;
        GY_Y = gy_y;
        GY_Z = gy_z;
        MG_X = mg_x;
        MG_Y = mg_y;
        MG_Z = mg_z;
        LT = lt;
    }
    
    public int Id { get; set; }
    
    public int SessionId { get; set; }
    
    public float Time { get; set; }
    
    public float DataSelector { get; set; }
    
    // Acceleration Data XYZ
    public float XL_X { get; set; }
    public float XL_Y { get; set; }
    public float XL_Z { get; set; }
    
    // Gyroscope Data XYZ
    public float GY_X { get; set; }
    public float GY_Y { get; set; }
    public float GY_Z { get; set; }
    
    // Magnetometer Data XYZ
    public float MG_X { get; set; }
    public float MG_Y { get; set; }
    public float MG_Z { get; set; }
    
    public float LT { get; set; }
}