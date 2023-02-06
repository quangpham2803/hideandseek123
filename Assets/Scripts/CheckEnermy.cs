using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnermy : MonoBehaviour
{
    public PlayerSetup player;

    private void OnTriggerStay(Collider other)
    {
        if (GameManager.instance.state == GameManager.GameState.Playing)
        {
            if (other.CompareTag("Player"))
            {
                PlayerSetup pS = other.GetComponent<PlayerSetup>();
                if (pS.player == GameManager.HideOrSeek.Hide && !pS.dead && player.player != GameManager.HideOrSeek.Hide && !player.isbeStun && !player.isFollow && !pS.invisible)
                {
                    player.attack = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (GameManager.instance.state == GameManager.GameState.Playing)
        {
            if (other.CompareTag("Player")
            && other.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Hide
            && player.player != GameManager.HideOrSeek.Hide)
            {
                player.attack = false;
            }
        }
    }
}
