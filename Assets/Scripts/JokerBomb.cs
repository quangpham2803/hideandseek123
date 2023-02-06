using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerBomb : MonoBehaviour
{
    public PlayerSetup owner;
    public float speed;
    private void OnEnable()
    {
        StartCoroutine(IEWait());
    }
    private void Update()
    {
        transform.Translate(Vector3.forward.normalized* speed * Time.deltaTime);
    }
    IEnumerator IEWait()
    {
        yield return new WaitForSeconds(0.4f);
        foreach (JokerToxic item in GameManager.instance.jokerToxic)
        {
            if (item.owner == owner)
            {
                item.gameObject.SetActive(true);
                item.transform.position = transform.position;
            }
        }
        gameObject.SetActive(false);
    }
}
