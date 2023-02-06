using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashWater : MonoBehaviour
{
    public TrashObject owner;
    public int id;
    public int timeUse;
    private void Start()
    {
        timeUse = 5;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.GetComponent<PlayerSetup>().dead)
        {
            //timeUse--;
            TrashObjectManager.manager.Slide(owner.id, id, other.GetComponent<PlayerSetup>().photonView.ViewID,timeUse);
        }        
    }
}
