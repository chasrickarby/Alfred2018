///-----------------------------------------------------------------
/// <summary>
/// Operations available on Microsoft EWS (Exchange Web Service).
/// </summary>
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace LibExchange
{
    public interface IExchange
    {
        List<Event> LoadAppointments(DateTime start, DateTime end, string accountName = null);
    }
}