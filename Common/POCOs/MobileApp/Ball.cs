namespace Common.POCOs.MobileApp;

public class Ball : Poco
{
    public Ball() { }

    public Ball(int? id, int userId, string name, string weight, string coreType)
    {
        Id = id;
        UserId = userId;
        Name = name;
        Weight = weight;
        CoreType = coreType;
    }

    public int? Id { get; set; }
    public int? UserId { get; set; }
    public string? Name { get; set; }
    public string? Weight { get; set; }
    public string? CoreType { get; set; }
}
