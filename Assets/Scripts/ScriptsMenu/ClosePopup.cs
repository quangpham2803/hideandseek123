using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePopup : MonoBehaviour
{
    public GameObject popUp;
    public void CloseClick()
    {
        popUp.SetActive(false);
    }
}
