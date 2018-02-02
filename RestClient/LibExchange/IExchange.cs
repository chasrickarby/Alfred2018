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
        IEnumerable<Room> GetAllRoomsDetails();
        IEnumerable<Room> GetAppointmentsAllRooms(DateTime start, DateTime end);
        Room GetAppointmentsByRoomAddress(string roomName, DateTime start, DateTime end);
    }
}