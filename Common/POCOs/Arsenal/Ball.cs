namespace Common.POCOs;
public class Ball : Poco
{
    public Ball(){}
    public Ball(string? name, double? diameter, double? weight, string? coretype)
    {
        Name = name;
        Diameter = diameter;
        Weight = weight;
        CoreType = coretype;
    }
    public string? Name { get; set; }
    public double? Diameter { get; set; }
    public string? CoreType { get; set; }

    public double? Weight { get; set; }

}
