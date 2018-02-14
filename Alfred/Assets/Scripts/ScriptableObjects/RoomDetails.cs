using System;
using UnityEngine;

[CreateAssetMenu]
public class RoomDetails : ResettableScriptableObject
{
    public string Address;
    public string Name;
    public long TicksAtLastUpdate;
    public int Temperature;
    public int Humidity;
    public bool Motion;

    public override void Reset()
    {
        Address = "";
        Name = "";
        TicksAtLastUpdate = 0L;
    }
}
