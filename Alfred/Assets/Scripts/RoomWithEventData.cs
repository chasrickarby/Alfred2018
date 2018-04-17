using UnityEngine;
using System;
using System.Text.RegularExpressions;

[System.Serializable]
public struct RoomWithEventData
{
    public string Name;
    public string Address;

    [System.Serializable]
    public struct Event
    {
        public string Id;
        public string Subject;
        public string Start;
        public string End;
    }
    public Event[] Events;

    public string LastUpdate;
    public int Temperature;
    public int Humidity;
    public bool Motion;


    public static RoomWithEventData CreateFromJSON(string jsonString)
    {
        try
        {
            return JsonUtility.FromJson<RoomWithEventData>(jsonString);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            throw new System.Exception("Exception while parsing RoomWithEventData object. See inner exception for details.", e);
        }
    }
}
