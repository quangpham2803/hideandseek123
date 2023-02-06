using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class DashSpeedManUI : MonoBehaviour
{
    public PlayerSetup player;
    public DashSpeedMan skill;
    private float time;
    public Image countdownUI;
    public Image imagePress;
    public Image defautImage;
    public Image silenceImage;
    bool t;
    public TextMeshProUGUI countDownText;
    public GameObject backgroundCountDown;
    bool isFirstTime;
    private void Start()
    {
        player = GetComponent<UIOwner>().owner;
        skill = player.GetComponentInChildren<DashSpeedMan>();
        isFirstTime = false;
        if (!player.photonView.IsMine || player.isBot)
        {
            gameObject.SetActive(false);
        }

    }
    public void UICountDown()
    {
        if (countdownUI.fillAmount >= 1)
        {
            countdownUI.gameObject.SetActive(false);
            countDownText.gameObject.SetActive(false);
            backgroundCountDown.gameObject.SetActive(false);
            isFirstTime = true;
            if (player.useSkill1 && player.isBot && PhotonNetwork.IsMasterClient)
            {
                player.useSkill1 = false;
                SkillClick();
            }
            if(ConfigDataGame.dashSpeedMan < 9)
            {
                ConfigDataGame.dashSpeedMan = 99;
            }
        }
        else
        {
            time += Time.deltaTime;
            if (time >= 0.3)
            {
                countdownUI.gameObject.SetActive(true);
                countDownText.gameObject.SetActive(true);
                if (isFirstTime == true)
                {
                    int timeTemp = (int)(ConfigDataGame.dashSpeedMan - time + 1);
                    backgroundCountDown.gameObject.SetActive(true);
                    countDownText.text = timeTemp.ToString();
                    countdownUI.fillAmount = time / ConfigDataGame.dashSpeedMan;
                }
                else
                {
                    int timeTemp = (int)(GameManager.instance.firstTimeColdown - time + 1);
                    backgroundCountDown.gameObject.SetActive(true);
                    countDownText.text = timeTemp.ToString();
                    countdownUI.fillAmount = time / GameManager.instance.firstTimeColdown;

                }
            }
        }
    }
    public void SkillClick()
    {
        if (countdownUI.fillAmount == 1 && player.dead == false && player.isJumping == false && !player.isSilent && player.isbeStun == false)
        {
            Utilities.FxButtonPress(imagePress.transform, true);
            Use();
            time = 0;
            countdownUI.fillAmount = 0;
        }
    }

    private void Update()
    {
        if (GameManager.instance.state != GameManager.GameState.Playing || !player.photonView.IsMine)
        {
            return;
        }
        UICountDown();
        if (player.isSilent && !t)
        {
            silenceImage.gameObject.SetActive(true);
            defautImage.gameObject.SetActive(false);
            t = true;
        }
        if (!player.isSilent && t)
        {
            silenceImage.gameObject.SetActive(false);
            defautImage.gameObject.SetActive(true);
            t = false;
        }
    }
    public void Use()
    {
        skill.Use();
    }
}
