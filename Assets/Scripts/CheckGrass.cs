using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CheckGrass : MonoBehaviour
{
    public bool inGrass;
    public PlayerSetup player;
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.state != GameManager.GameState.Playing || !player.photonView.IsMine || player.isBot)
            return;
        if (other.tag == "Grass")
        {
            player.rpc.SetGrassMaterial(other.gameObject, true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (GameManager.instance.state != GameManager.GameState.Playing)
            return;
        if (other.CompareTag("Player"))
        {
            PlayerSetup pS = other.GetComponent<PlayerSetup>();
            if (!pS.invisible && pS != player && pS.player != player.player)
            {
                if (!pS.defaultMater && !player.isBot)
                {
                    player.rpc.SetDefaultMaterial(pS);
                }
                if (!pS.isTranform)
                {
                    if (!player.playerInRange.Contains(pS))
                    {
                        player.playerInRange.Add(pS);
                    }
                }
            }

        }
        if (other.CompareTag("Summon") && !player.isBot)
        {
            //if (!player.photonView.IsMine)
            //{
            //    return;
            //}
            PlayerJoker pJ = other.GetComponent<PlayerJoker>();
            if (pJ.inGrass)
            {
                if (player.player == GameManager.HideOrSeek.Seek)
                {
                    pJ.InvisibleTeam(other.GetComponent<PlayerStat>().invisibleMaterial);
                }
                else
                {
                    pJ.InvisibleOtherTeam(other.GetComponent<PlayerStat>().invisibleOtherTeamMaterial);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameManager.instance.state != GameManager.GameState.Playing )
            return;
        if (other.CompareTag("Player"))
        {
            PlayerSetup pS = other.GetComponent<PlayerSetup>();
            if (pS.player != player.player && !pS.invisible && pS != player)
            {
                player.playerInRange.Remove(pS);
                //if (other.GetComponent<PlayerSetup>().inGrass)
                //{
                //    player.rpc.InvisibleOtherTeam(other.GetComponent<PlayerSetup>().invisibleOtherTeamMaterial, other.GetComponent<PlayerSetup>());
                //}
                foreach (PlayerSetup p in GameManager.instance.players)
                {
                    p.changePlayerInGrass = true;
                }
            }
            
        }
        if (other.CompareTag("Summon") && !player.isBot)
        {
            //if (!player.photonView.IsMine)
            //{
            //    return;
            //}
            other.GetComponent<PlayerJoker>().SetDefaultMaterial();
        }

        if (other.tag == "Grass" && !player.isBot)
        {
            if (!player.photonView.IsMine)
            {
                return;
            }
            player.rpc.SetGrassMaterial(other.gameObject, false);
        }

    }
}
