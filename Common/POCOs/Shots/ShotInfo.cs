using System.Text.Json.Serialization;

namespace Common.POCOs;
///<Summary>
/// Placeholder (fill in this section later)
///</Summary>
public class ShotInfo
{
    public ShotInfo() { }

    public ShotInfo(string? name, Coordinate? bezierInitPoint, Coordinate? bezierInflectionPoint, Coordinate bezierFinalPoint, double timeStep)
    {
        Name = name;
        BezierInitPoint = bezierInitPoint;
        bezierInflectionPoint = bezierInflectionPoint;
        BezierFinalPoint = bezierFinalPoint;
        TimeStep = timeStep;
    }

    
    public string Name { get; set; }
    
    public Coordinate BezierInitPoint { get; set; }

    public Coordinate BezierInflectionPoint { get; set; }

    public Coordinate BezierFinalPoint { get; set; }

    public double TimeStep { get; set; }

    public int? DataCount { get; set; }

}