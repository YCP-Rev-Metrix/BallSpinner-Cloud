namespace Common.POCOs.MobileApp;

public class Ball : Poco
{
    public Ball() { }

    public int? Id { get; set; }
    public int? MobileID { get; set; }
    public int? UserId { get; set; }
    public string? Name { get; set; }
    public string? BallMFG { get; set; }
    public string? BallMFGName { get; set; }
    public string? SerialNumber { get; set; }
    public int? Weight { get; set; }
    public string? Core { get; set; }
    public string? ColorString { get; set; }
    public string? Coverstock { get; set; }
    public string? Comment { get; set; }
    public bool Enabled { get; set; }
}
