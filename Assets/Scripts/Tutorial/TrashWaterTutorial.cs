using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashWaterTutorial : MonoBehaviour
{
    public TrashObjectTutorial owner;
    public int id;
    public int timeUse;
    private void Start()
    {
        timeUse = 5;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.GetComponent<PlayerSetupTutorial>().dead)
        {
            //timeUse--;
            TrashObjectManagerTutorial.manager.Slide(owner, id, other.GetComponent<PlayerSetupTutorial>(), timeUse);
        }
    }
}
