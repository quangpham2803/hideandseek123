using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TrashObjectManager : MonoBehaviourPunCallbacks
{
    public static TrashObjectManager manager;
    public List<TrashObject> trashObjectList;
    public float time;
    private void Awake()
    {
        manager = this;
    }
    // Start is called before the first frame update
    public void Start()
    {
        StartCoroutine(WaitToPlay());
    }
    IEnumerator WaitToPlay()
    {
        yield return new WaitUntil(isPlaying);
        int i = 0;
        foreach (TrashObject can in trashObjectList)
        {
            can.id = i;
            i++;
        }
    }
    bool isPlaying()
    {
        return GameManager.instance.state == GameManager.GameState.Playing;
    }
    public void HitTrashCan(int id, int way)
    {
        photonView.RPC("HitTrashCanCmd", RpcTarget.AllBuffered, id, way);
    }
    [PunRPC]
    public void HitTrashCanCmd(int id, int way)
    {
        foreach(TrashObject can in trashObjectList)
        {
            if(can.id == id)
            {
                can.isFall = true;
                can.trashBody.SetActive(false);
                can.trashFall[way].SetActive(true);
                can.trashWater[way].SetActive(true);
                StartCoroutine(DisAbleTrashCan(can.trashFall[way]));
                break;
            }
        }
    }
    IEnumerator DisAbleTrashCan(GameObject can)
    {
        yield return new WaitForSeconds(time);
        can.SetActive(false);
    }
    public void Slide(int idTrash, int idWater, int idPlayer, int timeUse)
    {
        photonView.RPC("slideCmd", RpcTarget.AllBuffered, idTrash, idWater, idPlayer, timeUse);
    }
    [PunRPC]
    void slideCmd(int idTrash, int idWater, int idPlayer, int timeUse)
    {
        foreach (TrashObject can in trashObjectList)
        {
            if (can.id == idTrash)
            {
                if(timeUse == 0)
                {
                    can.trashWater[idWater].SetActive(false);
                }
                else
                {
                    can.trashWater[idWater].GetComponent<TrashWater>().timeUse = timeUse; 
                }
                break;
            }
        }
        foreach(PlayerSetup p in GameManager.instance.players)
        {
            if(p.photonView.ViewID == idPlayer)
            {
                p.slide.StartCoroutine(p.slide.SlideTime());
                break;
            }
        }
    }
}
