using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class EndSceneManager : MonoBehaviourPunCallbacks
{
    public static EndSceneManager manager;
    //Hide win
    public CameraFollow camMain;
    public GameObject endScene;
    public string nameHider;
    public GameObject[] modelHider;
    public GoHome leaveRoom;
    //Seek win
    public GameObject endScene2;
    public PlayerSeekWin[] seekWinList;
    public GameObject winSeekOpen, winSeekClose;
    private void Awake()
    {
        manager = this;
    }
    public void OpenEndScene(string namePlayer, int charId, int idPlayer)
    {
        photonView.RPC("OpenEndSceneCmd", RpcTarget.AllBuffered, namePlayer, charId, idPlayer);
    }
    [PunRPC]
    void OpenEndSceneCmd(string namePlayer, int charId, int idPlayer)
    {
        camMain.btnDead.SetActive(false);
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            if(p.photonView.ViewID == idPlayer)
            {
                GameManager.instance.state = GameManager.GameState.Ending;
                GameManager.instance.seekWin = false;
                GameManager.instance.waypointCanavas.SetActive(false);
                GameManager.instance.mainPlayer.cam.cinemachine.LookAt = p.transform;
                GameManager.instance.mainPlayer.cam.cinemachine.Follow = p.transform;
                GameManager.instance.mainPlayer.GetComponent<Player>().rigidbody.velocity = Vector3.zero;
                DoorGame.door.escape.playAnim = true;
                StartCoroutine(DoorGame.door.escape.waitToExplo(p, 0.5f));
                break;
            }
        }


        modelHider[charId].SetActive(true);
        GameManager.instance.canavasPlayingGame.SetActive(false);
        GameManager.instance.waypointCanavas.SetActive(false);
        camMain.GetComponent<Camera>().enabled = false;
        endScene.SetActive(true);
        nameHider = namePlayer;
    }
    public IEnumerator PopUpMenu(PlayerSetup player)
    {
        yield return new WaitForSeconds(1f);
        endScene.SetActive(false);
        camMain.enabled = true;
        if (player.player == GameManager.HideOrSeek.Hide)
        {
            player.cam.WinCam();
        }
        else
        {
            player.cam.LoseCam();
        }
    }
    IEnumerator PopUpMenu2(PlayerSetup player)
    {
        yield return new WaitForSeconds(3f);
        if (player.player == GameManager.HideOrSeek.Seek)
        {
            player.cam.WinCam();
        }
        else
        {
            player.cam.LoseCam();
        }
    }
    public IEnumerator OpenSceneSeekWin()
    {
        yield return new WaitForSeconds(1f);
        camMain.btnDead.SetActive(false);
        endScene2.SetActive(true);
        int i = 0;
        foreach(PlayerSetup p in GameManager.instance.players)
        {
            if(p.player == GameManager.HideOrSeek.Seek)
            {
                seekWinList[i].AddPlayerAndName(SeekCharId(p.character), p.name.text);
                i++;
                Debug.Log(p.name.text);
            }
        }
        GameManager.instance.canavasPlayingGame.SetActive(false);
        GameManager.instance.waypointCanavas.SetActive(false);
        camMain.GetComponent<Camera>().enabled = false;
        yield return new WaitForSeconds(5f);
        winSeekOpen.SetActive(false);
        winSeekClose.SetActive(true);
        StartCoroutine(PopUpMenu2(GameManager.instance.mainPlayer));
    }
    int SeekCharId(GameManager.Character character)
    {
        int id = 0;
        switch (character)
        {
            case GameManager.Character.DarkRaven:
                {
                    id = 0;
                    break;
                }
            case GameManager.Character.Jokers:
                {
                    id = 1;
                    break;
                }
            case GameManager.Character.Davros:
                {
                    id = 2;
                    break;
                }
        }
        return id;
    }
}
