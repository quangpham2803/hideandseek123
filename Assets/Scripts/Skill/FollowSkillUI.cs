using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowSkillUI : MonoBehaviourPunCallbacks
{
    public PlayerSetup player;
    public PlayerRPC rpc;
    public SkillData skillData;
    private RunSkillData data;
    private float time;
    public Image countdownUI;
    public Text consume;
    //private int timeConsume;
    public Image imagePress;
    public Text countDownText;
    public GameObject haloBig;
    public GameObject haloSmall;
    public GameObject backgroundCountDown;
    public bool isNear;
    public Sprite canUse;
    public Sprite cantUse;
    public Image skillImg;
    bool isFirstTime;
    private void Start()
    {
        player = GetComponent<UIOwner>().owner;
        skillData = GetComponent<UIOwner>().owner.GetComponent<SkillData>();
        rpc = GetComponent<UIOwner>().owner.rpc;
        data = skillData.runSkillData;
        if (!player.photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
        isNear = false;
        isFirstTime = false;
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
            countdownUI.gameObject.SetActive(true);
            countDownText.gameObject.SetActive(true);
            if (isFirstTime == true)
            {
                int timeTemp = (int)(data.countdown - time + 1);
                backgroundCountDown.gameObject.SetActive(true);
                countDownText.text = timeTemp.ToString();
                countdownUI.fillAmount = (data.countdown - time) / data.countdown;
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


    public void SkillClick()
    {
        if (countdownUI.fillAmount == 0 && player.dead == false && player.isJumping == false && isNear == true)
        {
            //Utilities.FxButtonPress(imagePress.transform, true);
            //SoundManager.Instance.PlaySound(SoundManager.AUDIOLIST.run);
            Use();
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
        if (countdownUI.fillAmount == 0 && player.dead == false)
        {
            foreach (PlayerSetup p in GameManager.instance.players)
            {
                if (p.player == GameManager.HideOrSeek.Seek && Vector3.Distance(player.transform.position, p.transform.position) <= 20)
                {
                    isNear = true;
                    break;
                }
                else
                {
                    isNear = false;
                }                
            }
        }
        if(isNear == false)
        {
            skillImg.sprite = cantUse;
        }
        else
        {
            skillImg.sprite = canUse;
        }

    }
    public void Use()
    {
        time = 0;
        rpc.photonView.RPC("FollowSkill", RpcTarget.AllBuffered, 3f , 15f, 1f);
    }

}
