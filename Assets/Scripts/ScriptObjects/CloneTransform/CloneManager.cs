using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CloneManager : MonoBehaviourPun
{
    public static CloneManager manager;
    public List<CloneTransformPlayer> cloneList;
    public float cloneDisappearTime;
    private void Awake()
    {
        manager = this;
    }
    public void BackToHost(int idClone)
    {
        photonView.RPC("BackToHostCmd", RpcTarget.AllBuffered, idClone);
    }
    [PunRPC]
    void BackToHostCmd(int idClone)
    {
        foreach (CloneTransformPlayer clone in cloneList)
        {
            if(clone.id == idClone)
            {
                clone.currentTime = 0;
                clone.transform.SetParent(clone.skillHost.transform);
                clone.transform.position = clone.skillHost.transform.position;
                clone.gameObject.SetActive(false);
                break;
            }

        }
    }
    public void exploClone(int idClone, int idPlayer, Vector3 position)
    {
        photonView.RPC("exploCloneCmd", RpcTarget.AllBuffered, idClone, idPlayer, position);
    }
    [PunRPC]
    void exploCloneCmd(int idClone, int idPlayer, Vector3 position)
    {
        StunPlayer(idPlayer);
        StartCoroutine(explo(idClone,position));
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
    IEnumerator explo(int idClone, Vector3 position)
    {
        foreach(CloneTransformPlayer clone in cloneList)
        {
            if(clone.id == idClone)
            {
                clone.transform.position = position;
                clone.exploEffect.SetActive(true);
                clone.isExplo = true;
                clone.movement.isMove = false;
                yield return new WaitForSeconds(1f);
                clone.transform.SetParent(clone.skillHost.transform);
                clone.transform.position = clone.skillHost.transform.position;
                clone.gameObject.SetActive(false);
                break;
            }
        }
    }
}
