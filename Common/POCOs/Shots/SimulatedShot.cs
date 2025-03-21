using Common.POCOs.Shots;

namespace Common.POCOs;

///<Summary>
/// Placeholder (fill in this section later)
///</Summary>

public class SimulatedShot
{
    public ShotInfo? shotinfo { get; set; }
    public List<SampleData?> data { get; set; }

    public Ball ball { get; set; }
    public SmartDotInfo? sensorInfo { get; set; }
}