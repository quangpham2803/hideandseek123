using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoThroughObject : MonoBehaviour
{
    public Image countdownUI;
    public TransformSkill skill;
    private float time;
    public TransformSkillData data;
    PlayerSetup player;
    public Image imagePress;
    public Text countDownText;
    public GameObject backgroundCountDown;
    bool isFirstTime;
    public Image defautImage;
    public Image silenceImage;
    bool t;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<UIOwner>().owner;
        if (!player.photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
        skill = player.GetComponentInChildren<TransformSkill>();
        isFirstTime = false;
    }

    // Update is called once per frame
    void Update()
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
            skill = player.GetComponentInChildren<TransformSkill>();
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
            Use();
            time = 0;
            countdownUI.fillAmount = 0;
        }
    }
    public void Use()
    {
        skill.Use2();
    }
}
