namespace Common.POCOs.PITeam2025;

public class PiSession : Poco
{
    public PiSession() {}
    
    public PiSession(int? id, DateTime? timeStamp, string? name, bool? isShotMode, string? spin_Instruction_Points, string? tilt_Instruction_Points, string? angle_Instruction_Points)
    {
        Id = id;
        TimeStamp = timeStamp;
        Name = name;
        IsShotMode = isShotMode;
        Spin_Instruction_Points = spin_Instruction_Points;
        Tilt_Instruction_Points = tilt_Instruction_Points;
        Angle_Instruction_Points = angle_Instruction_Points;
    }
    
    public int? Id { get; set; }
    
    public DateTime? TimeStamp { get; set; }
    
    public string? Name { get; set; }
    
    public bool? IsShotMode { get; set; }

    public string? Spin_Instruction_Points { get; set; }

    public string? Tilt_Instruction_Points { get; set; }

    public string? Angle_Instruction_Points { get; set; }

}