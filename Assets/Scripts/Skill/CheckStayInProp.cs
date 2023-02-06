using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckStayInProp : MonoBehaviour
{
    public bool isStay;
    // Start is called before the first frame update
    void Start()
    {
        isStay = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("prop")|| other.CompareTag("table") || other.CompareTag("Player") || other.CompareTag("wall"))
        {
            isStay = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("prop") || other.CompareTag("table") || other.CompareTag("Player") || other.CompareTag("wall"))
        {
            isStay = false;
        }
    }
}
