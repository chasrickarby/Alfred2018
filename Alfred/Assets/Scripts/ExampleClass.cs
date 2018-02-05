// Get the latest webcam shot from outside "Friday's" in Times Square
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ExampleClass : MonoBehaviour
{
    public string Url = "http://10.64.105.101:51484/api/rooms/POR-cr6@ptc.com";
    public Text OrganizerText;
    public Text StartText;
    public Text EndText;

    IEnumerator Start()
    {
        using (WWW www = new WWW(Url))
        {
            yield return www;
            Debug.Log(www.text);
            Debug.Log(www.error);
            var roomDetails = Room.CreateFromJSON(www.text);
            Debug.Log(roomDetails.Events.Length);
            var startTime = DateTime.Parse(roomDetails.Events[0].Start);
            var endTime = DateTime.Parse(roomDetails.Events[0].End);
            OrganizerText.text = roomDetails.Events[0].Subject;
            StartText.text = $"{startTime:hh:mm tt}";
            EndText.text = $"{endTime:hh:mm tt}";
            }
    }
}