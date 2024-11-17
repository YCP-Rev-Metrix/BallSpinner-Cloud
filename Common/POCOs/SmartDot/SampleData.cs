namespace Common.POCOs;
///<Summary>
/// Placeholder (fill in this section later)
///</Summary>
public class SampleData: Poco
{
    public SampleData() { }

    public SampleData(string? type, int? count, double? logtime, double? x, double? y, double? z, double? w)
    {
        Type = type;
        Count = count;
        Logtime = logtime;
        X = x;
        Y = y;
        Z = z;
        W = w;
    }
    public string? Type {get; set;}
    
    public int? Count { get; set; }

    public double? Logtime { get; set; }

    public double? X { get; set; }
    public double? Y { get; set; }
    public double? Z { get; set; }
    public double? W { get; set; }
    

}