using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IntVariable : ResettableScriptableObject {

    public int Value;

    public override void Reset()
    {
        Value = 0;
    }
}
