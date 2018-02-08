using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;

public class RestExchangeClient : MonoBehaviour, IExchangeClient
{
    public const string ServerUrl = "http://alfred-hack.eastus.cloudapp.azure.com/RestServer/api";
    public const string RoomInformantionPath = "/rooms";
    public const int ServerTimeoutSec = 5;

    public bool CreateAppointment(string roomAddress, DateTime startTime, DateTime endTime, string Subject)
    {
        throw new NotImplementedException();
    }

    public ExchangeRoomInfoCollection GetAllAvailableRoomNames()
    {
        var url = ServerUrl + RoomInformantionPath;
        using (WWW www = new WWW(url))
        {
            WaitForServerResponse(www);
            var roomInfoCollection = ExchangeRoomInfoCollection.CreateFromJSON(www.text);
            return roomInfoCollection;
        }
    }

    public RoomWithEventData GetRoomDetailsByRoomAddress(string roomAddress)
    {
        var url = ServerUrl + RoomInformantionPath + "/" + Regex.Match(roomAddress, @"(.*)@ptc.com").Groups[1].Value;
        using (WWW www = new WWW(url))
        {
            WaitForServerResponse(www);
            var roomWithEventData = RoomWithEventData.CreateFromJSON(www.text);
            return roomWithEventData;
        }
    }

    private void WaitForServerResponse(WWW www)
    {
        var stopWatch = Stopwatch.StartNew();
        while (!www.isDone)
        {
            if (stopWatch.Elapsed.TotalSeconds > ServerTimeoutSec)
            {
                throw new TimeoutException(String.Format("Rest Server failed to respond within {0} seconds.", ServerTimeoutSec));
            }
        }
    }
}
