using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public abstract class Menu<T> : Singleton<T> where T : Singleton<T>
{
    public bool startClosed = true;
    [HideInInspector] public Canvas canvas;
    public new void Awake()
    {
        base.Awake();
        canvas = GetComponent<Canvas>();
        if(startClosed )
        canvas.enabled = false;
    }
    public void Open()
        { canvas.enabled=true;}
    public void Close()
        { canvas.enabled = false; }
    public void Toggle()
    { if(canvas.enabled)
            Close();
    else
            Open();
                }
}
