using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TableManager : MonoBehaviourPunCallbacks
{
    public static TableManager manager;
    public List<TableObject> tableList;
    private void Awake()
    {
        manager = this;
    }
    public void BanDage(int id)
    {
        photonView.RPC("BanDageToss", RpcTarget.AllBuffered, id);
    }
    [PunRPC]
    public void BanDageToss(int id)
    {
        PlayerSetup player = PhotonView.Find(id).GetComponent<PlayerSetup>();
        foreach (Bandage t in GameManager.instance.bandageList)
        {
            if (t.owner == player)
            {
                t.StopToss();
            }
        }
    }
    private void Start()
    {
        StartCoroutine(WaitToAddId());
    }
    IEnumerator WaitToAddId()
    {
        yield return new WaitUntil(GamePlaying);
        int i = 0;
        foreach(TableObject table in tableList)
        {
            table.tableId = i;
            i++;
        }
    }
    bool GamePlaying()
    {
        return GameManager.instance.state == GameManager.GameState.Playing;
    }
    public void Jump(float time, int id, int tableId)
    {
        photonView.RPC("JumpCmd", RpcTarget.AllBuffered, time, id, tableId);
    }
    [PunRPC]
    void JumpCmd(float time, int id, int tableId)
    {
        PlayerSetup p = PhotonView.Find(id).GetComponent<PlayerSetup>();
        StartCoroutine(WaitToJump(time, p, tableId));
    }
    IEnumerator WaitToJump(float time, PlayerSetup p, int tableId)
    {
        foreach(TableObject table in tableList)
        {
            if(table.tableId == tableId)
            {
                PlayerSetup playerJump = p;
                table.meshCollider.isTrigger = true;
                if (playerJump.isBot)
                {
                    playerJump.agent.enabled = false;
                }
                playerJump.speedStun = 0;
                playerJump.isJumping = true;
                foreach (AnimatorControllerParameter item in playerJump.pAnimator.parameters)
                {
                    playerJump.pAnimator.SetBool(item.name, false);
                }
                playerJump.pAnimator.SetBool("Jump", true);
                yield return new WaitForSeconds(0.5f);
                playerJump.isJumping = false;
                if (playerJump.isBot)
                {
                    playerJump.agent.enabled = true;
                    playerJump.GetComponent<Player>().caseBot = Player.CaseBot.GrassTable;
                    playerJump.GetComponent<Player>().CaseHiderBot();

                }
                playerJump.pAnimator.SetBool("Jump", false);
                playerJump.speedStun = 1;
                table.meshCollider.isTrigger = false;
                if (table.status < 3)
                {
                    table.status++;
                    table.body.material = table.listMaterial[table.status];
                }
                else
                {
                    foreach (PlayerSetup pl in GameManager.instance.players)
                    {
                        if (pl.isBot && pl.player == GameManager.HideOrSeek.Hide && pl.GetComponent<Player>().caseBot == Player.CaseBot.Table
                            && (pl.GetComponent<Player>().tableMove == table.point1 || pl.GetComponent<Player>().tableMove == table.point2))
                        {
                            pl.GetComponent<Player>().caseBot = Player.CaseBot.GrassTable;
                            playerJump.GetComponent<Player>().CaseHiderBot();
                        }
                    }
                    tableList.Remove(table);
                    Destroy(table.body.gameObject);
                    break;
                }
            }
        }        
    }
}
