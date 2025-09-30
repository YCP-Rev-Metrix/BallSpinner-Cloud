using System.Text.Json.Serialization;

namespace Common.POCOs;
///<Summary>
/// Placeholder (fill in this section later)
///</Summary>
public class Coordinate
{
    public Coordinate(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public double x { get; set; }

    public double y { get; set;  }
}
