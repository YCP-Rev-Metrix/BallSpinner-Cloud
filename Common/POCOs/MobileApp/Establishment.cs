namespace Common.POCOs.MobileApp;

public class Establishment : Poco
{
    public Establishment() { }

    public Establishment(int? id, string name, string lanes, string type, string location)
    {
        Id = id;
        Name = name;
        Lanes = lanes;
        Type = type;
        Location = location;
    }

    public int? Id { get; set; }
    public int? MobileID { get; set; }
    public string? Name { get; set; }
    public string? Lanes { get; set; }
    public string? Type { get; set; }
    public string? Location { get; set; }
}
