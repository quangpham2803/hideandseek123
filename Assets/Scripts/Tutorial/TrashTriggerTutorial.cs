using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTriggerTutorial : MonoBehaviour
{
    public TrashObjectTutorial owner;
    public int way;
    private void OnTriggerEnter(Collider other)
    {
        if (owner.isFall == false && other.CompareTag("Player"))
        {
            TrashObjectManagerTutorial.manager.HitTrashCan(owner, way);
        }
    }
}
