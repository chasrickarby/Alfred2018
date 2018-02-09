using System;
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
        Id = "";
        Subject = "";
        StartTime = DateTime.MinValue;
        EndTime = DateTime.MaxValue;
    }
}
