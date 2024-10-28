using System;

namespace Common.POCOs;

public class Shot : POCO
{
    // Parameterless constructor needed for model binding
    public Shot() { }

    // Parameterized constructor can still exist and be used elsewhere
    public Shot(int shot_id, int? session_id, int? game_id, int? frame_id, int? ball_id, int? video_id,
                DateTime time, int? shot_number, int? shot_number_ot, int? lane_number, int? pocket_hit, 
                string? count, string? pins, float ddx, float ddy, float ddz, float x_position, float y_position, float z_position)
    {
        Shot_id = shot_id;
        Session_id = session_id;
        Game_id = game_id;
        Frame_id = frame_id;
        Ball_id = ball_id;
        Video_id = video_id;
        Time = time;
        Shot_number = shot_number;
        Shot_number_ot = shot_number_ot;
        Lane_Number = lane_number;
        Pocket_hit = pocket_hit;
        Count = count;
        Pins = pins;
        Ddx = ddx;
        Ddy = ddy;
        Ddz = ddz;
        X_position = x_position;
        Y_position = y_position;
        Z_position = z_position;
    }

    // Properties
    public int? Shot_id { get; set; }
    public int? Session_id { get; set; }
    public int? Game_id { get; set; }
    public int? Frame_id { get; set; }
    public int? Ball_id { get; set; }
    public int? Video_id { get; set; }
    public int? Shot_number { get; set; }
    public int? Shot_number_ot { get; set; }
    public DateTime Time { get; set; }
    public int? Lane_Number { get; set; }
    public int? Pocket_hit { get; set; }
    public string? Count { get; set; }
    public string? Pins { get; set; }
    public float Ddx { get; set; }
    public float Ddy { get; set; }
    public float Ddz { get; set; }
    public float X_position { get; set; }
    public float Y_position { get; set; }
    public float Z_position { get; set; }
}
