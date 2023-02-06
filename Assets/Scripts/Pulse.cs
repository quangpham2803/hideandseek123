using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public float range;
    public float timeFear;
    public float rangemax;
    private bool temp;
    public PlayerSetup owner;
    public Animator animator;
    private void OnEnable()
    {
        temp = false;
        StartCoroutine(IEDisapear());
    }
    private void Start()
    {
    }
    private void Update()
    {
        if (!temp)
        {
            foreach (PlayerSetup p in GameManager.instance.players)
            {
                if (p.player == GameManager.HideOrSeek.Hide && Vector3.Distance(transform.position, p.transform.position) <= range)
                {
                    p.StartCoroutine(p.SlowTimeAfterRun(2, p, p.slowEffectAffterRunningObj, p.currentSpeed * 0.4f));
                    //foreach (GameObject r in GameManager.instance.effectRaven)
                    //{
                    //    if (r.GetComponent<FearEffect>().owner == p && !p.inGrass && !p.invisible)
                    //    {
                    //        r.SetActive(true);
                    //    }
                    //}
                    //p.photonView.RPC("AddFearEffectToObjectInvisible", Photon.Pun.RpcTarget.AllBuffered);
                }
            }
            temp = true;
        }
    }

    IEnumerator IEDisapear()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
