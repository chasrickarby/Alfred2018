using System.Collections;
using UnityEngine;
using System;

public class RoomGuiController : MonoBehaviour {
    public RestExchangeClient ExchangeClient;
    public LongReference ticksAtLastUpdate;
    public int RefreshThresholdSec = 10;
    public GameEvent DataReadyToDisplay;
    public StringReference RoomAddress;
    public StringReference RoomName;
    public RoomEvent[] RoomEvents;

    private RoomWithEventData roomInfo;

    public void FetchData()
    {
        if ((DateTime.Now.Ticks - ticksAtLastUpdate.Value) / 10000000 > RefreshThresholdSec)
        {
            // Its been long enough for us to update the room data.
            // First reset any data we already had.
            foreach (var roomEvent in RoomEvents)
            {
                roomEvent.Reset();
            }
            StartCoroutine(GetRoomData());
        }
        else
        {
            // No need to refresh. 
            DataReadyToDisplay.Raise();
        }
    }

    private IEnumerator GetRoomData()
    {
        var cd = new CoroutineWithData(this, SendRequestToServer());
        yield return cd.coroutine;
        roomInfo = (RoomWithEventData)cd.result;

        // Parse and cache events
        for (int i = 0; i < roomInfo.Events.Length; i++)
        {
            RoomEvents[i].Id = roomInfo.Events[i].Id;
            RoomEvents[i].Subject = roomInfo.Events[i].Subject;
            RoomEvents[i].StartTime = DateTime.Parse(roomInfo.Events[i].Start);
            RoomEvents[i].EndTime = DateTime.Parse(roomInfo.Events[i].End);
        }
        DataReadyToDisplay.Raise();
    }

    private IEnumerator SendRequestToServer()
    {
        yield return ExchangeClient.GetRoomDetailsByRoomAddress(RoomAddress.Value);
    }
}
