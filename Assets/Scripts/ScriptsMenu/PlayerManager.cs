using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;
using MonoBehaviour = UnityEngine.MonoBehaviour;

public class PlayerManager : MonoBehaviour
{

	PhotonView PV;
    void Awake()
	{
		PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        int spawnPicker = UnityEngine.Random.Range(0, GameSetupController.GS.point.Length);
        if (PV.IsMine)
        {
            CreateController(GameSetupController.GS.point[spawnPicker]);
        }
    }
    void CreateController(Transform point)
	{
		GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), point.position, Quaternion.identity,0);

	}
}