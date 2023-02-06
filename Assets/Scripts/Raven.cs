using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raven : MonoBehaviourPunCallbacks
{
    public float speed;
    public Animator animator;
    public PlayerSetup owner;
    private bool dead;
    public Vector3 offset;
    //public Camera camera;
    private Vector3 inputVector;
    private Rigidbody rigidbody;
    private void OnEnable()
    {
        if (owner == null)
            return;
        if (!owner.isBot)
        {
            owner.cam.cinemachine.LookAt = transform;
            owner.cam.cinemachine.Follow = transform;
            owner.cam.cinemachine.m_Lens.OrthographicSize = 10f;
        }
        isPress = false;
        t = false;
        StartCoroutine(Disapear());
    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        GameManager.instance.summonRaven.Add(this);
        //foreach (PlayerSetup p in GameManager.instance.players)
        //{
        //    if (photonView.Owner == p.photonView.Owner)
        //    {
        //        owner = p;
        //        p.summonDarkRaven = this;
        //        break;
        //    }
        //}
        gameObject.SetActive(false);
    }
    bool t;
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Leave") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.9f)
        {
            gameObject.SetActive(false);
        }
        if (!owner.isBot)
        {
            if (owner.isSummonDarkRaven && owner.photonView.IsMine)
            {
                float horizontal = owner.joystick.Horizontal;
                float vertical = owner.joystick.Vertical;
                inputVector = new Vector3(-horizontal * 6f, rigidbody.velocity.y, -vertical * 6f);
                if (inputVector != Vector3.zero)
                {
                    transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));
                }
                rigidbody.velocity = inputVector;
            }
            else if (!owner.isSummonDarkRaven && owner.photonView.IsMine)
            {
                foreach (GameObject item in owner.skill)
                {
                    if (item.GetComponent<ShotRavenSkill>() != null)
                    {
                        item.SetActive(false);
                    }
                    else
                    {
                        item.SetActive(true);
                    }
                }
            }
            if (isPress && !t)
            {
                t = true;
                owner.agentPlayer.enabled = false;
                owner.cam.cinemachine.LookAt = owner.transform;
                owner.cam.cinemachine.Follow = owner.transform;
                owner.cam.cinemachine.m_Lens.OrthographicSize = 6f;
                animator.Play("Leave");
                if (owner.photonView.IsMine)
                {
                    foreach (GameObject item in owner.skill)
                    {
                        if (item.GetComponent<ShotRavenSkill>() != null)
                        {
                            item.SetActive(false);
                        }
                        else
                        {
                            item.SetActive(true);
                        }
                    }
                }
                StartCoroutine(WaitDisapear());
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient && owner.isSummonDarkRaven)
            {
                transform.position = new Vector3(owner.transform.position.x, transform.position.y, owner.transform.position.z);
            }
            else if (PhotonNetwork.IsMasterClient && !owner.isSummonDarkRaven)
            {
                foreach (GameObject item in owner.skill)
                {
                    if (item.GetComponent<ShotRavenSkill>() != null)
                    {
                        item.SetActive(false);
                    }
                    else
                    {
                        item.SetActive(true);
                    }
                }
            }
            if (isPress && !t && PhotonNetwork.IsMasterClient)
            {
                t = true;
                animator.SetBool("Leave", true);
                foreach (GameObject item in owner.skill)
                {
                    if (item.GetComponent<ShotRavenSkill>() != null)
                    {
                        item.SetActive(false);
                    }
                    else
                    {
                        item.SetActive(true);
                    }
                }
                StartCoroutine(WaitDisapear());
            }
        }
        
    }
    public void Disapear2()
    {
        gameObject.SetActive(false);
    }
    IEnumerator WaitDisapear()
    {
        owner.speedUp += 1;
        yield return new WaitForSeconds(1f);
        owner.speedUp -= 1;
    }
    [PunRPC]
    public void TimeOut()
    {
        if (!owner.isBot)
        {
            owner.agentPlayer.enabled = false;
            owner.cam.cinemachine.LookAt = owner.transform;
            owner.cam.cinemachine.Follow = owner.transform;
            owner.cam.cinemachine.m_Lens.OrthographicSize = 6f;
        }
        owner.isSummonDarkRaven = false;
        foreach (Pulse r in GameManager.instance.shotRaven)
        {
            if (r.owner == owner)
            {
                r.gameObject.SetActive(true);
                Vector3 temp = transform.position;
                Vector3 pos = new Vector3(temp.x, owner.transform.position.y, temp.z);
                r.transform.position = pos;
                r.owner.isSummonDarkRaven = false;
            }
        }
        animator.Play("Leave");
    }
    public bool isPress;
    public IEnumerator Disapear()
    {
        yield return new WaitForSeconds(5f);
        if (!isPress)
        {
            photonView.RPC("TimeOut", RpcTarget.AllBuffered);
        }
    }
}
