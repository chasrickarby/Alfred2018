using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RoomDataDisplayer : MonoBehaviour
{
    public RoomDetails RoomDetails;
    public Color HighValueColor;
    public Color GoodValueColor;
    public Color LowValueColor;
    public int LowTempThreshold;
    public int HighTempThreshold;
    public int LowHumidityThreshold;
    public int HighHumidityThreshold;
    public RoomEvent[] RoomeEvents;
    public StringReference AddressOfLastAccess;
    public Text RoomNameText;
    public Text RoomCurrentStatusText;
    public Color RoomBookedColor;
    public Color RoomOpenColor;
    public Text RoomNextAvailableText;
    public Text RoomTempText;
    public Text RoomHumidityText;
    public Image[] MeetingImages;
    public Color[] MeetingColors;
    public Color NoMeetingColor;
    public Text[] MeetingOrganizerTexts;
    public StringReference ErrorMessage;
    public GameEvent ShowDataCanvas;
    public GameEvent ShowErrorCanvas;
    public int FirstHourOfDay = 8;
    public int LastHourOfDay = 5;

    public void DisplayData()
    {
        if (!RoomDetails.Address.Equals(AddressOfLastAccess.Value))
        {
            // Not our data, return.
            return;
        }

        // We need to pair down our Room Event array to only include Room Events that have data in them
        var eventList = new List<RoomEvent>();
        foreach (var roomEvent in RoomeEvents)
        {
            if (!roomEvent.Id.Equals(""))
            {
                eventList.Add(roomEvent);
            }
            else
            {
                // We've reached an empty RoomEvent, no need to continue.
                break;
            }
        }

        // First update info fields
        // Name
        RoomNameText.text = RoomDetails.Name;

        // Get our schedule and colors
        var meetingColorDefinition = new List<Color>();
        
        var schedule = GetUsedBlocks(eventList, out meetingColorDefinition);

        // Current Status
        if (MeetingInProgress(schedule))
        {
            RoomCurrentStatusText.text = "BOOKED";
            RoomCurrentStatusText.color = RoomBookedColor;
            var nextAvailable = GetNextAvailableStartTime(schedule);
            RoomNextAvailableText.text = nextAvailable.ToString("hh:mm tt");
            }
        else
        {
            RoomCurrentStatusText.text = "OPEN";
            RoomCurrentStatusText.color = RoomOpenColor;
            RoomNextAvailableText.text = "Now";
        }

        // Temp and humidity
        SetTempTextColor();
        SetHumidityTextColor();   
        RoomTempText.text = RoomDetails.Temperature.ToString() + " F";
        RoomHumidityText.text = RoomDetails.Humidity.ToString() + "% RH";

        // Fill in Meeting graph.
        var startIdx = FirstHourOfDay * 4;
        var endIdx = LastHourOfDay * 4;
        for (var i = startIdx; i < endIdx; i++)
        {
            MeetingImages[i - (FirstHourOfDay * 4)].color = meetingColorDefinition[i];
        }

        // Fill in the Organizer list
        // clear first
        foreach (var meetingOrganizerText in MeetingOrganizerTexts)
        {
            meetingOrganizerText.text = "";
        }
        foreach (var roomEvent in eventList)
        {
            var idx = GetIndexFromTime(roomEvent.StartTime);
            MeetingOrganizerTexts[idx - (FirstHourOfDay * 4)].text = roomEvent.Subject + " (" + roomEvent.StartTime.ToString("hh:mm") + " - " + roomEvent.EndTime.ToString("hh:mm") + ")";
        }

        // We're done loading!
        ShowDataCanvas.Raise();
    }

    public void ServerCommunicationError()
    {
        ErrorMessage.Value = "Server communication error...";
        ShowErrorCanvas.Raise();
    }

    private void SetHumidityTextColor()
    {
        if (RoomDetails.Humidity <= LowHumidityThreshold || RoomDetails.Humidity >= HighHumidityThreshold)
        {
            RoomHumidityText.color = HighValueColor;
        }
        else
        {
            RoomHumidityText.color = GoodValueColor;
        }
    }

    private void SetTempTextColor()
    {
        if (RoomDetails.Temperature <= LowTempThreshold)
        {
            RoomTempText.color = LowValueColor;
        }
        else if (RoomDetails.Temperature >= HighTempThreshold)
        {
            RoomTempText.color = HighValueColor;
        }
        else
        {
            RoomTempText.color = GoodValueColor;
        }
    }

    private List<bool> GetUsedBlocks(List<RoomEvent> eventList, out List<Color> meetingColors)
    {
        var result = new List<bool>();
        meetingColors = new List<Color>();
        for (int i = 0; i < 96; i++)
        {
            meetingColors.Add(NoMeetingColor);
        }
        for (int i = 0; i < 96; i++)
        {
            result.Add(false);
        }

        Debug.Log("Generating schedule for room");
        var meetingIdx = 0;
        foreach (var roomeEvent in eventList)
        {
            var startIdx = GetIndexFromTime(roomeEvent.StartTime);
            var endIdx = GetIndexFromTime(roomeEvent.EndTime);
            for (int i = startIdx; i < endIdx; i++)
            {
                result[i] = true;
                meetingColors[i] = MeetingColors[meetingIdx];
            }
            meetingIdx++;
            if (meetingIdx == MeetingColors.Length)
            {
                meetingIdx = 0;
            }
        }
        return result;
    }

    private int GetIndexFromTime(DateTime time)
    {
        // idx = (hr *idx/hr))+(min/min/idx)
        return (time.Hour * 4) + (time.Minute / 15);
    }

    private DateTime GetTimeFromIndex(int idx)
    {
        DateTime result = DateTime.Today;
        int hoursToAdd = idx / 4;
        int minToAdd = (idx % 4) * 15;
        //Debug.Log(string.Format("idx: {0}, hoursToAdd: {1}, minutesToAdd: {2}", idx, hoursToAdd, minToAdd));
        
        return result.AddHours(hoursToAdd).AddMinutes(minToAdd);
    }

    private DateTime GetNextAvailableStartTime(List<bool> schedule)
    {
        var startIdx = GetIndexFromTime(DateTime.Now);
        for (var i = startIdx; i < schedule.Count; i++)
        {
            if (!schedule[i])
            {
                return GetTimeFromIndex(i);
            }
        }
        throw new Exception("Could not find any available times...");
    }

    private bool MeetingInProgress(List<bool> schedule)
    {
        var idx = GetIndexFromTime(DateTime.Now);
        return schedule[idx];
    }
}
