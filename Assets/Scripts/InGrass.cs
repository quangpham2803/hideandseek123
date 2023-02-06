using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class InGrass : MonoBehaviourPunCallbacks
{
    public List<GameObject> player=new List<GameObject>();
    private int temp;
    private void OnTriggerStay(Collider other)
    {
        if (GameManager.instance.state != GameManager.GameState.Playing)
            return;
        if (other.CompareTag("Player"))
        {
            temp = 0;
            PlayerSetup pS = other.GetComponent<PlayerSetup>();
            pS.inGrass = true;
            pS.grass = gameObject;
            foreach (GameObject _p in player)
            {
                if (other.gameObject == _p)
                {
                    temp++;
                }
            }
            if (pS.defaultMater && pS.playerInRange.Count == 0)
            {
                foreach (PlayerSetup p in GameManager.instance.players)
                {
                    p.changePlayerInGrass = true;
                }
            }
            if (temp == 0)
            {
                player.Add(other.gameObject);
            }

        }
        if (other.CompareTag("Summon"))
        {
            other.GetComponent<PlayerJoker>().inGrass = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (GameManager.instance.state != GameManager.GameState.Playing)
            return;
        if (other.CompareTag("Player"))
        {
            PlayerSetup pS = other.GetComponent<PlayerSetup>();
            player.Remove(other.gameObject);
            pS.inGrass = false;
            foreach (PlayerSetup p in GameManager.instance.players)
            {
                p.changePlayerInGrass = true;
            }
            pS.grass = null;
        }
        if (other.CompareTag("Summon"))
        {
            other.GetComponent<PlayerJoker>().inGrass = false;
            other.GetComponent<PlayerJoker>().SetDefaultMaterial();
        }
    }

}
