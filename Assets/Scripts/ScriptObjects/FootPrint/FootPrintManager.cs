using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class FootPrintManager : MonoBehaviourPunCallbacks
{
    public static FootPrintManager manager;
    public List<FootPrint> notUse;
    public List<FootPrint> inUse;
    public float timePrinter;
    private void Awake()
    {
        manager = this;
    }
    public void SpawnFootPrint(int id)
    {
        photonView.RPC("SpawnFootPrintCmd", RpcTarget.AllBuffered, id);
    }
    [PunRPC]
    void SpawnFootPrintCmd(int id)
    {
        foreach(PlayerSetup p in GameManager.instance.players)
        {
            if(p.photonView.ViewID == id)
            {
                p.footPrinter.StartCoroutine(p.footPrinter.FootPrinter());
                break;
            }
        }
    }
    public void StopSpawnFootPrint(int id)
    {
        photonView.RPC("StopSpawnFootPrintCmd", RpcTarget.AllBuffered, id);
    }
    [PunRPC]
    void StopSpawnFootPrintCmd(int id)
    {
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            if (p.photonView.ViewID == id)
            {
                p.footPrinter.isPrinting = false;
                break;
            }
        }
    }
}
