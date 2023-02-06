using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScene : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.instance == null)
        {
            if (GameManagerTutorial.instance.mainPlayer.cam == null)
            {
                return;
            }
            transform.LookAt(transform.position + GameManagerTutorial.instance.mainPlayer.cam.transform.rotation * -Vector3.back,
            GameManagerTutorial.instance.mainPlayer.cam.transform.rotation * -Vector3.down);
        }
        else
        {
            if (GameManager.instance.mainPlayer.cam == null)
            {
                return;
            }
            transform.LookAt(transform.position + GameManager.instance.mainPlayer.cam.transform.rotation * -Vector3.back,
            GameManager.instance.mainPlayer.cam.transform.rotation * -Vector3.down);
        }
    }
}
