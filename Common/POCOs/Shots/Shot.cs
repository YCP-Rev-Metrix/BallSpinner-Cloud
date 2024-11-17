namespace Common.POCOs;

///<Summary>
/// Placeholder (fill in this section later)
///</Summary>

public class Shot: Poco
{
    public SimulatedShot SimulatedShot { get; set; }
    public List<SampleData> Data { get; set; } = new List<SampleData>();

    public Shot(SimulatedShot simulatedShot)
    {
        SimulatedShot = simulatedShot;
    }
}
    