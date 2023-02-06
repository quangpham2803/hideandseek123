using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunSkill : MonoBehaviourPunCallbacks
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
    }

    public void UICountDown()
    {
        if (countdownUI.fillAmount <= 0)
        {
            countdownUI.gameObject.SetActive(false);
            countDownText.gameObject.SetActive(false);
            backgroundCountDown.gameObject.SetActive(false);
        }
        else
        {
            time += Time.deltaTime;
            countdownUI.gameObject.SetActive(true);
            countDownText.gameObject.SetActive(true);
            int timeTemp = (int)(data.countdown - time + 1);
            backgroundCountDown.gameObject.SetActive(true);
            countDownText.text = timeTemp.ToString();
            countdownUI.fillAmount = (data.countdown - time) / data.countdown;
        }
    }


    public void SkillClick()
    {
        if (countdownUI.fillAmount == 0 && player.dead ==false)
        {
            //Utilities.FxButtonPress(imagePress.transform, true);
            SoundManager.Instance.PlaySound(SoundManager.AUDIOLIST.run);
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
    }
    public void Use()
    {
        time = 0;
        rpc.photonView.RPC("FastRunSkill", RpcTarget.AllBuffered, true);
        rpc.StartCoroutine(rpc.Run());
    }
    
}
