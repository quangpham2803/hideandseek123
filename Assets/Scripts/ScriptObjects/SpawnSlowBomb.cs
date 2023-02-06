using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSlowBomb : MonoBehaviour
{
    public PlayerSetup owner;
    public List<Transform> point;

    public void Spawn()
    {
        StartCoroutine(SpawnTime());
    }
    IEnumerator SpawnTime()
    {
        foreach (Transform p in point)
        {
            SlowBombManager.manager.AddBomb(owner, p.position);
        }
        yield return new WaitForSeconds(1f);
        foreach (Transform p in point)
        {
            SlowBombManager.manager.AddBomb(owner, p.position);
        }
        yield return new WaitForSeconds(1f);
        foreach (Transform p in point)
        {
            SlowBombManager.manager.AddBomb(owner, p.position);
        }
        yield return new WaitForSeconds(1f);
        foreach (Transform p in point)
        {
            SlowBombManager.manager.AddBomb(owner, p.position);
        }
        yield return new WaitForSeconds(1f);
        foreach (Transform p in point)
        {
            SlowBombManager.manager.AddBomb(owner, p.position);
        }
    }
}
