using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class IntReference
{

    public bool UseConstant = true;
    public int ConstantValue;
    public IntVariable Variable;

    public int Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set { if (!UseConstant) Variable.Value = value; }
    }
}
