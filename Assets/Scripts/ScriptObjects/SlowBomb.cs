using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SlowBomb : MonoBehaviour
{
    public PlayerSetup owner;
    public int id;
    private void OnEnable()
    {
        StartCoroutine(waitToDestroy());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerSetup player = other.GetComponent<PlayerSetup>();
            if(player != owner)
            {
                player.ReciveBoxDebuff(0);
                SlowBombManager.manager.AddBombBack(id);
            }
        }
    }
    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(15f);
        SlowBombManager.manager.AddBombBack(id);
    }
}
