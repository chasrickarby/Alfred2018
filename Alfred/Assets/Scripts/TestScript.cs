using UnityEngine;
using LibExchange;
using System;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using UnityEngine.UI;

public class TestScript : MonoBehaviour {

    public string RoomName = "POR-cr6@ptc.com";
    public Text message;

	// Use this for initialization
	void Start () {
        var exchange = new Exchange();
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        Debug.Log("Retrieving event information for " + RoomName + "...");
        var roomInformation = exchange.GetAppointmentsByRoomAddress(RoomName, DateTime.Today, DateTime.Today.AddDays(1));
        for (var i =0; i < roomInformation.Events.Count; i++)
        {
            if (i == 0)
            {
                message.text = roomInformation.Events[i].Subject;
            }
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(String.Format("Event {0} details:", i));
            stringBuilder.AppendLine(String.Format("Subject: {0}, Start: {1}, End: {2}, Id: {3}", roomInformation.Events[i].Subject, roomInformation.Events[i].Start, roomInformation.Events[i].End, roomInformation.Events[i].Id));
            Debug.Log(stringBuilder.ToString());
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                        Debug.Log("chain failed");
                    }
                }
            }
        }
        if (isOk)
        {
            Debug.Log("chain passed");
        }
        return isOk;
    }
}
