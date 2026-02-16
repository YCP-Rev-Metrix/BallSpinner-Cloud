namespace Common.POCOs.PITeam2025;

public class PiShot : Poco
{
    public PiShot() {}
    
    public PiShot(int? id, int sessionId, float time, float rpm, float angleDegrees, float tiltDegrees)
    {
        Id = id ?? 0;
        SessionId = sessionId;
        Time = time;
        Rpm = rpm;
        AngleDegrees = angleDegrees;
        TiltDegrees = tiltDegrees;
    }
    
    public int Id { get; set; }
    
    public int SessionId { get; set; }
    
    public float Time { get; set; }
    
    public float Rpm { get; set; }
    
    public float AngleDegrees { get; set; }
    
    public float TiltDegrees { get; set; }
}