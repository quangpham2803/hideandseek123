using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    // Start is called before the first frame update
    public Image fill;
   
    private float  distance,startDistance;
    public GameObject player, finish;

    private void Start()
    {
        
        startDistance = Vector3.Distance(player.transform.position, finish.transform.position);

    }
    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, finish.transform.position);

        if ( player.transform.position.x < finish.transform.position.x|| player.transform.position.y < finish.transform.position.y|| player.transform.position.z < finish.transform.position.z)
        {
            fill.fillAmount = 1 - (distance/startDistance);
        }
    }

}
