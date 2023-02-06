using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManagerTutorial : MonoBehaviour
{
    public static TableManagerTutorial manager;
    public List<TableObjectTutorial> tableList;
    private void Awake()
    {
        manager = this;
    }
    //private void Start()
    //{
    //    StartCoroutine(WaitToAddId());
    //}
    //IEnumerator WaitToAddId()
    //{
    //    yield return new WaitUntil(GamePlaying);
    //    int i = 0;
    //    foreach (TableObjectTutorial table in tableList)
    //    {
    //        table.tableId = i;
    //        i++;
    //    }
    //}
    bool GamePlaying()
    {
        return GameManagerTutorial.instance.state == GameManagerTutorial.GameState.Playing;
    }
    public void Jump(float time, PlayerSetupTutorial p, TableObjectTutorial table)
    {
        JumpCmd(time, p, table);
    }
    void JumpCmd(float time, PlayerSetupTutorial p, TableObjectTutorial table)
    {
        StartCoroutine(WaitToJump(time, p, table));
    }
    IEnumerator WaitToJump(float time, PlayerSetupTutorial p, TableObjectTutorial table)
    {
        PlayerSetupTutorial playerJump = p;
        table.meshCollider.isTrigger = true;
        playerJump.speedStun = 0;
        playerJump.isJumping = true;
        playerJump.pAnimator.SetBool("Jump", true);
        yield return new WaitForSeconds(0.5f);
        playerJump.isJumping = false;
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
            foreach (PlayerSetupTutorial pl in GameManagerTutorial.instance.players)
            {
                if (pl.isBot && pl.player == GameManagerTutorial.HideOrSeek.Hide && pl.GetComponent<PlayerBotHideTutorial>().caseBot == PlayerBotHideTutorial.CaseBot.Table
                    && (pl.GetComponent<PlayerBotHideTutorial>().tableMove == table.point1 || pl.GetComponent<PlayerBotHideTutorial>().tableMove == table.point2))
                {
                    pl.GetComponent<PlayerBotHideTutorial>().caseBot = PlayerBotHideTutorial.CaseBot.GrassTable;
                    playerJump.GetComponent<PlayerBotHideTutorial>().CaseHiderBot();
                }
            }
            tableList.Remove(table);
            Destroy(table.body.gameObject);
        }
    }
}
