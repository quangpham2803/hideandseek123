using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaven : MonoBehaviour
{
    public GameObject owner;
    public float y;
    public Vector3 cameraOffset;
    public float Smoothfactor = 0.125f;
    void Update()
    {
        Vector3 newPos = owner.transform.position + cameraOffset;
        Vector3 smoothPosision = Vector3.Lerp(transform.position, newPos, Smoothfactor * Time.deltaTime);
        transform.position = new Vector3(smoothPosision.x, y, smoothPosision.z);
        transform.LookAt(owner.transform);
    }
}
