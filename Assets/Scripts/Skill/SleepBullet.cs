using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepBullet : MonoBehaviour
{
    public GameObject effect;
    public GameObject lockEffect;
    public WizardObject wizardObj;
    public GameObject boxKnock;
    private void Start()
    {
        boxKnock.SetActive(true);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Seek)
        {
            other.GetComponent<PlayerSetup>().photonView.RPC("FearPlayer", Photon.Pun.RpcTarget.AllBuffered, 3f);
            foreach (PlayerSetup item in GameManager.instance.players)
            {
                if (other.GetComponent<PlayerSetup>().inGrass && item.player == GameManager.HideOrSeek.Seek)
                {

                }
                else
                {
                    wizardObj.LockEffectActive(other.transform);
                    other.GetComponent<PlayerSetup>().photonView.RPC("AddFearEffectToObjectInvisible", Photon.Pun.RpcTarget.AllBuffered);
                }
            }
            StartCoroutine(waitToHit());            
        }
    }
    IEnumerator waitToHit()
    {
        boxKnock.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        boxKnock.SetActive(false);
        gameObject.SetActive(false);
    }
}
