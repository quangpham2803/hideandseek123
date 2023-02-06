using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bandage : MonoBehaviourPun
{
    public PlayerSetup owner;
    public float speed;
    public bool hitObject;
    public bool hitPlayer;
    public float timeDuration;
    public GameObject effect;
    public int id;
    private void OnEnable()
    {
        if (!BandageManager.manager.bandageList.Contains(this))
        {
            BandageManager.manager.bandageList.Add(this);
        }
        hitObject = false;
        hitPlayer = false;
        effect.SetActive(false);
        foreach (LineRender item in GameManager.instance.lineRenderer)
        {
            if (item.owner == owner)
            {
                owner.speedHook = 0f;
                item.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
                item.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
                item.gameObject.SetActive(true);
                break;
            }
        }
        
        //if ((owner.photonView.IsMine && !owner.isBot) || (owner.isBot && PhotonNetwork.IsMasterClient))
        //{
        //    BandageManager.manager.IEWaitBandage(id);
        //}
        StartCoroutine(IEWait());
    }
    public void StopToss()
    {
        //foreach (LineRender item in GameManager.instance.lineRenderer)
        //{
        //    if (item.owner == owner)
        //    {
        //        item.gameObject.SetActive(false);
        //    }
        //}
        //owner.pAnimator.SetBool("Toss", false);
        //owner.speedHook = 1f;
        //gameObject.SetActive(false);
        if ((owner.photonView.IsMine && !owner.isBot) || (owner.isBot && PhotonNetwork.IsMasterClient))
        {
            BandageManager.manager.StopToss(id);
        }
    }
    private void FixedUpdate()
    {
        if (owner.slide.isSliding || owner.isAttack)
        {
            StopToss();
            return;
        }
        if (!hitObject && !hitPlayer)
        {
            transform.Translate(Vector3.forward.normalized * speed * Time.deltaTime);
        }
        else
        {
            IsHitted();
        }
        foreach (LineRender item in GameManager.instance.lineRenderer)
        {
            if (item.owner == owner)
            {
                item.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                item.GetComponent<LineRenderer>().SetPosition(1, owner.transform.position);
            }
        }
    }
    void IsHitted()
    {
        owner.transform.LookAt(transform.position);
        owner.transform.position = Vector3.MoveTowards(owner.transform.position, transform.position, speed * Time.deltaTime);
        //if (!owner.isBot && owner.photonView.IsMine)
        //{
        //    if (Vector3.Distance(owner.transform.position, transform.position) < 0.3f)
        //    {
        //        TableManager.manager.BanDage(owner.photonView.ViewID);
        //    }
        //}
        //else if (owner.isBot && PhotonNetwork.IsMasterClient)
        //{
        //    if (Vector3.Distance(owner.transform.position, transform.position) < 0.3f)
        //    { 
        //        TableManager.manager.BanDage(owner.photonView.ViewID);
        //    }
        //}
        if (PhotonNetwork.IsMasterClient)
        {
            if (Vector3.Distance(owner.transform.position, transform.position) < 0.3f)
            {
                BandageManager.manager.IsHitted(id, transform.position, owner.transform.position);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.state != GameManager.GameState.Playing)
        {
            return;
        }
        if (other.CompareTag("Player") && !other.GetComponent<PlayerSetup>().dead && other.GetComponent<PlayerSetup>().photonView.IsMine)
        { 
            if (!hitObject)
            {
                hitPlayer = true;
                //PlayerSetup player = other.GetComponent<PlayerSetup>();
                //effect.SetActive(true);
                //effect.transform.position = transform.position;
                //if (player != owner)
                //{
                //    player.rpc.photonView.RPC("StunSeek", RpcTarget.AllBuffered, player.photonView.ViewID);
                //}
                BandageManager.manager.IsStunPlayer(id, other.GetComponent<PlayerSetup>().photonView.ViewID, transform.position);
            }
            
        }
        if (other.CompareTag("wall") || other.CompareTag("prop") || other.CompareTag("table") && PhotonNetwork.IsMasterClient)
        {
            if (!hitPlayer)
            {
                hitObject = true;
                BandageManager.manager.HitWall(id, transform.position);
            }
            //effect.SetActive(true);
            //effect.transform.position = transform.position;
        }
        
    }
    IEnumerator IEWait()
    {
        yield return new WaitForSeconds(timeDuration);
        if (!hitObject && !hitPlayer)
        {
            if (!owner.isBot && owner.photonView.IsMine)
            {
                TableManager.manager.BanDage(owner.photonView.ViewID);
            }
            else if (owner.isBot && PhotonNetwork.IsMasterClient)
            {
                TableManager.manager.BanDage(owner.photonView.ViewID);
            }
        }
    }
    private void OnDisable()
    {
        foreach (LineRender item in GameManager.instance.lineRenderer)
        {
            if (item.owner == owner)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
