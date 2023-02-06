using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerToxic : MonoBehaviour
{
    public PlayerSetup owner;
    private void OnEnable()
    {
        StartCoroutine(TimeDuration());
    }
    
    IEnumerator TimeDuration()
    {
        yield return new WaitForSeconds(4f);
        foreach (PingToxic ping in GameManager.instance.pingToxic)
        {
            if (ping.gameObject.activeSelf)
            {
                ping.owner.speedDown = 0f;
                ping.gameObject.SetActive(false);
            }
        }
        gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")&&other.GetComponent<PlayerSetup>().player==GameManager.HideOrSeek.Hide)
        {
            PlayerSetup p = other.GetComponent<PlayerSetup>();
            owner.rpc.SetDefaultMaterial(p);
            p.invisible = false;
            foreach (Invisible i in GameManager.instance.invisibleObject)
            {
                if (!i.gameObject.activeSelf)
                    continue;
                if(i.owner==p)
                {
                    i.gameObject.SetActive(false);
                }
            }
            foreach (PingToxic ping in GameManager.instance.pingToxic)
            {
                if (ping.owner == p)
                {
                    ping.owner.speedDown = 1f;
                    ping.gameObject.SetActive(true);
                    ping.transform.position = p.transform.position;
                    ping.enable = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Hide)
        {
            PlayerSetup p = other.GetComponent<PlayerSetup>();
            p.speedDown = 0f;
        }
    }

}
