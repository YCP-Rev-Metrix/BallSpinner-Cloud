namespace Common.POCOs.PITeam2025;

public class PiHeatData : Poco
{
    public PiHeatData() {}
    
    public PiHeatData(int id, int sessionId, float time, float value, int motorId)
    {
        Id = id;
        SessionId = sessionId;
        Time = time;
        Value = value;
        MotorId = motorId;
    }
    
    public int Id { get; set; }
    public int SessionId { get; set; }
    public float Time { get; set; }
    public float Value { get; set; }
    public int MotorId { get; set; }
}