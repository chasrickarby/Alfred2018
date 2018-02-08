using System;
using UnityEngine;

[CreateAssetMenu]
public class RoomDetails : ResettableScriptableObject
{
    public string Address;
    public string Name;
    public long TicksAtLastUpdate;

    public override void Reset()
    {
        Address = "";
        Name = "";
        TicksAtLastUpdate = 0L;
    }
}
