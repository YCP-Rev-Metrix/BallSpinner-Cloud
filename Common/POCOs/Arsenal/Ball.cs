using System;
using System.Numerics;

namespace Common.POCOs;
public class Ball : Poco
{
    public Ball(float weight, string? color)
    {
        Weight = weight;
        Color = color;
    }

    public float Weight { get; set; }
    public string? Color { get; set; }

}
