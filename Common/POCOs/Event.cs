using System;
using System.Numerics;

namespace Common.POCOs;
public class Event : POCO
{
    public Event(int user_id, string event_type )
    {
        User_id = user_id;
        Event_type = event_type;
    }
    public int User_id { get; set; }
    public string Event_type { get; set; }

}

