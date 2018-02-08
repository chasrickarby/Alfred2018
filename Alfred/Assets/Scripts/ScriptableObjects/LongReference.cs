using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LongReference
{

    public bool UseConstant = true;
    public long ConstantValue;
    public LongVariable Variable;

    public long Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set { if (!UseConstant) Variable.Value = value; }
    }
}
