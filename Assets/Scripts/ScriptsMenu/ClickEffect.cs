using UnityEngine;
using System.Collections;
using UnityEngine.UI; //Required when using UI Elements.
using UnityEngine.EventSystems; // Required when using event data.

public class ClickEffect : MonoBehaviour, IPointerDownHandler
{
    public Transform effect;

    //Invoked when the mouse pointer goes down on a UI element.
    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log("Click");
        effect.position = data.position;
        effect.GetComponent<Animator>().Play("clickanimation");
    }
}
