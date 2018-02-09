using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrackableEventHandler : DefaultTrackableEventHandler {
    public GameEvent FetchDataForRoom;

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();

        FetchDataForRoom.Raise();
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        
        // Do something here if you want to take action after the target is lost.
    }
}
