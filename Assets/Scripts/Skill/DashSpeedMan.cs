using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DashSpeedMan : MonoBehaviour
{
    public Transform target;
    public Collider triggerBox;
    public bool isDash;
    public PlayerSetup owner;
    public RunSkillData data;
    float currentTime;
    public float timeDash;
    public PlayerRPC rpc;
    public GameObject smoke;
    public bool isNearWall;

    public GameObject blackHole;
    void Start()
    {
        smoke.SetActive(false);
        isDash = false;
        owner = GetComponentInParent<PlayerSetup>();
        rpc = owner.rpc;
        isNearWall = false;
        rpc.isBlackHoleOn = false;
    }
    void Update()
    {
        if(owner != null)
        {
            if (isDash == true)
            {
                if (owner.dead == false && !owner.isbeStun)
                {
                    owner.isMoving = true;
                    float step = data.improveSpeed * Time.deltaTime;
                    owner.transform.position = Vector3.MoveTowards(owner.transform.position, target.position, step);
                    currentTime += Time.deltaTime;
                    if (currentTime > timeDash)
                    {
                        DisableDash();
                    }
                }
                else
                {
                    DisableDash();
                }
            }
            if (rpc.isDash)
            {
                smoke.SetActive(true);
            }
            else
            {
                smoke.SetActive(false);
            }
            if (rpc.isBlackHoleOn == true)
            {
                blackHole.SetActive(true);
                blackHole.transform.parent = null;
            }
            else
            {
                blackHole.SetActive(false);
                blackHole.transform.SetParent(transform);
                blackHole.transform.position = transform.position;
            }
        }

    }
    public void Use()
    {       
        isDash = true;
        currentTime = 0;
        owner.speedDash = 0;
        rpc.photonView.RPC("ActiveDash", RpcTarget.AllBuffered, true);
    }
    public void Use2()
    {
        rpc.photonView.RPC("SummonBlackHole", RpcTarget.AllBuffered);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(isDash == true)
        {
            if (other.CompareTag("Player") && owner.dead == false && owner.photonView.IsMine && other.GetComponent<PlayerSetup>().dead == false)
            {
                DisableDash();
                rpc.photonView.RPC("StunSeek", RpcTarget.AllBuffered, other.GetComponent<PlayerSetup>().photonView.ViewID);
            }
            if (other.CompareTag("table"))
            {
                DisableDash();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("wall") || other.CompareTag("prop"))
        {
            isNearWall = true;          
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("wall") || other.CompareTag("prop") || other.CompareTag("Player"))
        {
            isNearWall = false;
        }
    }
    void DisableDash()
    {
        isDash = false;
        currentTime = 0;
        rpc.SetDefaultMaterial(owner);
        owner.speedDash = 1;
        owner.isMoving = false;
        rpc.photonView.RPC("ActiveDash", RpcTarget.AllBuffered, false);
    }

}
