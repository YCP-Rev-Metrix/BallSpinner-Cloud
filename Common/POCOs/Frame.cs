using System;
using System.Numerics;

namespace Common.POCOs;
public class Frame : POCO
{
    public Frame(int game_id, int shot_number, int score)
    {
        Game_id = game_id;
        Shot_number = shot_number;
        Score = score;
    }
    public int Game_id { get; set; }

    public int Shot_number { get; set; }

    public int Score { get; set;}

}

