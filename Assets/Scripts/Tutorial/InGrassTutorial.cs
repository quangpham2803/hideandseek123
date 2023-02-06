using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGrassTutorial : MonoBehaviour
{
    public List<GameObject> player = new List<GameObject>();
    private int temp;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            temp = 0;
            other.GetComponent<PlayerSetupTutorial>().inGrass = true;
            other.GetComponent<PlayerSetupTutorial>().grass = gameObject;
            foreach (GameObject _p in player)
            {
                if (other.gameObject == _p)
                {
                    temp++;
                }
            }
            if (temp == 0) player.Add(other.gameObject);
            foreach (PlayerSetupTutorial p in GameManagerTutorial.instance.players)
            {
                p.changePlayerInGrass = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.Remove(other.gameObject);
            other.GetComponent<PlayerSetupTutorial>().inGrass = false;
            foreach (PlayerSetupTutorial p in GameManagerTutorial.instance.players)
            {
                p.changePlayerInGrass = true;
            }
            other.GetComponent<PlayerSetupTutorial>().grass = null;
        }
    }
}
