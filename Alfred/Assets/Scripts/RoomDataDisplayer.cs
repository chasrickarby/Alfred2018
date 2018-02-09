using UnityEngine;
using UnityEngine.UI;

public class RoomDataDisplayer : MonoBehaviour
{
    public RoomDetails RoomDetails;
    public RoomEvent[] RoomeEvents;
    public StringReference NameOfLastAccess;
    public Text RoomNameText;

    public void DisplayData()
    {
        if (!RoomDetails.Name.Equals(NameOfLastAccess.Value))
        {
            // Not our data, return.
            return;
        }
        
        RoomNameText.text = RoomDetails.Name;
        var eventCount = 0;
        foreach (var roomeEvent in RoomeEvents)
        {
            if (!roomeEvent.Id.Equals(""))
            {
                eventCount++;
            }
        }
    }
}
