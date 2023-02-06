using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnRandomBox : MonoBehaviourPun
{
    public static SpawnRandomBox manager;
    public RandomBox boxItem;
    public DebuffBox debuffItem;
    public List<Transform> pointList;
    public int maxBuffBox;
    public int currentBuffBox;
    public int haveAdd;
    public List<RandomBox> boxList;
    public List<DebuffBox> debuffList;
    public void Awake()
    {
        manager = this;
        haveAdd = 0;
    }
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RandomSpawnPoint", RpcTarget.AllBuffered);
            currentBuffBox = 0;
            int idTm = 0;
            foreach (var i in pointList)
            {
                if (currentBuffBox < maxBuffBox)
                {
                    int tm = Random.Range(0, 3);                    
                    if (tm == 1)
                    {
                        int g = Random.Range(0, 2);
                        currentBuffBox++;
                        idTm++;
                        if(g == 0)
                        {
                            photonView.RPC("AddBox", RpcTarget.AllBuffered, i.position, idTm, currentBuffBox);
                        }
                        else
                        {
                            photonView.RPC("AddDebuff", RpcTarget.AllBuffered, i.position, idTm, currentBuffBox);
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }       
    }
    List<Transform> listTemp = new List<Transform>();
    [PunRPC]
    public void RandomSpawnPoint()
    {
        foreach (Transform item in GameManager.instance.hidePos)
        {
            listTemp.Add(item);
        }
        for (int i = 1; i <= GameManager.instance.hidePos.Count; i++)
        {
            if (i % 3 != 1)
            {
                listTemp.Remove(GameManager.instance.hidePos[i - 1]);
            }
        }
        GameManager.instance.hidePos = listTemp;
    }
    [PunRPC]
    public void AddBox(Vector3 position, int idTm, int numberBox)
    {       
        RandomBox box = Instantiate(boxItem.gameObject, position, Quaternion.identity).GetComponent<RandomBox>();
        box.id = idTm;
        currentBuffBox = numberBox;
        boxList.Add(box);
        box.transform.parent = this.transform;
    }
    [PunRPC]
    public void AddDebuff(Vector3 position, int idTm, int numberBox)
    {
        DebuffBox box = Instantiate(debuffItem.gameObject, position, Quaternion.identity).GetComponent<DebuffBox>();
        box.id = idTm;
        currentBuffBox = numberBox;
        debuffList.Add(box);
        box.transform.parent = this.transform;
    }
    public void UseBox()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("UseBoxCmd", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    void UseBoxCmd()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            currentBuffBox--;
            if(currentBuffBox <= 0)
            {
                StartCoroutine(GenerateBox());
            }
        }
    }
    IEnumerator GenerateBox()
    {
        yield return new WaitForSeconds(10f);
        int idTm = 0;
        foreach (var i in pointList)
        {
            if (currentBuffBox < maxBuffBox)
            {
                int tm = Random.Range(0, 3);
                if (tm == 1)
                {
                    currentBuffBox++;
                    idTm++;
                    photonView.RPC("AddBox", RpcTarget.AllBuffered, i.position, idTm, currentBuffBox);
                }
            }
            else
            {
                break;
            }
        }
    }
    //Buff box
    public void ConsumeBox(int idPlayer, int idBox)
    {
        int i = Random.Range(0, 3);
        photonView.RPC("ConsumeBoxCmd", RpcTarget.AllBuffered, idPlayer, idBox, i);
    }
    [PunRPC]
    void ConsumeBoxCmd(int idPlayer, int idBox, int i)
    {
        foreach(PlayerSetup player in GameManager.instance.players)
        {
            if(player.photonView.ViewID == idPlayer)
            {                
                player.ReciveBoxEffect(i);
                //currentBuffBox--;
                foreach (RandomBox box in boxList)
                {
                    if(box.id == idBox)
                    {
                        box.Cooldown();
                    }
                }
                //if (currentBuffBox <= 0)
                //{
                //    boxList.Clear();
                //    debuffList.Clear();
                //    StartCoroutine(GenerateBox());
                //}
            }
        }
    }
    //Debuff box
    public void ConsumeBoxDebuff(int idPlayer, int idBox)
    {
        int i = Random.Range(0, 5);
        photonView.RPC("ConsumeBoxDebuffCmd", RpcTarget.AllBuffered, idPlayer, idBox, i);
    }
    [PunRPC]
    void ConsumeBoxDebuffCmd(int idPlayer, int idBox, int i)
    {
        foreach (PlayerSetup player in GameManager.instance.players)
        {
            if (player.photonView.ViewID == idPlayer)
            {
                player.ReciveBoxDebuff(i);
                //currentBuffBox--;
                foreach (DebuffBox box in debuffList)
                {
                    if (box.id == idBox)
                    {
                        box.Cooldown();
                    }
                }
                //if (currentBuffBox <= 0)
                //{
                //    boxList.Clear();
                //    debuffList.Clear();
                //    StartCoroutine(GenerateBox());
                //}
            }
        }
    }
}
