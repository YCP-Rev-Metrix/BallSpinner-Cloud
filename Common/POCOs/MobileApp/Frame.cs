namespace Common.POCOs.MobileApp;

public class Frame : Poco
{
    public Frame() {}

    public Frame(int? id, int? gameId, int? shotOne, int? shotTwo, int? frameNumber, int? lane, int? result)
    {
        Id = id;
        GameId = gameId;
        ShotOne = shotOne;
        ShotTwo = shotTwo;
        FrameNumber = frameNumber;
        Lane = lane;
        Result = result;
    }
    
    public int? Id { get; set; }
    public int? GameId { get; set; }
    
    public int? ShotOne { get; set; }
    
    public int? ShotTwo { get; set; }
    
    public int? FrameNumber { get; set; }
    
    public int? Lane { get; set; }
    
    public int? Result { get; set; }
}