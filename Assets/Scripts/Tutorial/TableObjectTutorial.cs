using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableObjectTutorial : MonoBehaviour
{
    public float speedHide;
    public PlayerSetupTutorial player;
    public MeshRenderer body;
    public Material[] listMaterial;
    public int status;
    public List<PlayerSetupTutorial> curJumpPlayer = new List<PlayerSetupTutorial>();
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
        if (GameManagerTutorial.instance.isSeeker && isTurnOff == false)
        {
            jumpicon.SetActive(false);
            cantjumpicon.SetActive(true);
            isTurnOff = true;
        }
    }
    public void PlayerJump(PlayerSetupTutorial setup)
    {
        if (!setup.isJumping)
        {
            if (setup.player == GameManagerTutorial.HideOrSeek.Hide && setup.dead == false && !setup.isbeStun)
            {
                curJumpPlayer.Add(setup);
                if (curJumpPlayer.Count != 0)
                {
                    for (int i = 0; i < curJumpPlayer.Count; i++)
                    {
                        if (setup == curJumpPlayer[i])
                        {
                            TableManagerTutorial.manager.Jump(speedHide, curJumpPlayer[i], this);
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
        if (GameManagerTutorial.instance.state != GameManagerTutorial.GameState.Playing)
            return;
        if (curJumpPlayer.Count != 0)
        {
            foreach (PlayerSetupTutorial item in curJumpPlayer)
            {
                if (item.isBot)
                {
                    item.isJumping = false;
                    foreach (PlayerSetupTutorial pl in GameManagerTutorial.instance.players)
                    {
                        if (pl.isBot && pl.player == GameManagerTutorial.HideOrSeek.Hide && pl.GetComponent<PlayerBotHideTutorial>().caseBot == PlayerBotHideTutorial.CaseBot.Table
                            && (pl.GetComponent<PlayerBotHideTutorial>().targetPoint == point1 || pl.GetComponent<PlayerBotHideTutorial>().targetPoint == point2))
                        {
                            pl.GetComponent<PlayerBotHideTutorial>().caseBot = PlayerBotHideTutorial.CaseBot.GrassTable;
                            pl.GetComponent<PlayerBotHideTutorial>().CaseHiderBot();
                        }
                    }
                    GameManagerTutorial.instance.tableAI.Remove(point1);
                    GameManagerTutorial.instance.tableAI.Remove(point2);
                }
                if (item.speedStun == 0)
                {
                    item.pAnimator.SetBool("Jump", false);
                    item.speedStun = 1;
                    item.isJumping = false;
                }
            }
        }
    }
}
