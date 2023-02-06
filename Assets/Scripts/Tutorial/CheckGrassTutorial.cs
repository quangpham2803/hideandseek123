using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrassTutorial : MonoBehaviour
{
    public bool inGrass;
    public PlayerSetupTutorial player;
    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameManagerTutorial.instance.state != GameManagerTutorial.GameState.Playing)
            return;
        //if (other.CompareTag("Player") && !other.GetComponent<PlayerSetupTutorial>().invisible)
        //{
        //    if (other.GetComponent<PlayerSetupTutorial>().player != player.player && !other.GetComponent<PlayerSetupTutorial>().isTranform)
        //    {
        //        player.rpc.SetDefaultMaterial(other.GetComponent<PlayerSetupTutorial>());
        //        if (player.playerInRange.Count == 0)
        //        {
        //            player.playerInRange.Add(other.GetComponent<PlayerSetupTutorial>());
        //        }
        //        else
        //        {
        //            try
        //            {
        //                foreach (PlayerSetupTutorial item in player.playerInRange)
        //                {
        //                    if (item == other.GetComponent<PlayerSetupTutorial>())
        //                    {
        //                        continue;
        //                    }
        //                    player.playerInRange.Add(other.GetComponent<PlayerSetupTutorial>());
        //                }
        //            }
        //            catch
        //            {

        //            }
        //        }
        //    }
        //}
        if (other.tag == "Grass" && !player.isBot)
        {
            player.rpc.SetGrassMaterial(other.gameObject, true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (GameManagerTutorial.instance.state != GameManagerTutorial.GameState.Playing)
            return;
        if (other.CompareTag("Player") && !other.GetComponent<PlayerSetupTutorial>().invisible)
        {
            if (other.GetComponent<PlayerSetupTutorial>().player != player.player)
            {
                //player.playerInRange.Remove(other.GetComponent<PlayerSetupTutorial>());
                if (other.GetComponent<PlayerSetupTutorial>().inGrass)
                {
                    //player.rpc.InvisibleOtherTeam(other.GetComponent<PlayerSetupTutorial>().invisibleOtherTeamMaterial, other.GetComponent<PlayerSetupTutorial>());
                }
            }

        }
        if (other.tag == "Grass" && !player.isBot)
        {
            //if (!player.photonView.IsMine)
            //{
            //    return;
            //}
            player.rpc.SetGrassMaterial(other.gameObject, false);
        }

    }
}
