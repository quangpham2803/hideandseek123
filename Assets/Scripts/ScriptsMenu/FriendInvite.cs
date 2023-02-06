using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class FriendInvite : MonoBehaviourPunCallbacks
{
    public TMPro.TextMeshProUGUI playerName;
    public string roomName;
    public string regionId;
    public void SetUpMess(string name,string room, string id)
    {
        playerName.text = name;
        roomName = room;
        regionId = id;
    }
    public void Accept()
    {
        if(InventoryManager.inventory.energy >= 20)
        {
            if (PhotonNetwork.CloudRegion != regionId.Remove(0, 2))
            {
                PhotonNetwork.ConnectToRegion(regionId.Remove(0, 2));
            }
            Debug.Log(roomName.Remove(0, 2));
            Launcher2.Instance.charMenu.SetActive(false);
            Launcher2.Instance.LoadingMenu.SetActive(true);
            Launcher2.Instance.JoinRoomByName(roomName.Remove(0, 2));
            Destroy(gameObject);
        }
        else
        {
            Launcher2.Instance.AlertPopup("NOT ENOUGHT ENERGY!");
            Destroy(gameObject);
        }
    }
    public void Not()
    {
        Destroy(gameObject);
    }
    private void Start()
    {
        StartCoroutine(waitToClosePopup());
    }
    IEnumerator waitToClosePopup()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
