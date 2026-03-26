namespace Common.POCOs.MobileApp;

public class Shot : Poco
{
    public Shot() { }

    public Shot(int? id, int mobileID, int type, int smartDotId, int sessionId, int ballId, int frameId, int shotNumber, int leaveType, string side, string position, string comment)
    {
        Id = id;
        MobileID = mobileID;
        Type = type;
        SmartDotId = smartDotId;
        SessionId = sessionId;
        BallId = ballId;
        FrameId = frameId;
        ShotNumber = shotNumber;
        LeaveType = leaveType;
        Side = side;
        Position = position;
        Comment = comment;
    }

    public int? Id { get; set; }
    public int? MobileID { get; set; }
    public int? Type { get; set; }
    public int? SmartDotId { get; set; }
    public int? SessionId { get; set; }
    public int? BallId { get; set; }
    public int? FrameId { get; set; }
    public int? ShotNumber { get; set; }
    public int? LeaveType { get; set; }
    public string? Side { get; set; }
    public string? Position { get; set; }
    public string? Comment { get; set; }
}
