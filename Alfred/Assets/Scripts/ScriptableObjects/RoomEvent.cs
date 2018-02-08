using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RoomEvent : ResettableScriptableObject
{
    public string Id;
    public string Subject;
    public DateTime StartTime;
    public DateTime EndTime;

    public override void Reset()
    {
        Id = string.Empty;
        Subject = string.Empty;
        StartTime = DateTime.MinValue;
        EndTime = DateTime.MinValue;
    }
}
