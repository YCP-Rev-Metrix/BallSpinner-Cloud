namespace Common.POCOs;
///<Summary>
/// Placeholder (fill in this section later)
///</Summary>
public class ShotList: Poco
{
    public ShotList() { }

    public ShotList(List<Shot>? shots)
    {
        Shots = shots;
    }

    

    public List<Shot>? Shots{ get; set; }

}