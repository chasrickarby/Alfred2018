using System.Collections;
using UnityEngine;
using System;

public class RoomDataFetcher : MonoBehaviour {
    public RestExchangeClient ExchangeClient;
    public int RefreshThresholdSec = 10;
    public GameEvent DataReadyToDisplay;
    public StringReference NameOfLastAccess;
    public RoomEvent[] RoomEvents;
    public RoomDetails RoomDetails;
    public Canvas LiveDataCanvas;
    public Canvas LoadingCanvas;

    private RoomWithEventData roomInfo;

    public void FetchData()
    {
        if (!NameOfLastAccess.Value.Equals(RoomDetails.Address) || (DateTime.Now.Ticks - RoomDetails.TicksAtLastUpdate) / 10000000 > RefreshThresholdSec)
        {
            // Its been long enough for us to update the room data or the data doesn't match our Id.
            // First reset any data we already had.
            foreach (var roomEvent in RoomEvents)
            {
                roomEvent.Reset();
            }
            LiveDataCanvas.enabled = false;
            LoadingCanvas.enabled = true;

            ExchangeClient.GetRoomDetailsByRoomAddress(RoomDetails.Address, RoomDetails);
            }
        else
        {
            // No need to refresh. 
            DataReadyToDisplay.Raise();
        }
    }
}
