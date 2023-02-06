using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BandageManager : MonoBehaviourPun
{
    public static BandageManager manager;
    public List<Bandage> bandageList;
    public float timeDuration;
    public float speed;
    private void Awake()
    {
        manager = this;
    }
    public void IEWaitBandage(int idBandage)
    {
        photonView.RPC("IEWaitCmd", RpcTarget.AllBuffered, idBandage);
    }
    [PunRPC]
    void IEWaitCmd(int idBandage)
    {
        StartCoroutine(IEWait(idBandage));
    }
    IEnumerator IEWait(int idBandage)
    {
        foreach(Bandage b in bandageList)
        {
            yield return new WaitForSeconds(timeDuration);
            if (!b.hitObject)
            {
                if (!b.owner.isBot && b.owner.photonView.IsMine)
                {
                    TableManager.manager.BanDage(b.owner.photonView.ViewID);
                }
                else if (b.owner.isBot && PhotonNetwork.IsMasterClient)
                {
                    TableManager.manager.BanDage(b.owner.photonView.ViewID);
                }
            }
        }
    }
    public void StopToss(int idBandage)
    {
        photonView.RPC("StopTossCmd", RpcTarget.AllBuffered, idBandage);
    }
    [PunRPC]
    void StopTossCmd(int idBandage)
    {
        foreach(Bandage b in bandageList)
        {
            if(b.id == idBandage)
            {
                foreach (LineRender item in GameManager.instance.lineRenderer)
                {
                    if (item.owner == b.owner)
                    {
                        item.gameObject.SetActive(false);
                    }
                }
                b.owner.pAnimator.SetBool("Toss", false);
                b.owner.speedHook = 1f;
                b.gameObject.SetActive(false);
                break;
            }
        }
    }
    public void IsHitted(int idBandage, Vector3 bandagePosition, Vector3 playerPosition)
    {
        photonView.RPC("IsHittedCmd", RpcTarget.AllBuffered, idBandage, bandagePosition, playerPosition);
    }
    [PunRPC]
    void IsHittedCmd(int idBandage, Vector3 bandagePosition, Vector3 playerPosition)
    {
        foreach (Bandage b in bandageList)
        {
            if (b.id == idBandage)
            {
                b.transform.position = bandagePosition;
                b.owner.transform.position = playerPosition;
                TableManager.manager.BanDage(b.owner.photonView.ViewID);
                break;
            }
        }
    }
    public void IsStunPlayer(int idBandage, int idPlayer, Vector3 position)
    {
        photonView.RPC("IsStunPlayerCmd", RpcTarget.AllBuffered, idBandage, idPlayer, position);
    }
    [PunRPC]
    void IsStunPlayerCmd(int idBandage, int idPlayer, Vector3 position)
    {
        foreach (Bandage b in bandageList)
        {
            if (b.id == idBandage)
            {
                b.transform.position = position;
                b.hitObject = true;
                PlayerSetup player = new PlayerSetup();
                foreach (PlayerSetup p in GameManager.instance.players)
                {
                    if(p.photonView.ViewID == idPlayer)
                    {
                        player = p;
                        break;
                    }
                }                 
                b.effect.SetActive(true);
                b.effect.transform.position = transform.position;
                if (player != b.owner)
                {
                    StunPlayer(idPlayer);
                    break;
                }
                break;
            }
        }
    }
    void StunPlayer(int idPlayer)
    {
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            if (p.photonView.ViewID == idPlayer)
            {
                p.StartCoroutine(p.StunTime(3f));
                break;
            }
        }
    }
    public void HitWall(int idBandage, Vector3 position)
    {
        photonView.RPC("HitWallCmd", RpcTarget.AllBuffered, idBandage, position);
    }
    [PunRPC]
    void HitWallCmd(int idBandage, Vector3 position)
    {
        foreach (Bandage b in bandageList)
        {
            if (b.id == idBandage)
            {
                b.transform.position = position;
                b.effect.SetActive(true);
                b.effect.transform.position = position;
                b.hitObject = true;
                break;
            }
        }
    }
}
