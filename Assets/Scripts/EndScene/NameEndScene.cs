using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameEndScene : MonoBehaviour
{
    public Camera cam1, cam2;
    // Update is called once per frame
    void Update()
    {
        if(cam1.enabled == true)
        {
            transform.LookAt(transform.position + cam1.transform.forward);
        }
        if (cam2.enabled == true)
        {
            transform.LookAt(transform.position + cam2.transform.forward);
        }
    }
}
