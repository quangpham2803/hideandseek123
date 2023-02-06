using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class GoHome : MonoBehaviourPunCallbacks
{
    public void Home()
    {
        //if (PlayerPrefs.GetInt("RemoveAd") == 0)
        //{
        //    StatusPlayer.Instance.showAD = true;
        //}
        //StatusPlayer.Instance.showRate = true;
        PhotonNetwork.LeaveRoom();
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
    //public IEnumerator LeaveRoom()
    //{
    //    PhotonNetwork.LeaveRoom();
    //    PhotonNetwork.Disconnect();
    //    while (PhotonNetwork.InRoom)
    //    {
    //        yield return null;
    //    }
    //    SceneManager.LoadScene(0);
    //}

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
        base.OnLeftRoom();
    }
}
