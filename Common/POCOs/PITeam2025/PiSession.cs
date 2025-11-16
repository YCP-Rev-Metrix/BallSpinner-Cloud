namespace Common.POCOs.PITeam2025;

public class PiSession : Poco
{
    public PiSession() {}
    
    public PiSession(int? id, DateTime? timeStamp, string? name, bool? isShotMode)
    {
        Id = id;
        TimeStamp = timeStamp;
        Name = name;
        IsShotMode = isShotMode;
    }
    
    public int? Id { get; set; }
    
    public DateTime? TimeStamp { get; set; }
    
    public string? Name { get; set; }
    
    public bool? IsShotMode { get; set; }
    
}