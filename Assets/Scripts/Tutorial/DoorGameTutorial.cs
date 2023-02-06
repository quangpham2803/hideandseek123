using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorGameTutorial : MonoBehaviour
{
    public ButtonTutorial[] buttonList;
    public static DoorGameTutorial door;
    public bool allButtonCheck;
    //Hide
    public int numberAll, numberOpened;
    public EscapeTutorial escape;
    public Text warning;
    public bool opened;
    public bool opening;
    public bool isSpeedUpHide;
    private float timeOpening;
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

    }
    public bool playerDead;
    private void Start()
    {
        numberAll = buttonList.Length;
        numberOpened = 0;
        warning.text = "";
        opened = false;
        isSpeedUpHide = false;
    }
    public int timeTemp;
    bool temp;
    public bool isLastZone;
    public float processDoorDecrease;
    public GameObject waitfor10s;
    private void Update()
    {
        if (GameManagerTutorial.instance.scene == GameManagerTutorial.isScene.Scene3)
        {
            if ((GameManagerTutorial.instance.currentMatchTime <= 30 || Input.GetKeyDown(KeyCode.O)) && !temp)
            {
                StartCoroutine(OpenDoorScene());
                GameManagerTutorial.instance.itemDoorAndButton.SetActive(false);
                GameManagerTutorial.instance.doorUI.SetActive(false);
                GameManagerTutorial.instance.doorOpening.SetActive(false);
                GameManagerTutorial.instance.escapeText.gameObject.SetActive(true);
                GameManagerTutorial.instance.processDoorText.gameObject.SetActive(false);
                GameManagerTutorial.instance.doorOpenIn30s = false;
                GameManagerTutorial.instance.ResetListItemInfo();
                GameManagerTutorial.instance.ItemInfoActive(GameManagerTutorial.instance.itemDoorOpen);
                opened = true;
                escape.GetComponent<Escape>().opened = true;
            }
            if (/*((GameManager.instance.doorOpenIn30s && (buttonList.Length - numberOpened <= 1))) || Input.GetKeyDown(KeyCode.P)*/
                GameManagerTutorial.instance.zone.activeSelf && ZoneTutorial.Instance.CurStep == ZoneTutorial.Instance.StepsToEnd - 1 && isLastZone == false)
            {
                timeOpening += Time.deltaTime;
                escape.opening = true;
                timeTemp = 15 - (int)timeOpening;
                GameManagerTutorial.instance.processDoorText.text = timeTemp.ToString() + "s";
                if (timeTemp <= 15 && !waitfor10s.activeSelf)
                {
                    waitfor10s.SetActive(true);
                }
                if (timeTemp <= 0)
                {
                    StartCoroutine(OpenDoorScene());
                    GameManagerTutorial.instance.doorOpening.SetActive(false);
                    if (GameManagerTutorial.instance.mainPlayer.player == GameManagerTutorial.HideOrSeek.Seek)
                    {
                        GameManagerTutorial.instance.escapeText.text = "STOP THEM";
                    }
                    GameManagerTutorial.instance.escapeText.gameObject.SetActive(true);
                    GameManagerTutorial.instance.processDoorText.gameObject.SetActive(false);
                    GameManagerTutorial.instance.doorOpenIn30s = false;
                    GameManagerTutorial.instance.ResetListItemInfo();
                    GameManagerTutorial.instance.ItemInfoActive(GameManagerTutorial.instance.itemDoorOpen);
                    opened = true;
                    escape.opened = true;
                    isLastZone = true;
                }
            }
        }
        
    }
    bool isWatching;
    public GameObject FF;
    public IEnumerator OpenDoorScene()
    {
        if (GameManagerTutorial.instance.scene == GameManagerTutorial.isScene.Scene1)
        {
            if (Vector3.Distance(GameManagerTutorial.instance.mainPlayer.transform.position, GameManagerTutorial.instance.posEscape1.position) > Vector3.Distance(GameManagerTutorial.instance.mainPlayer.transform.position, GameManagerTutorial.instance.posEscape2.position))
            {
                escape.transform.position = GameManagerTutorial.instance.posEscape1.position;
            }
            else
            {
                escape.transform.position = GameManagerTutorial.instance.posEscape2.position;
            }
        }
        escape.GetComponent<EscapeTutorial>().effect.SetActive(true);
        escape.GetComponent<EscapeTutorial>().opened = true;
        //GameManagerTutorial.instance.waypointCanavas.SetActive(false);
        GameManagerTutorial.instance.mainPlayer.cam.cinemachine.LookAt = escape.transform;
        GameManagerTutorial.instance.mainPlayer.cam.cinemachine.Follow = escape.transform;
        foreach (PlayerSetupTutorial p in GameManagerTutorial.instance.players)
        {
            p.speedStun = 0;
        }
        isWatching = true;
        yield return new WaitForSeconds(3f);
        GameManagerTutorial.instance.mainPlayer.cam.cinemachine.LookAt = GameManagerTutorial.instance.mainPlayer.cam.owner.transform;
        GameManagerTutorial.instance.mainPlayer.cam.cinemachine.Follow = GameManagerTutorial.instance.mainPlayer.cam.owner.transform;
        //GameManagerTutorial.instance.waypointCanavas.SetActive(true);
        foreach (PlayerSetupTutorial p in GameManagerTutorial.instance.players)
        {
            p.speedStun = 1;
            if (p.isbeStun)
            {
                p.isbeStun = false;
            }
        }
        if (GameManagerTutorial.instance.scene == GameManagerTutorial.isScene.Scene1)
        {
            GameManagerTutorial.instance.tutorial = GameManagerTutorial.SceneTutorial1.Step7;
        }
        isWatching = false;
    }
    public void Check()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            if (buttonList[i].isActive == true && buttonList[i].isAlert == false)
            {
                UpdateButtonNumber();
                StartCoroutine(ButtonAlert(buttonList[i].nameButton));
                buttonList[i].isAlert = true;
            }
        }
    }

    public void FixedButton()
    {
        if (numberOpened == 4)
        {
        }
        numberAll++;
        numberOpened--;
        GameManagerTutorial.instance.itemDoorAndButton.SetActive(true);
        GameManagerTutorial.instance.doorUI.SetActive(false);
        GameManagerTutorial.instance.ResetListItemInfo();
        GameManagerTutorial.instance.itemButtonDisactive.SetActive(true);
        GameManagerTutorial.instance.buttonChangeNumber.text = "+1";
        //GameManagerTutorial.instance.itemDoorAndButton.GetComponent<Animator>().SetBool("Active", true);
        Utilities.FxButtonPress(GameManagerTutorial.instance.imageButton.transform, false);
        StartCoroutine(IEItem((4 - numberOpened).ToString(),GameManagerTutorial.instance.itemButtonDisactive, 2f));

        //GameManagerTutorial.instance.doorOpenIn30s = false;
        //escape.GetComponent<Escape>().opening = false;
        //foreach (PlayerSetupTutorial item in GameManagerTutorial.instance.players)
        //{
        //    if (item.player == GameManagerTutorial.HideOrSeek.Seek)
        //    {
        //        item.speedDoorOpen = 0f;
        //    }
        //}
    }
    IEnumerator IEItem(string number, GameObject item, float time)
    {
        yield return new WaitForSeconds(time);
        GameManagerTutorial.instance.itemDoorAndButton.GetComponent<Animator>().SetBool("Active", false);
        GameManagerTutorial.instance.buttonNumber.text = number;
        item.SetActive(false);
    }
    IEnumerator ButtonAlert(string buttonName)
    {
        if (numberOpened != 4)
        {
            //GameManagerTutorial.instance.step1Text.text = (4 - numberOpened).ToString();
            GameManagerTutorial.instance.stepText.text = "Bạn cần kích hoạt: <color=orange><size=73>"+ (4 - numberOpened).ToString()+ "</size> </color><sprite=0> \nđể mở cửa";
            GameManagerTutorial.instance.ResetListItemInfo();
            GameManagerTutorial.instance.buttonChangeNumber.text = "-1";
            GameManagerTutorial.instance.itemButtonActive.SetActive(true);
            GameManagerTutorial.instance.itemDoorAndButton.GetComponent<Animator>().SetBool("Active", true);
            Utilities.FxButtonPress(GameManagerTutorial.instance.imageButton.transform, false);
            StartCoroutine(IEItem((4 - numberOpened).ToString(), GameManagerTutorial.instance.itemButtonActive, 2f));
            yield return new WaitForSeconds(3f);
        }
        else if (numberOpened == 4)
        {
            GameManagerTutorial.instance.itemDoorAndButton.SetActive(false);
            GameManagerTutorial.instance.itemButtonActive.SetActive(false);
            //GameManagerTutorial.instance.doorUI.SetActive(true);
            //GameManagerTutorial.instance.doorOpening.SetActive(true);
            //GameManagerTutorial.instance.ResetListItemInfo();
            //GameManagerTutorial.instance.ItemInfoActive(GameManagerTutorial.instance.itemDoor30s);
            GameManagerTutorial.instance.skip.SetActive(true);
            Utilities.FxButtonPress(GameManagerTutorial.instance.doorUI.transform, false);
            //foreach (PlayerSetupTutorial item in GameManagerTutorial.instance.players)
            //{
            //    if (item.player == GameManagerTutorial.HideOrSeek.Seek)
            //    {
            //        item.speedDoorOpen = 1.5f;
            //    }
            //}
            //GameManagerTutorial.instance.step1a.SetActive(false);
            //GameManagerTutorial.instance.doorOpenIn30s = true;
            //GameManagerTutorial.instance.doorUI.SetActive(true);
            yield return new WaitForSeconds(5f);
        }
    }
    IEnumerator Time30sRemain(string text)
    {
        warning.text = text;
        yield return new WaitForSeconds(5f);
        warning.text = "";
    }
    void UpdateButtonNumber()
    {
        numberAll--;
        numberOpened++;
    }
    public int progress;
    public void CheckPlayerDie(string playerName)
    {
        playerDead = true;
        //StartCoroutine(KillerAlert(playerName));
    }
}
