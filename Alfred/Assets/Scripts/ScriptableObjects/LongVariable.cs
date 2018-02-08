using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LongVariable : ResettableScriptableObject
{
    public long Value;

    public override void Reset()
    {
        Value = 0L;
    }
}
