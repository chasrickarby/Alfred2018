///-----------------------------------------------------------------
/// <summary>
/// Single calendar event details.
/// </summary>
///-----------------------------------------------------------------

using System;

namespace LibExchange
{
    public class ExchangeEvent
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}