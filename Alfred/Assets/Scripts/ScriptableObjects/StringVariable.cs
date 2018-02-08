using UnityEngine;

[CreateAssetMenu]
public class StringVariable : ResettableScriptableObject
{
    public string Value;

    public override void Reset()
    {
        Value = "";
    }
}
