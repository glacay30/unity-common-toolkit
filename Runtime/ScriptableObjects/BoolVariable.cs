using UnityEngine;

[CreateAssetMenu]
public class BoolVariable : ScriptableObject
{

#if UNITY_EDITOR
    [TextArea]
    public string DeveloperDescription = "";
#endif
    public bool InitialValue;
    public bool Value;

    private void OnEnable()
    {
        Value = InitialValue;
    }

    public void SetValue(bool value)
    {
        Value = value;
    }

    public void SetValue(BoolVariable value)
    {
        Value = value.Value;
    }
}