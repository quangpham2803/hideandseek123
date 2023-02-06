using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingToxic : MonoBehaviour
{
    public PlayerSetup owner;
    public bool enable;
    bool t;
    private void Update()
    {
        if (!enable && !t)
        {
            t = true;
            StartCoroutine(PingDuration());
        }
    }
    IEnumerator PingDuration()
    {
        yield return new WaitForSeconds(4f);
        foreach (PingToxic ping in GameManager.instance.pingToxic)
        {
            if (ping.gameObject.activeSelf)
            {
                ping.gameObject.SetActive(false);
            }
        }
        gameObject.SetActive(false);
    }
}
