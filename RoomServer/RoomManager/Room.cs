///-----------------------------------------------------------------
/// <summary>
/// Room information and all events.
/// </summary>
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace RoomManager
{
    public class Room
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Event> Events { get; set; } = new List<Event>();
        public DateTime LastUpdate { get; set; } = new DateTime();
        public double Temperature { get; set; } = 0;
        public double Humidity { get; set; } = 0;
    }
}