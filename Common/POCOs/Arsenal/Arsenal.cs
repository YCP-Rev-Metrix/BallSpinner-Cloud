using Common.POCOs;

namespace RevMetrix.BallSpinner.BackEnd.Common.POCOs;
///<Summary>
/// Placeholder (fill in this section later)
///</Summary>
public class Arsenal: Poco
{
    public Arsenal(int userid, int ballid)
    {
        UserId = userid;
        Ballid = ballid;
    }

    // TODO create Poco
    public int ArsenalId { get; set; }

    public int UserId { get; set; }
    public int Ballid { get; set; }
    /*
    public UserIdentification User { get; set; }

    public ICollection<Ball> Balls { get; set; }
    */


}