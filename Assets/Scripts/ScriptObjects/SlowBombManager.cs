using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SlowBombManager : MonoBehaviourPunCallbacks
{
    public static SlowBombManager manager;
    public List<SlowBomb> slowBombList;
    public List<SlowBomb> inUseList;
    public void Awake()
    {
        manager = this;
    }
    private void Start()
    {
        int i = 0;
        foreach(SlowBomb bomb in slowBombList)
        {
            bomb.id = i;
            i++;
        }
    }
    public void AddBomb(PlayerSetup owner, Vector3 position)
    {
        photonView.RPC("AddBombCmd", RpcTarget.AllBuffered, owner.photonView.ViewID, position,slowBombList[0].id);
    }
    [PunRPC]
    void AddBombCmd(int id, Vector3 position, int bombId)
    {
        foreach(PlayerSetup p in GameManager.instance.players)
        {
            if(p.photonView.ViewID == id)
            {
                foreach(SlowBomb bomb in slowBombList)
                {
                    if(bomb.id == bombId)
                    {
                        bomb.owner = p;
                        bomb.gameObject.SetActive(true);
                        bomb.transform.position = position;
                        inUseList.Add(bomb);
                        slowBombList.Remove(bomb);
                        break;
                    }                    
                }
                break;
            }
        }
    }
    public void AddBombBack(int id)
    {
        photonView.RPC("AddBombBackCmd", RpcTarget.AllBuffered, id);
    }
    [PunRPC]
    void AddBombBackCmd(int id)
    {
        foreach(SlowBomb bomb in inUseList)
        {
            if(bomb.id == id)
            {
                slowBombList.Add(bomb);
                inUseList.Remove(bomb);
                bomb.gameObject.SetActive(false);
                break;
            }
        }
    }
}
