using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffBox : MonoBehaviour
{
    public int id;
    public GameObject body;
    public bool isWait;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isWait == false)
        {
            SpawnRandomBox.manager.ConsumeBoxDebuff(other.GetComponent<PlayerSetup>().photonView.ViewID, id);
        }
    }
    public void Cooldown()
    {
        StartCoroutine(WaitToActiveAgain());
    }
    IEnumerator WaitToActiveAgain()
    {
        isWait = true;
        body.SetActive(false);
        yield return new WaitForSeconds(10f);
        isWait = false;
        body.SetActive(true);
    }
}
