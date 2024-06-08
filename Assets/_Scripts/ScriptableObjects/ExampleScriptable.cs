using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ExampleScriptable", fileName = "New Scriptable")]
public class ExampleScriptable : ScriptableObject
{
    [TextArea(2,6)]
    [SerializeField] string s_name = "Example Name";

    public string GetName()
    {
        return s_name;
    }
    
}
