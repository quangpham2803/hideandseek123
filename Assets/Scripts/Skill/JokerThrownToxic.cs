using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JokerThrownToxic : MonoBehaviour
{
    public PlayerSetup player;
    public PlayerRPC rpc;
    private float time;
    public Image countdownUI;
    public Text countDownText;
    public GameObject haloBig;
    public GameObject haloSmall;
    public GameObject backgroundCountDown;
    private void Start()
    {
        player = GetComponentInParent<PlayerSetup>();
        rpc = GetComponentInParent<PlayerRPC>();
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
            //int timeTemp = (int)(data.countdown - time + 1);
            //backgroundCountDown.gameObject.SetActive(true);
            //countDownText.text = timeTemp.ToString();
            //countdownUI.fillAmount = (data.countdown - time) / data.countdown;
        }
    }

    IEnumerator IEClick()
    {
        haloSmall.SetActive(true);
        haloBig.SetActive(true);
        Use();
        time = 0;
        countdownUI.fillAmount = 1;
        yield return new WaitForSeconds(0.75f);
        haloSmall.SetActive(false);
        haloBig.SetActive(false);
    }
    public void SkillClick()
    {
        if (countdownUI.fillAmount == 0 )
        {
            StartCoroutine(IEClick());
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
        rpc.photonView.RPC("JokerThrownToxic", RpcTarget.AllBuffered/*,player.shotWeapon.transform.forward,player.shotWeapon.transform.rotation*/);
    }
}
