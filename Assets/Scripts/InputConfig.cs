using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeysBindings", menuName = "Config/Binding Config", order = 1)]
public class InputConfig : ScriptableObject
{
    public KeysBindings[] KeysBindings;
}

[Serializable]
public class KeysBindings
{
    public List<KeyCode> keys;
    public Tips tip;
}
