using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    Vector3 startPosition;
    protected override void Start()
    {
        startPosition = background.transform.position;
        base.Start();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.transform.position = startPosition;
        base.OnPointerUp(eventData);
    }
}