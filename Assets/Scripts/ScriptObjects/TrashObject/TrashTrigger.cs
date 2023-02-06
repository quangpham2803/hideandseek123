using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTrigger : MonoBehaviour
{
    public TrashObject owner;
    public int way;
    private void OnTriggerEnter(Collider other)
    {
        if(owner.isFall == false && other.CompareTag("Player"))
        {
            TrashObjectManager.manager.HitTrashCan(owner.id, way);
        }
    }
}
