using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameLookAtCameraTutorial : MonoBehaviour
{
    void Update()
    {
        if (GameManagerTutorial.instance.mainPlayer.cam == null)
        {
            return;
        }
        transform.LookAt(transform.position + GameManagerTutorial.instance.mainPlayer.cam.transform.forward);
    }
}
