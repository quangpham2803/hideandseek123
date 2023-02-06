using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class RandomMapPrefabs : MonoBehaviourPun
{
    #region Private Fields

    [SerializeField]
    private GameObject[] mapRandomArray;
    #endregion
    public Transform[] point;
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach(Transform p in point)
            {
                //#Random number in array of map
                int randomInt = Random.Range(0, mapRandomArray.Length);
                photonView.RPC("GenerateMap", RpcTarget.AllBuffered, mapRandomArray[randomInt].GetComponent<AddPointToButtonManager>().id, p.position);
            }
        }            
    }
    [PunRPC]
    void GenerateMap(int id, Vector3 position)
    {
        foreach(GameObject map in mapRandomArray)
        {
            if(map.GetComponent<AddPointToButtonManager>().id == id)
            {
                GameObject mapPart =Instantiate(map, position, Quaternion.identity);
                mapPart.transform.parent = transform;
                break;
            }
        }
    }
}
