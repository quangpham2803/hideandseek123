using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeProp : MonoBehaviour
{
    public TransformSkill skillHost;
    public float time;
    public float currentTime;
    public GameObject exploEffect;
    public bool isExplo;
    // Start is called before the first frame update
    private void OnEnable()
    {
        currentTime = 0;
        isExplo = false;
        exploEffect.SetActive(false);
    }
    private void Update()
    {
        if(currentTime < time)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0;
            transform.SetParent(skillHost.transform);
            transform.position = skillHost.transform.position;
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.GetComponent<PlayerSetup>() != skillHost.rpc.player && isExplo == false && !other.GetComponent<PlayerSetup>().dead)
        {
            StartCoroutine(explo());
            skillHost.rpc.photonView.RPC("StunSeek", RpcTarget.AllBuffered, other.GetComponent<PlayerSetup>().photonView.ViewID);
        }
    }
    IEnumerator explo()
    {
        exploEffect.SetActive(true);
        isExplo = true;
        GetComponent<CloneMove>().isMove = false;
        yield return new WaitForSeconds(1f);
        transform.SetParent(skillHost.transform);
        transform.position = skillHost.transform.position;
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        currentTime = 0;
        isExplo = false;
        exploEffect.SetActive(false);
    }
}
