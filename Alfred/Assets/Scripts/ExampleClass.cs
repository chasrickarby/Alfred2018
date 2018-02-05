// Get the latest webcam shot from outside "Friday's" in Times Square
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ExampleClass : MonoBehaviour
{
    public string url = "http://localhost:51484/api/rooms/POR-cr6@ptc.com";
    public Text Organizer;
    public Text StartText;
    public Text End;

    IEnumerator Start()
    {
        using (WWW www = new WWW(url))
        {
            yield return www;
            var roomDetails = Room.CreateFromJSON(www.text);
            DateTime startTime = DateTime.Parse(roomDetails.Events[0].Start);
            Organizer.text = roomDetails.Events[0].Subject;
            End.text = roomDetails.Events[0].End;

        }
    }
}