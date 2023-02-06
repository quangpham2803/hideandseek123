using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwitchSkillUI : MonoBehaviour
{
    public Image countdownUI;
    private float time;
    public float countdown;
    public GameObject skillFake, skillTranform;
    int i = 0;
    public Image imagePress;
    public Text countDownText;
    public GameObject backgroundCountDown;
    PlayerSetup player;
    public Image defautImage;
    public Image silenceImage;
    public Image silenceImage2Skill;
    bool t;
    bool isFirstTime;
    private void Start()
    {
        player = GetComponent<UIOwner>().owner;
        isFirstTime = false;
    }
    IEnumerator IEClick()
    {
        time = 0;
        yield return new WaitForSeconds(0.75f);
    }
    public void Onclick()
    {
        if (countdownUI.fillAmount == 0 && player.dead == false && !player.isSilent)
        {
            if (i == 0)
            {
                skillFake.SetActive(false);
                skillTranform.SetActive(true);
                i = 1;
            }
            else
            {
                skillFake.SetActive(true);
                skillTranform.SetActive(false);
                i = 0;
            }
            Utilities.FxButtonPressSmall(imagePress.transform, true);
            time = 0;
            countdownUI.fillAmount = 1;
        }
        
    }
    private void Update()
    {
        if (!player.photonView.IsMine)
        {
            return;
        }
        UICountDown();
        if (player.isSilent && !t)
        {
            silenceImage.gameObject.SetActive(true);
            silenceImage2Skill.gameObject.SetActive(true);
            defautImage.gameObject.SetActive(false);
            skillFake.SetActive(false);
            skillTranform.SetActive(false);
            t = true;
        }
        if (!player.isSilent && t)
        {
            silenceImage.gameObject.SetActive(false);
            silenceImage2Skill.gameObject.SetActive(false);
            defautImage.gameObject.SetActive(true);
            if (i == 0)
            {
                skillFake.SetActive(false);
                skillTranform.SetActive(true);
            }
            else
            {
                skillFake.SetActive(true);
                skillTranform.SetActive(false);
            }
            t = false;
        }
    }
    public void UICountDown()
    {
        if (countdownUI.fillAmount <= 0)
        {
            countdownUI.gameObject.SetActive(false);
            countDownText.gameObject.SetActive(false);
            backgroundCountDown.gameObject.SetActive(false);
            isFirstTime = true;
        }
        else
        {
            time += Time.deltaTime;
            if(time >= 0.3f)
            {
                countdownUI.gameObject.SetActive(true);
                countDownText.gameObject.SetActive(true);
                if (isFirstTime == true)
                {
                    int timeTemp = (int)(countdown - time + 1);
                    backgroundCountDown.gameObject.SetActive(true);
                    countDownText.text = timeTemp.ToString();
                    countdownUI.fillAmount = (countdown - time) / countdown;
                }
                else
                {
                    int timeTemp = (int)(GameManager.instance.firstTimeColdown - time + 1);
                    backgroundCountDown.gameObject.SetActive(true);
                    countDownText.text = timeTemp.ToString();
                    countdownUI.fillAmount = (GameManager.instance.firstTimeColdown - time) / GameManager.instance.firstTimeColdown;
                }
            }
        }
    }
}
