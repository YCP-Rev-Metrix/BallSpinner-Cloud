using System;
using System.Numerics;

namespace Common.POCOs;
public class Game : POCO
{
    public Game(int session_id, int score)
    {
        Session_id = session_id;
        Score = score;
    }
    public int Session_id { get; set; }
    public int Score { get; set; }

}

