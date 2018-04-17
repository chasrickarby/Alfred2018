using System.Collections;
using UnityEngine;
using System;
using System.Diagnostics;

public class RoomDataFetcher : MonoBehaviour {
    public RestExchangeClient ExchangeClient;
    public int RefreshThresholdSec = 10;
    public GameEvent DataReadyToDisplay;
    public StringReference AddressOfLastAccess;
    public RoomEvent[] RoomEvents;
    public RoomDetails RoomDetails;
    public GameEvent ShowLoadingCanvas;
    public GameEvent ServerCommunicationError;

    public void FetchData()
    {
        if (!AddressOfLastAccess.Value.Equals(RoomDetails.Address) || (DateTime.Now.Ticks - RoomDetails.TicksAtLastUpdate) / 10000000 > RefreshThresholdSec)
        {
            // Its been long enough for us to update the room data or the data doesn't match our Id.
            // First reset any data we already had.
            foreach (var roomEvent in RoomEvents)
            {
                roomEvent.Reset();
            }
            ShowLoadingCanvas.Raise();

            // There's a chance that the room details object hasn't been updated yet if the user opens the App and quickly points at a target.
            // If this happens, we should check for room details for up to 5 seconds before reporting an error.
            StartCoroutine(CheckRoomThenCallServer());
            }
        else
        {
            // No need to refresh. 
            DataReadyToDisplay.Raise();
        }
    }

    private IEnumerator CheckRoomThenCallServer()
    {
        var stopWatch = Stopwatch.StartNew();
        while (RoomDetails.Address.Equals(""))
        {
            if (stopWatch.ElapsedMilliseconds > 10000)
            {
                ServerCommunicationError.Raise();
                yield break;
            }
            yield return new WaitForSeconds(.25f);
        }
        ExchangeClient.GetRoomDetailsByRoomAddress(RoomDetails.Address, RoomDetails);
    }
}
