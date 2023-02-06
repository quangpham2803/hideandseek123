using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tele : MonoBehaviour
{
    public Transform telePosition;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(TeleTime(other.gameObject));
        }
    }
    IEnumerator TeleTime(GameObject player)
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = telePosition.position;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<CharacterController>().enabled = true;
    }
}
