namespace Common.POCOs.MobileApp;

public class Game : Poco
{
    public Game() { }

    public Game(int? id, int mobileID, string gameNumber, string lanes, int score, int win, int startingLane, int sessionId, int teamResult, int individualResult)
    {
        Id = id;
        MobileID = mobileID;
        GameNumber = gameNumber;
        Lanes = lanes;
        Score = score;
        Win = win;
        StartingLane = startingLane;
        SessionId = sessionId;
        TeamResult = teamResult;
        IndividualResult = individualResult;
    }

    public int? Id { get; set; }
    public int? MobileID { get; set; }
    public string? GameNumber { get; set; }
    public string? Lanes { get; set; }
    public int? Score { get; set; }
    public int? Win { get; set; }
    public int? StartingLane { get; set; }
    public int? SessionId { get; set; }
    public int? TeamResult { get; set; }
    public int? IndividualResult { get; set; }
}
