using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Coin : MonoBehaviourPunCallbacks
{

    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Pocket.pocket.AddMoney();
            //GetComponent<PhotonView>().RPC("DestroyCoinCmd", RpcTarget.All);
            Destroy(gameObject);
        }
    }

    //[PunRPC]
    //void DestroyCoinCmd()
    //{
    //    Destroy(gameObject);
    //}
}
