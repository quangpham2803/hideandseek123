using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameLookAtCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.mainPlayer.cam == null)
        {
            return;
        }
        transform.LookAt(transform.position + GameManager.instance.mainPlayer.cam.transform.forward);
    }
}
