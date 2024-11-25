namespace Common.POCOs;
public class Ball : Poco
{

    public Ball(string? name, double? weight, double? hardness, string? coretype)
    {
        Name = name;
        Weight = weight;
        Hardness = hardness;
        CoreType = coretype;
    }

    public string? Name { get; set; }
    public string? CoreType { get; set; }
    public double? Hardness { get; set; }

    public double? Weight { get; set; }

}
