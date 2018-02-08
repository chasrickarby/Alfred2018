using UnityEngine;
using System;
using System.Text.RegularExpressions;

[System.Serializable]
public struct RoomWithEventData : IComparable {

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

    public static RoomWithEventData CreateFromJSON(string jsonString)
    {
        try
        {
            return JsonUtility.FromJson<RoomWithEventData>(jsonString);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw new System.Exception("Exception while parsing RoomWithEventData object. See inner exception for details.", e);
        }
        
    }

    // Want to always sort based on CR number in address. example address: POR-cr1@ptc.com
    // TODO: should throw exception if we fail to match indicating an unusual address format was given.
    public int CompareTo(object obj)
    {
        if (obj == null) return 1;
        RoomWithEventData other = (RoomWithEventData)obj;
        var myInt = int.Parse(Regex.Match(this.Address, @"cr(\d+)@").Groups[1].Value);
        var otherInt = int.Parse(Regex.Match(other.Address, @"cr(\d+)@").Groups[1].Value);
        return myInt.CompareTo(otherInt);
    }
}
