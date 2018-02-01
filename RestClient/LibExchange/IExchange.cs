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
        List<ExchangeEvent> LoadAppointments(DateTime start, DateTime end, string accountName = null);
    }
}