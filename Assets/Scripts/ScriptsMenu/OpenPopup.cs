using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPopup : MonoBehaviour
{
    public GameObject popup;
    public void OnpenClick()
    {
        popup.SetActive(true);
    }
}
