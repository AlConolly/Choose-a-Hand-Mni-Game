using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
///<summary>
///Makes buttons expand slightly when hovered over with your mouse
///</summary>
public class ButtonScaler : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Vector3 hoverScale = new Vector3(1.1f,1.1f,1f);
    private Vector3 startScale;
    public float desiredDuration = .2f;
    private float elapsedTime;
    private Vector3 previousScale;
    private Coroutine startPointer;
    private Coroutine endPointer;
    void Start()
    {
        startScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        startPointer = StartCoroutine(ScaleUp());
    }
    public void OnPointerExit(PointerEventData data)
    {
        endPointer = StartCoroutine(ScaleDown());
    }
    public IEnumerator ScaleUp()
    {
        if(endPointer != null) StopCoroutine(endPointer);
        elapsedTime = 0f;

        while(transform.localScale != hoverScale) {
        elapsedTime += Time.unscaledDeltaTime;
        float percentageComplete = elapsedTime/desiredDuration;

        transform.localScale = Vector3.Lerp(startScale, hoverScale, percentageComplete);
        yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator ScaleDown()
    {
        if (startPointer != null) StopCoroutine(startPointer);
        previousScale = transform.localScale;
        elapsedTime = 0f;

        while(transform.localScale != startScale) {
        elapsedTime += Time.unscaledDeltaTime;
        float percentageComplete = elapsedTime/desiredDuration;

        transform.localScale = Vector3.Lerp(previousScale, startScale, percentageComplete);
        yield return new WaitForEndOfFrame();
        }
    }
}
