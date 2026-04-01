namespace Common.POCOs.MobileApp;

public class Event : Poco
{
    public Event() { }

    public int? Id { get; set; }
    public int? MobileID { get; set; }
    public int UserId { get; set; }
    public string? LongName { get; set; }
    public string? NickName { get; set; }
    public string? Type { get; set; }
    public string? Location { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? WeekDay { get; set; }
    public string? StartTime { get; set; }
    public int NumGamesPerSession { get; set; }
    public int? Average { get; set; }
    public string? Schedule { get; set; }
    public int? Stats { get; set; }
    public string? Standings { get; set; }
    public bool Enabled { get; set; }
}
