using System;
using System.Numerics;

namespace Common.POCOs;
public class Session : POCO
{
    public Session(
                //int user_id,
                int? leauge_id,
                int? tournament_id,
                int? practice_id,
                DateTime time,
                string location,
                int total_games,
                int total_frames)
    {
        Leauge_id = leauge_id;
        Tournament_id = tournament_id;
        Practice_id = practice_id;
        Time = time;
        //Lane_Number = lane_number;
        Location = location;
        Total_Games = total_games;
        Total_Frames = total_frames;
    }

    public int? Leauge_id { get; set; }
    public int? Tournament_id { get; set; }

    public int? Practice_id { get; set; }
    public DateTime Time { get; set; }

    public string Location { get; set; }
    public int Total_Games { get; set; }
    public int Total_Frames { get; set; }


}
