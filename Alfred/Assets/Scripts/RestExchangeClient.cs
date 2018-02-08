using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class RestExchangeClient : MonoBehaviour, IExchangeClient
{
    public const string ServerUrl = "http://alfred-hack.eastus.cloudapp.azure.com/RestServer/api";
    public const string RoomInformantionPath = "/rooms";
    public const int ServerTimeoutSec = 10;
    public RoomDetails[] RoomDetails;
    public RoomEvent[] RoomEvents;
    public GameEvent FetchData;
    public StringReference NameOfLastAccess;
    public GameEvent DataReadyToDisplay;

    public bool CreateAppointment(string roomAddress, DateTime startTime, DateTime endTime, string Subject)
    {
        throw new NotImplementedException();
    }

    public void GetAllAvailableRoomNames()
    {
        var url = ServerUrl + RoomInformantionPath;
        Debug.Log(string.Format("GetAllNamesCalled. URL: {0}", url));
        StartCoroutine(GetAllNamesCoroutine(url));
    }

    public void GetRoomDetailsByRoomAddress(string roomAddress, RoomDetails roomDetails)
    {
        var url = ServerUrl + RoomInformantionPath + "/" + Regex.Match(roomAddress, @"(.*)@ptc.com").Groups[1].Value;
        Debug.Log(url);
        StartCoroutine(GetRoomByAddressCoroutine(url, roomDetails));
    }

    private IEnumerator GetAllNamesCoroutine(string url)
    {
        Debug.Log("Made it here");
        using (WWW www = new WWW(url))
        {
            yield return www;
            var roomInfoCollection = ExchangeRoomInfoCollection.CreateFromJSON(www.text);
            Array.Sort(roomInfoCollection.RoomInfoCollection);
            for (var i = 0; i < roomInfoCollection.RoomInfoCollection.Length; i++)
            {
                if (i == RoomDetails.Length)
                {
                    throw new System.Exception(
                        "Didn't have enough string references for the number of conference rooms found.");
                }
                RoomDetails[i].Address = roomInfoCollection.RoomInfoCollection[i].Address;
                RoomDetails[i].Name =
                    Regex.Match(roomInfoCollection.RoomInfoCollection[i].Name, @"/(.*)").Groups[1].Value;
            }
        }
        FetchData.Raise();
    }

    private IEnumerator GetRoomByAddressCoroutine(string url, RoomDetails roomDetails)
    {
        using (WWW www = new WWW(url))
        {
            yield return www;
            // Parse and cache events
            roomDetails.TicksAtLastUpdate = DateTime.Now.Ticks;
            var roomWithEventData = RoomWithEventData.CreateFromJSON(www.text);
            NameOfLastAccess.Value = roomDetails.Name;
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
