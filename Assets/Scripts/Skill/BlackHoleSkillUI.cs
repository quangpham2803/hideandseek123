using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleSkillUI : MonoBehaviour
{
    public PlayerSetup player;
    public DashSpeedMan skill;
    public SkillData skillData;
    private InvisibleSkillData data;
    private float time;
    public Image countdownUI;
    public Image imagePress;
    public Text countDownText;
    public GameObject backgroundCountDown;
    PlayerSetup owner;
    bool isFirstTime;
    public Image defautImage;
    public Image silenceImage;
    bool t;
    private void Start()
    {
        player = GetComponent<UIOwner>().owner;
        skillData = GetComponent<UIOwner>().owner.GetComponent<SkillData>();
        data = skillData.invisibleSkillData;
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
                    int timeTemp = (int)(data.countdown - time + 1);
                    backgroundCountDown.gameObject.SetActive(true);
                    countDownText.text = timeTemp.ToString();
                    countdownUI.fillAmount = time / data.countdown;
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
        if (countdownUI.fillAmount == 1 && player.dead == false && player.isJumping == false && !player.isSilent)
        {
            Utilities.FxButtonPressSmall(imagePress.transform, true);
            Use();
            time = 0;
            countdownUI.fillAmount = 0;
        }
    }
    private void Update()
    {
        if (!player.photonView.IsMine)
        {
            return;
        }
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
        UICountDown();
    }
    public void Use()
    {
        skill.Use2();
    }
}
