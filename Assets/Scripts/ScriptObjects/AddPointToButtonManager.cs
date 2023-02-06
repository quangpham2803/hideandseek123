using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AddPointToButtonManager : MonoBehaviourPunCallbacks
{
    public int id;
    public List<Transform> pointButton;
    public List<Transform> pointBox;
    public List<GameObject> grass;
    public List<Transform> spawnPoint;
    public List<TableObject> tableList;
    public List<TrashObject> trashList;
    // Start is called before the first frame update
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach(Transform point in pointButton)
            {
                SpawnButtonRandom.Spawn.pointList.Add(point);
            }
            SpawnButtonRandom.Spawn.haveAdd++;
            foreach (Transform point in pointBox)
            {
                SpawnRandomBox.manager.pointList.Add(point);
            }
            SpawnRandomBox.manager.haveAdd++;
            //if(SpawnButtonRandom.Spawn.haveAdd == 9)
            //{
            //    SpawnButtonRandom.Spawn.SpawnRandom();
            //}
            if(SpawnRandomBox.manager.haveAdd == 9)
            {
                SpawnRandomBox.manager.StartGame();
            }
        }
        SetupMap();
    }
    public void SetupMap()
    {
        foreach(Transform point in spawnPoint)
        {
            GameManager.instance.hidePos.Add(point);
        }
        foreach(GameObject grassPart in grass)
        {
            GameManager.instance.grass.Add(grassPart);
        }
        foreach(TableObject table in tableList)
        {
            GameManager.instance.tableAI.Add(table.point1);
            GameManager.instance.tableAI.Add(table.point2);
            TableManager.manager.tableList.Add(table);
        }
        foreach (TrashObject can in trashList)
        {
            TrashObjectManager.manager.trashObjectList.Add(can);
        }
    }
}
