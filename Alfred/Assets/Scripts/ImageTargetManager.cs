using System.Collections;
using UnityEngine;
using Vuforia;
using System;
using System.Text.RegularExpressions;

public class ImageTargetManager : MonoBehaviour {

    public RestExchangeClient ExchangeClient;
    public ImageTargetBehaviour[] ImageTargetsInScene;
    public StringReference[] ConferenceRoomAddresses;
    public StringReference[] ConferenceRoomNames;

    public GameEvent FetchData;

	// Use this for initialization

	IEnumerator Start () {
        // First clear any existing room address and name data
        for (var i = 0; i < ConferenceRoomAddresses.Length; i++)
        {
            ConferenceRoomAddresses[i].Value = "";
            ConferenceRoomNames[i].Value = "";
        }

        var cd = new CoroutineWithData(this, GetRoomInfo());
        yield return cd.coroutine;
        ExchangeRoomInfoCollection roomInfo = (ExchangeRoomInfoCollection)cd.result;
        Array.Sort(roomInfo.RoomInfoCollection);
        for (var i = 0; i < roomInfo.RoomInfoCollection.Length; i++)
        {
            if (i == ConferenceRoomAddresses.Length || i == ConferenceRoomNames.Length)
            {
                throw new System.Exception("Didn't have enough string references for the number of conference rooms found.");
            }
            ConferenceRoomAddresses[i].Value = roomInfo.RoomInfoCollection[i].Address;
            ConferenceRoomNames[i].Value = Regex.Match(roomInfo.RoomInfoCollection[i].Name, @"/(.*)").Groups[1].Value;
        }
        FetchData.Raise();
	}

    private IEnumerator GetRoomInfo()
    {
        yield return ExchangeClient.GetAllAvailableRoomNames();
    }

    private IEnumerator GetRoomWithEventData(string roomAddress)
    {
        yield return ExchangeClient.GetRoomDetailsByRoomAddress(roomAddress);
    }
}
