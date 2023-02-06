using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizardShootSkillUI : MonoBehaviour
{
    public ShootSkillData data;
    public Image countdownUI;
    private float time;
    public WizardShootSkill skill;
    PlayerSetup player;
    public Text countDownText;
    public GameObject backgroundCountDown;
    public Image imagePress;
    bool isFirstTime;
    public Image defautImage;
    public Image silenceImage;
    bool t;
    private void Start()
    {
        player = GetComponent<UIOwner>().owner;
        if (!player.photonView.IsMine || player.isBot)
        {
            gameObject.SetActive(false);
        }
        skill = player.GetComponentInChildren<WizardShootSkill>();
        isFirstTime = false;
    }
    private void Update()
    {
        if (!player.photonView.IsMine)
        {
            return;
        }
        if (skill != null || player.playerTemp == GameManager.HideOrSeek.Seek)
        {
            UICountDown();
        }
        else
        {
            skill = player.GetComponentInChildren<WizardShootSkill>();
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
    }
    public void UICountDown()
    {
        if (countdownUI.fillAmount >= 1)
        {
            countdownUI.gameObject.SetActive(false);
            countDownText.gameObject.SetActive(false);
            backgroundCountDown.gameObject.SetActive(false);
            isFirstTime = true;
            if (player.useSkill2 && player.isBot && PhotonNetwork.IsMasterClient)
            {
                player.useSkill2 = false;
                SkillClick();
            }
        }
        else
        {
            time += Time.deltaTime;
            if(time >= 0.3)
            {
                countdownUI.gameObject.SetActive(true);
                countDownText.gameObject.SetActive(true);
                if (isFirstTime == true)
                {
                    int timeTemp = (int)(data.countdown2 - time + 1);
                    backgroundCountDown.gameObject.SetActive(true);
                    countDownText.text = timeTemp.ToString();
                    countdownUI.fillAmount = time / data.countdown2;
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
            countdownUI.fillAmount = 0;
            Use();
        }
    }
    public void Use()
    {
        time = 0;
        skill.Use();
    }
}
