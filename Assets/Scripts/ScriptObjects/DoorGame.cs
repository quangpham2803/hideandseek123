using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DoorGame : MonoBehaviourPunCallbacks
{
    public ButtonGame[] buttonList;
    public static DoorGame door;
    public bool allButtonCheck;
    //Hide
    public int numberAll, numberOpened;
    public Escape escape;
    public Text warning;
    public bool opened;
    public bool opening;
    public bool isSpeedUpHide;
    private float timeOpening;
    public GameObject waitfor10s;
    private void Awake()
    {
        if (door == null)
        {
            door = this;
        }
        else
        {
            Destroy(gameObject);
        }
        isWatching = false;
    }
    public bool playerDead;
    private void Start()
    {
        numberAll = buttonList.Length;
        numberOpened = 0;
        warning.text = "";
        opened = false;
        isSpeedUpHide = false;
        isLastZone = false;
    }
    public int timeTemp;
    bool temp;
    public bool isLastZone;
    public float processDoorDecrease;
    private void Update()
    {
        //if ((GameManager.instance.currentMatchTime <= 30 || Input.GetKeyDown(KeyCode.O)) && !temp)
        //{
        //    temp = true;
        //    StartCoroutine(OpenDoorScene());
        //    GameManager.instance.itemDoorAndButton.SetActive(false);
        //    GameManager.instance.doorOpening.SetActive(false);
        //    if (GameManager.instance.mainPlayer.player == GameManager.HideOrSeek.Seek)
        //    {
        //        GameManager.instance.escapeText.text = "STOP THEM";
        //    }
        //    GameManager.instance.escapeText.gameObject.SetActive(true);
        //    GameManager.instance.processDoorText.gameObject.SetActive(false);
        //    GameManager.instance.doorOpenIn30s = false;
        //    GameManager.instance.ResetListItemInfo();
        //    GameManager.instance.ItemInfoActive(GameManager.instance.itemDoorOpen);
        //    opened = true;
        //    escape.opened = true;
        //}
        if (/*((GameManager.instance.doorOpenIn30s && (buttonList.Length - numberOpened <= 1))) || Input.GetKeyDown(KeyCode.P)*/
            GameManager.instance.zone.activeSelf && CoolBattleRoyaleZone.Zone.Instance.CurStep == CoolBattleRoyaleZone.Zone.Instance.StepsToEnd -1 && isLastZone == false)
        {           
            timeOpening += Time.deltaTime;
            escape.opening = true;
            timeTemp = 15 - (int)timeOpening;
            GameManager.instance.processDoorText.text = timeTemp.ToString() +"s";
            if(timeTemp <= 15 && !waitfor10s.activeSelf)
            {
                waitfor10s.SetActive(true);
            }
            if (timeTemp <= 0)
            {
                StartCoroutine(OpenDoorScene());
                GameManager.instance.doorOpening.SetActive(false);
                if (GameManager.instance.mainPlayer.player == GameManager.HideOrSeek.Seek)
                {
                    GameManager.instance.escapeText.text = "STOP THEM";
                }
                GameManager.instance.escapeText.gameObject.SetActive(true);
                GameManager.instance.processDoorText.gameObject.SetActive(false);
                GameManager.instance.doorOpenIn30s = false;
                GameManager.instance.ResetListItemInfo();
                opened = true;
                escape.opened = true;
                isLastZone = true;
            }           
        }
        //if (GameManager.instance.state == GameManager.GameState.Playing && GameManager.instance.players.Count == GameManager.instance.numberSeek + 1 && isSpeedUpHide == false)
        //{
        //    foreach (PlayerSetup p in GameManager.instance.players)
        //    {
        //        if (p.player == GameManager.HideOrSeek.Hide)
        //        {
        //            p.speedDoorOpen = 1f;
        //        }
        //    }
        //    //StartCoroutine(Time30sRemain("Hiders increase speed"));
        //    isSpeedUpHide = true;
        //}
    }
    public bool isWatching;
    //public GameObject FF;
    IEnumerator OpenDoorScene()
    {
        //FF.SetActive(true);
        //GetComponent<Animator>().SetBool("opened", true);
        GameManager.instance.waypointCanavas.SetActive(false);
        GameManager.instance.mainPlayer.cam.cinemachine.LookAt = escape.transform;
        GameManager.instance.mainPlayer.cam.cinemachine.Follow = escape.transform;
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            p.speedStun = 0;
        }
        isWatching = true;
        yield return new WaitForSeconds(3f);
        GameManager.instance.mainPlayer.cam.cinemachine.LookAt = GameManager.instance.mainPlayer.cam.owner.transform;
        GameManager.instance.mainPlayer.cam.cinemachine.Follow = GameManager.instance.mainPlayer.cam.owner.transform;
        GameManager.instance.waypointCanavas.SetActive(true);
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            p.speedStun = 1;
        }
        isWatching = false;
        //gameObject.SetActive(false);
    }
    public void Check()
    {
        for(int i = 0; i < buttonList.Length; i++)
        {
            if(buttonList[i].isActive == true && buttonList[i].isAlert == false)
            {
                UpdateButtonNumber();
                buttonList[i].isAlert = true;
            }
        }
    }
    
    IEnumerator IEItem(GameObject item, float time)
    {
        yield return new WaitForSeconds(time);
        item.SetActive(false);
    }
    
    IEnumerator Time30sRemain(string text)
    {
        warning.text = text; 
        yield return new WaitForSeconds(5f);
        warning.text = "";
    }
    void UpdateButtonNumber()
    {
        numberAll --;
        numberOpened ++;
    }
    public int progress;
    public void CheckPlayerDie(string playerName)
    {
        playerDead = true;
        //StartCoroutine(KillerAlert(playerName));
    }
}
