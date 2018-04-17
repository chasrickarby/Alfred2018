using UnityEngine;

public class ImageTargetManager : MonoBehaviour {

    public RestExchangeClient ExchangeClient;
    public RoomDetails[] RoomDetails;
    
	// Use this for initialization
	void Awake ()
    {
        // First clear any existing room address and name data
        Debug.Log("Start called on ImageTargetManager");
	    foreach (var roomDetail in RoomDetails)
	    {
            roomDetail.Reset();
	    }

	    ExchangeClient.GetAllAvailableRoomNames();
	}
}
