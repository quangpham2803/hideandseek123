using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class InvisibleSkill : MonoBehaviour
{ 
    public Image countdownUI;
    [SerializeField] TMPro.TextMeshProUGUI currentTimeTransformText;
    private float time;
    public InvisibleSkillData data;
    //public Text consume;
    PlayerSetup player;
    public Image imagePress;
    public Text countDownText;
    public GameObject backgroundCountDown;
    bool isFirstTime;
    PlayerRPC rpc;
    private void Start()
    {
        player = GetComponent<UIOwner>().owner;
        data = GetComponent<UIOwner>().owner.GetComponent<SkillData>().invisibleSkillData;
        rpc = GetComponent<UIOwner>().owner.rpc;
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
        if (countdownUI.fillAmount == 0  && player.dead == false)
        {
            //Utilities.FxButtonPress(imagePress.transform, true);
            Use();
            time = 0;
            countdownUI.fillAmount = 1;
        }
    }
    void Update()
    {
    }
    public void Use()
    {
        rpc.photonView.RPC("Invisible", RpcTarget.AllBuffered);
    }
}
