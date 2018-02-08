using System;
using System.Collections.Generic;
using UnityEngine;

public interface IExchangeClient {
    ExchangeRoomInfoCollection GetAllAvailableRoomNames();

    RoomWithEventData GetRoomDetailsByRoomAddress(string roomAddress);

    bool CreateAppointment(string roomAddress, DateTime startTime, DateTime endTime, string Subject);
}
