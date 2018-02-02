///-----------------------------------------------------------------
/// <summary>
/// Room information and all events.
/// </summary>
///-----------------------------------------------------------------

using System.Collections.Generic;

namespace LibExchange
{
    public class Room
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Event> Events { get; set; } = new List<Event>();
    }
}