namespace Common.POCOs.MobileApp;

public class Session : Poco
{
    public Session() { }

    public Session(int? id, int mobileID, int sessionNumber, int establishmentId, int eventId, int dateTime, string teamOpponent, string individualOpponent, int score, int stats, int teamRecord, int individualRecord)
    {
        Id = id;
        MobileID = mobileID;
        SessionNumber = sessionNumber;
        EstablishmentId = establishmentId;
        EventId = eventId;
        DateTime = dateTime;
        TeamOpponent = teamOpponent;
        IndividualOpponent = individualOpponent;
        Score = score;
        Stats = stats;
        TeamRecord = teamRecord;
        IndividualRecord = individualRecord;
    }

    public int? Id { get; set; }
    public int? MobileID { get; set; }
    public int? SessionNumber { get; set; }
    public int? EstablishmentId { get; set; }
    public int? EventId { get; set; }
    public int? DateTime { get; set; }
    public string? TeamOpponent { get; set; }
    public string? IndividualOpponent { get; set; }
    public int? Score { get; set; }
    public int? Stats { get; set; }
    public int? TeamRecord { get; set; }
    public int? IndividualRecord { get; set; }
}
