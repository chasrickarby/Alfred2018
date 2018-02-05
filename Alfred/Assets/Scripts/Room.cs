using UnityEngine;

[System.Serializable]
public struct Room {

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

    public static Room CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Room>(jsonString);
    }
}
