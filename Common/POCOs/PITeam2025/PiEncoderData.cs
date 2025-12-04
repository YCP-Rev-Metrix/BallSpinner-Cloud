namespace Common.POCOs.PITeam2025;

public class PiEncoderData : Poco
{
    public PiEncoderData() {}
    
    public PiEncoderData(int id, int sessionId, float time, float pulses, int motorId)
    {
        Id = id;
        SessionId = sessionId;
        Time = time;
        Pulses = pulses;
        MotorId = motorId;
    }
    
    public int Id { get; set; }
    public int SessionId { get; set; }
    public float Time { get; set; }
    public float Pulses { get; set; }
    public int MotorId { get; set; }
    public int ReplayIteration { get; set; }
}