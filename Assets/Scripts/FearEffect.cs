using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearEffect : MonoBehaviour
{
    public PlayerSetup owner;
    private void OnEnable()
    {
        StartCoroutine(FearTime(3f));
    }
    private void Update()
    {
        if(owner.isFear == true)
        {
            transform.position = owner.transform.position;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    IEnumerator FearTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
