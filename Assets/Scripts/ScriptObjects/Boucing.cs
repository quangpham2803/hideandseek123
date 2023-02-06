using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boucing : MonoBehaviour
{
    public float flyUpSpeed;
    public float timeFly;
    private float currentTime = 0;
    private GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.gameObject;
            currentTime = timeFly;
        }
    }
    private void Update()
    {
        if(currentTime > 0 && player != null)
        {
            player.GetComponent<CharacterController>().enabled = false;
            currentTime -= Time.deltaTime;
            Vector3 tm = player.transform.position;
            tm.y += flyUpSpeed * Time.deltaTime;
            tm.x -= (flyUpSpeed/3) * Time.deltaTime;
            player.transform.position = tm;
        }
        else if(player != null)
        {
            player.GetComponent<CharacterController>().enabled = true;
            player = null;
            currentTime = 0;
        }
    }
}
