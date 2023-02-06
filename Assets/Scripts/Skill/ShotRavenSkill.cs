using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotRavenSkill : MonoBehaviour
{
    public PlayerSetup player;
    public PlayerRPC rpc;
    public Image imagePress;
    public Image countdownUI;
    private void OnEnable()
    {
        time = 0;
    }
    private void Start()
    {
        player = GetComponent<UIOwner>().owner;
        rpc = GetComponent<UIOwner>().owner.rpc;
        gameObject.SetActive(false);
        if (!player.photonView.IsMine || player.isBot)
        {
            gameObject.SetActive(false);
        }

    }

    public void SkillClick()
    {
        if (player.dead == false && player.isFollow == false && player.isbeStun == false)
        {
            //Utilities.FxButtonPress(imagePress.transform, true);
            Use();
        }
    }
    float time;
    private void Update()
    {
        if (!player.photonView.IsMine)
        {
            return;
        }
        if (player.useSkill2 && player.isBot && PhotonNetwork.IsMasterClient)
        {
            player.useSkill2 = false;
            SkillClick();
        }
        time += Time.deltaTime;
        countdownUI.fillAmount =(ConfigDataGame.ravenTimeShot - time) / ConfigDataGame.ravenTimeShot;
        //UICountDown();
    }
    public void Use()
    {
        Vector3 temp = player.summonDarkRaven.transform.position;
        Vector3 pos = new Vector3(temp.x, player.transform.position.y, temp.z);
        rpc.photonView.RPC("ShotRaven", RpcTarget.AllBuffered, pos);
    }
}
