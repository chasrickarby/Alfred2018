using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomDataDisplayer : MonoBehaviour
{
    public RoomDetails RoomDetails;
    public RoomEvent[] RoomeEvents;
    public Text EventCountText;
    public Text RoomNameText;

    public void DisplayData()
    {
        RoomNameText.text = RoomDetails.Name;
        var eventCount = 0;
        foreach (var roomeEvent in RoomeEvents)
        {
            Debug.Log(roomeEvent.Id);
            if (!roomeEvent.Id.Equals(""))
            {
                eventCount++;
            }
        }
        Debug.Log(string.Format("EventCount: {0}", eventCount));
        EventCountText.text = eventCount.ToString();
    }
}
