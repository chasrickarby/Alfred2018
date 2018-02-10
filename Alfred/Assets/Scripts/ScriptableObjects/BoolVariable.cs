using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoolVariable : ResettableScriptableObject
{
    public bool Value;
    public bool DefaultValue = false;
    public override void Reset()
    {
        Value = DefaultValue;
    }
}
