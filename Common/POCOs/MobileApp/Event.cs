namespace Common.POCOs.MobileApp;

public class Event : Poco
{
    public Event() {}

    public Event(int? id, int userId, string name, string type, string location, int average, int? stats, string standings)
    {
        Id = id;
        UserId = userId;
        Name = name;
        Type = type;
        Location = location;
        Average = average;
        Stats = stats;
        Standings = standings;
    }
    
    public int? Id { get; set; }
    public int UserId { get; set; }
    
    public string? Name { get; set; }
    
    public string? Type { get; set; }
    
    public string? Location { get; set; }
    
    public int? Average { get; set; }
    
    public int? Stats { get; set; }
    
    public string? Standings { get; set; }
}