namespace Common.POCOs.PITeam2025;

public class PiHeatData : Poco
{
    public PiHeatData() {}
    
    public PiHeatData(int id, int sessionId, float time, float value, int motorId, int replayIteration)
    {
        Id = id;
        SessionId = sessionId;
        Time = time;
        Value = value;
        MotorId = motorId;
        ReplayIteration = replayIteration;
    }
    
    public int Id { get; set; }
    public int SessionId { get; set; }
    public float Time { get; set; }
    public float Value { get; set; }
    public int MotorId { get; set; }
    public int ReplayIteration { get; set; }
}