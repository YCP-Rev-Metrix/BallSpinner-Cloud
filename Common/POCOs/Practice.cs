using System;
using System.Numerics;

namespace Common.POCOs;
public class Practice : POCO
{
    public Practice(int event_id)
    {
        Event_id = event_id;
    }
    public int Event_id { get; set; }

}

