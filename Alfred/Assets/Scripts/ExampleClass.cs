// Get the latest webcam shot from outside "Friday's" in Times Square
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ExampleClass : MonoBehaviour
{
    public string Url = "http://alfred-hack.eastus.cloudapp.azure.com/RestServer/api/rooms/POR-cr6";
    public Text OrganizerText;
    public Text StartText;
    public Text EndText;

    IEnumerator Start()
    {
        using (WWW www = new WWW(Url))
        {
            yield return www;
            var roomDetails = RoomWithEventData.CreateFromJSON(www.text);
            var startTime = DateTime.Parse(roomDetails.Events[0].Start);
            var endTime = DateTime.Parse(roomDetails.Events[0].End);
            OrganizerText.text = roomDetails.Events[0].Subject;
            StartText.text = startTime.ToString("hh:mm tt");
            EndText.text = endTime.ToString("hh:mm tt");
            // EndText.text = $"{endTime.ToLocalTime():hh:mm tt}";
            }
    }
}