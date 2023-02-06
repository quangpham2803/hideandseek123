using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TableObject : MonoBehaviourPunCallbacks
{
    public float speedHide;
    public PlayerSetup player;
    public MeshRenderer body;
    public Material[] listMaterial;
    public int status;
    public List<PlayerSetup> curJumpPlayer = new List<PlayerSetup>();
    public MeshCollider meshCollider;
    public GameObject jumpicon;
    public GameObject cantjumpicon;
    bool isTurnOff;
    public Transform point1;
    public Transform point2;
    public bool tableHorizontal;
    public int tableId;
    private void Start()
    {
        status = 0;
        meshCollider = GetComponent<MeshCollider>();
        body.material = listMaterial[status];
        isTurnOff = false;
        cantjumpicon.SetActive(false);
    }
    private void Update()
    {
        if(GameManager.instance.isSeeker && isTurnOff == false)
        {
            jumpicon.SetActive(false);
            cantjumpicon.SetActive(true);
            isTurnOff = true;
        }
    }
    public void PlayerJump(PlayerSetup setup)
    {
        if (!setup.isJumping)
        {
            if (setup.player == GameManager.HideOrSeek.Hide && setup != player && setup.dead == false && !setup.isbeStun)
            {
                curJumpPlayer.Add(setup);
                if (curJumpPlayer.Count != 0)
                {
                    for (int i = 0; i < curJumpPlayer.Count; i++)
                    {
                        if (setup == curJumpPlayer[i])
                        {
                            TableManager.manager.Jump(speedHide, curJumpPlayer[i].photonView.ViewID, tableId);
                            break;
                        }
                    }
                }
            }
            else
            {
                player = null;
            }

        }
    }

    private void OnDisable()
    {
        if (GameManager.instance.state != GameManager.GameState.Playing)
            return;
        if(curJumpPlayer.Count != 0 )
        {
            foreach (PlayerSetup item in curJumpPlayer)
            {
                if (item.isBot)
                {
                    item.isJumping = false;
                    foreach (PlayerSetup pl in GameManager.instance.players)
                    {
                        if (pl.isBot && pl.player == GameManager.HideOrSeek.Hide && pl.GetComponent<Player>().caseBot == Player.CaseBot.Table
                            && (pl.GetComponent<Player>().targetPoint == point1 || pl.GetComponent<Player>().targetPoint == point2))
                        {
                            pl.GetComponent<Player>().caseBot = Player.CaseBot.GrassTable;
                            pl.GetComponent<Player>().CaseHiderBot();
                        }
                    }
                    GameManager.instance.tableAI.Remove(point1);
                    GameManager.instance.tableAI.Remove(point2);
                }
                if (item.speedStun ==0)
                {
                    item.pAnimator.SetBool("Jump", false);
                    item.speedStun = 1;
                    item.isJumping = false;
                }
            }
        }
    }
}
