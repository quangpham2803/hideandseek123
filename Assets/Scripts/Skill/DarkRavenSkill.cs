using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DarkRavenSkill : MonoBehaviour
{
    public PlayerSetup player;
    public PlayerRPC rpc;
    private float time;
    public Image countdownUI;
    public Image imagePress;
    public TextMeshProUGUI countDownText;
    public GameObject backgroundCountDown;
    bool isFirstTime;
    private void Start()
    {
        player = GetComponent<UIOwner>().owner;
        rpc = GetComponent<UIOwner>().owner.rpc;
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
            if (player.useSkill2 && player.isBot && PhotonNetwork.IsMasterClient)
            {
                player.useSkill2 = false;
                SkillClick();
            }
            if(ConfigDataGame.summonRaven < 10)
            {
                ConfigDataGame.summonRaven = 99;
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
                    int timeTemp = (int)(ConfigDataGame.summonRaven - time + 1);
                    backgroundCountDown.gameObject.SetActive(true);
                    countDownText.text = timeTemp.ToString();
                    countdownUI.fillAmount = time / ConfigDataGame.summonRaven;
                }
                else
                {
                    int timeTemp = (int)(GameManager.instance.firstTimeColdown - time + 1 - 5);
                    backgroundCountDown.gameObject.SetActive(true);
                    countDownText.text = timeTemp.ToString();
                    countdownUI.fillAmount = time / 6;
                }
            }           
        }
    }

    public void SkillClick()
    {
        if (countdownUI.fillAmount == 1 && player.dead == false && player.isFollow == false && player.isbeStun == false)
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
    }
    public void Use()
    {
        if (!player.photonView.IsMine)
        {
            return;
        }
        foreach (GameObject item in player.skill)
        {
            if (item.GetComponent<ShotRavenSkill>() != null)
            {
                item.SetActive(true);
            }
        }
        rpc.photonView.RPC("SummonRavenSkill", RpcTarget.AllBuffered/*,player.photonView.ViewID*/, player.transform.position);
        gameObject.SetActive(false);
        
    }
}
