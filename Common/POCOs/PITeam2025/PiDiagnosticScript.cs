namespace Common.POCOs.PITeam2025;

public class PiDiagnosticScript : Poco
{
    public PiDiagnosticScript() {}
    
    public PiDiagnosticScript(int? id, int sessionId, float time, int motorId, float instruction)
    {
        Id = id;
        SessionId = sessionId;
        Time = time;
        MotorId = motorId;
        Instruction = instruction;
    }
    
    public int? Id { get; set; }
    
    public int SessionId { get; set; }
    
    public float Time { get; set; }
    
    public int MotorId { get; set; }
    
    public float Instruction { get; set; }
}