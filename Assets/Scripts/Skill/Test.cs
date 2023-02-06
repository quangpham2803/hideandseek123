using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject player;
    private void Update()
    {
        if (player!=null)
        {
            Debug.Log(Vector3.Distance(transform.position, player.transform.position));
        }
    }
}
