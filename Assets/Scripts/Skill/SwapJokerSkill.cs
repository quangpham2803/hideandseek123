using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwapJokerSkill : MonoBehaviour
{
    public PlayerSetup player;
    public PlayerRPC rpc;
    private float time;
    public Image imagePress;
    public Image countdownUI;
    public Image haveTarget;
    public TextMeshProUGUI countDownText;
    public GameObject backgroundCountDown;
    bool isFirstTime;
    private void Start()
    {
        player = GetComponent<UIOwner>().owner;
        rpc = GetComponent<UIOwner>().owner.rpc;
        isFirstTime = false;
        if (!player.photonView.IsMine)
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
            if (haveTarget.gameObject.activeSelf && player.isBot && PhotonNetwork.IsMasterClient)
            {
                foreach (PlayerJoker pj in GameManager.instance.summonJoker)
                {
                    if (!pj.gameObject.activeSelf || pj.owner != player || !pj.canClick)
                    {
                        continue;
                    }
                    SkillClick();
                }

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
                    int timeTemp = (int)(ConfigDataGame.teleJoker - time + 1);
                    backgroundCountDown.gameObject.SetActive(true);
                    countDownText.text = timeTemp.ToString();
                    countdownUI.fillAmount = time / ConfigDataGame.teleJoker;
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
            foreach (PlayerJoker item in GameManager.instance.summonJoker)
            {
                if (!item.gameObject.activeSelf)
                {
                    continue;
                }
                Utilities.FxButtonPress(imagePress.transform, true);
                Use();
                time = 0;
                countdownUI.fillAmount = 0;
                break;
            }
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
        rpc.photonView.RPC("SwapJokerSkill", RpcTarget.AllBuffered);
    }

}
