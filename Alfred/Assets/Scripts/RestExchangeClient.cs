using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class RestExchangeClient : MonoBehaviour, IExchangeClient
{
    public const string ServerUrl = "http://alfred-hack.eastus.cloudapp.azure.com/RestServer/api";
    public const string RoomInformantionPath = "/rooms";
    public const int ServerTimeoutSec = 10;
    public RoomDetails[] RoomDetails;
    public RoomEvent[] RoomEvents;
    public StringReference AddressOfLastAccess;
    public GameEvent DataReadyToDisplay;
    public StringReference OfficeLocation;

    public bool CreateAppointment(string roomAddress, DateTime startTime, DateTime endTime, string Subject)
    {
        throw new NotImplementedException();
    }

    public void GetAllAvailableRoomNames()
    {
        var url = ServerUrl + RoomInformantionPath;
        StartCoroutine(GetAllNamesCoroutine(url));
    }

    public void GetRoomDetailsByRoomAddress(string roomAddress, RoomDetails roomDetails)
    {
        var url = ServerUrl + RoomInformantionPath + "/" + Regex.Match(roomAddress, @"(.*)@ptc.com").Groups[1].Value;
        StartCoroutine(GetRoomByAddressCoroutine(url, roomDetails));
    }

    private IEnumerator GetAllNamesCoroutine(string url)
    {
        using (WWW www = new WWW(url))
        {
            yield return www;
            var roomInfoCollection = ExchangeRoomInfoCollection.CreateFromJSON(www.text);
            var filteredCollection = roomInfoCollection.RoomInfoCollection.Where(s => s.Address.StartsWith("POR")).ToList();
            var sortedList = filteredCollection.OrderBy(s => int.Parse(Regex.Match(s.Address, @"-cr(\d+)@").Groups[1].Value)).ToList();
            for (var i = 0; i < sortedList.Count; i++)
            {
                if (i == RoomDetails.Length)
                {
                    throw new System.Exception(
                        "Didn't have enough string references for the number of conference rooms found.");
                }
                RoomDetails[i].Address = sortedList[i].Address;
                RoomDetails[i].Name = Regex.Match(sortedList[i].Name, @"/(.*)").Groups[1].Value;
            }
        }
    }

    private IEnumerator GetRoomByAddressCoroutine(string url, RoomDetails roomDetails)
    {
        using (WWW www = new WWW(url))
        {
            yield return www;
            // Parse and cache events
            roomDetails.TicksAtLastUpdate = DateTime.Now.Ticks;
            var roomWithEventData = RoomWithEventData.CreateFromJSON(www.text);
            AddressOfLastAccess.Value = roomDetails.Address;
            for (int i = 0; i < roomWithEventData.Events.Length; i++)
            {
                RoomEvents[i].Id = roomWithEventData.Events[i].Id;
                RoomEvents[i].Subject = roomWithEventData.Events[i].Subject;
                RoomEvents[i].StartTime = DateTime.Parse(roomWithEventData.Events[i].Start);
                RoomEvents[i].EndTime = DateTime.Parse(roomWithEventData.Events[i].End);
            }
            DataReadyToDisplay.Raise();
        }
    }
}
