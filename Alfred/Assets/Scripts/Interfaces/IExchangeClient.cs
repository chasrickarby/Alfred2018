using System;
using System.Collections.Generic;
using UnityEngine;

public interface IExchangeClient {
    void GetAllAvailableRoomNames();

    void GetRoomDetailsByRoomAddress(string roomAddress, RoomDetails roomDetails);

    bool CreateAppointment(string roomAddress, DateTime startTime, DateTime endTime, string Subject);
}
