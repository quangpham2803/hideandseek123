using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CloneTransformPlayer : MonoBehaviourPunCallbacks
{
    public int id;
    public TransformSkill skillHost;
    public float time;
    public float currentTime;
    public GameObject exploEffect;
    public bool isExplo;
    public CloneMove movement;
    private void OnEnable()
    {
        id = skillHost.rpc.player.photonView.ViewID;
        if (!CloneManager.manager.cloneList.Contains(this))
        {
            CloneManager.manager.cloneList.Add(this);
        }
        time = CloneManager.manager.cloneDisappearTime;
        currentTime = 0;
        isExplo = false;
        exploEffect.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (currentTime < time)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            if((skillHost.rpc.player.photonView.IsMine && !skillHost.rpc.player.isBot)||
                (skillHost.rpc.player.isBot && PhotonNetwork.IsMasterClient))
            {
                CloneManager.manager.BackToHost(id);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && 
            other.GetComponent<PlayerSetup>() != skillHost.rpc.player &&
            ((other.GetComponent<PlayerSetup>().photonView.IsMine && !other.GetComponent<PlayerSetup>().isBot)||
            (other.GetComponent<PlayerSetup>().isBot && PhotonNetwork.IsMasterClient))&&
            isExplo == false && !other.GetComponent<PlayerSetup>().dead)
        {
            CloneManager.manager.exploClone(id, 
                other.GetComponent<PlayerSetup>().photonView.ViewID, 
                transform.position);
        }
    }

    private void OnDisable()
    {
        currentTime = 0;
        isExplo = false;
        exploEffect.SetActive(false);
    }
}
