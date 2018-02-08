using UnityEngine;

[System.Serializable]
public struct ExchangeRoomInfoCollection
{
    public RoomWithEventData[] RoomInfoCollection;
        
    public static ExchangeRoomInfoCollection CreateFromJSON(string jsonString)
    {
        var newString = FixJson(jsonString);
        try
        {
            return JsonUtility.FromJson<ExchangeRoomInfoCollection>(newString);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw new System.Exception("Exception while parsing ExchangeRoomInfoCollection object. See inner exception for details.", e);
        }
    }

    private static string FixJson(string jsonString)
    {
        // JSON returned by rest server includes a un-named array of objects. This isn't supported
        // by the JsonUtility parser.
        var newString = "{\"RoomInfoCollection\":" + jsonString + "}";
        return newString;
    }
}
