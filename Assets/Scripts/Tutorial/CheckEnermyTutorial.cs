using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnermyTutorial : MonoBehaviour
{
    public PlayerSetupTutorial player;
    private void OnTriggerStay(Collider other)
    {
        if (GameManagerTutorial.instance.state == GameManagerTutorial.GameState.Playing)
        {
            if (other.CompareTag("Player") && other == GameManagerTutorial.instance.mainPlayer.gameManager)
            {
                Debug.Log("111");
            }
            if (other.CompareTag("Player") && other.GetComponent<PlayerSetupTutorial>().player == GameManagerTutorial.HideOrSeek.Hide
            && !other.GetComponent<PlayerSetupTutorial>().dead
            && player.player != GameManagerTutorial.HideOrSeek.Hide
            && !player.isbeStun && !player.isFollow
            && !other.GetComponent<PlayerSetupTutorial>().invisible)
            {
                Debug.Log(10);
                player.attack = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (GameManagerTutorial.instance.state == GameManagerTutorial.GameState.Playing)
        {
            if (other.CompareTag("Player")
            && other.GetComponent<PlayerSetupTutorial>().player == GameManagerTutorial.HideOrSeek.Hide
            && player.player != GameManagerTutorial.HideOrSeek.Hide)
            {
                player.attack = false;
            }
        }
    }
}
