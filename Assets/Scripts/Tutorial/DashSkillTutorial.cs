using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashSkillTutorial : MonoBehaviour
{
    public PlayerSetupTutorial player;
    public DashSpeedManTutorial skill;
    private float time;
    public Image countdownUI;
    public Image imagePress;
    public TextMeshProUGUI countDownText;
    public GameObject backgroundCountDown;
    private void Start()
    {
        player = GetComponent<UIOwnerTutorial>().owner;
        skill = player.GetComponentInChildren<DashSpeedManTutorial>();
    }
    public void UICountDown()
    {
        if (countdownUI.fillAmount >= 1)
        {
            countdownUI.gameObject.SetActive(false);
            countDownText.gameObject.SetActive(false);
            backgroundCountDown.gameObject.SetActive(false);
        }
        else
        {
            time += Time.deltaTime;
            if (time >= 0.3)
            {
                countdownUI.gameObject.SetActive(true);
                countDownText.gameObject.SetActive(true);
                int timeTemp = (int)(1 - time + 1);
                backgroundCountDown.gameObject.SetActive(true);
                countDownText.text = (timeTemp +1).ToString();
                countdownUI.fillAmount = time / 2;
            }
        }
    }
    public void SkillClick()
    {
        if (countdownUI.fillAmount == 1 && player.dead == false && player.isJumping == false && !player.isSilent)
        {
            Utilities.FxButtonPress(imagePress.transform, false);
            Use();
            time = 0;
            countdownUI.fillAmount = 0;
        }
    }

    private void Update()
    {
        UICountDown();
    }
    public void Use()
    { 
        player.pAnimator.enabled = true;
        skill.Use();
        
    }
}
