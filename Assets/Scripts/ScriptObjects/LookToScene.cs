using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToScene : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.mainPlayer.cam == null)
        {
            return;
        }
        transform.LookAt(transform.position + GameManager.instance.mainPlayer.cam.transform.rotation * -Vector3.back,
        GameManager.instance.mainPlayer.cam.transform.rotation * -Vector3.down);
    }
}
