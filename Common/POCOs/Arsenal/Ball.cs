namespace Common.POCOs;
public class Ball : Poco
{

    public Ball(string? name, double? weight, double? hardness, string? coretype, double? diameter)
    {
        Name = name;
        Weight = weight;
        Hardness = hardness;
        CoreType = coretype;
        Diameter = diameter;
    }

    public string? Name { get; set; }
    public string? CoreType { get; set; }
    public double? Hardness { get; set; }

    public double? Weight { get; set; }

    public double? Diameter { get; set; }

}
