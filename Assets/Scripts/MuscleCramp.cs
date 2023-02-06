
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleCramp : MonoBehaviour
{
    public PlayerSetup owner;
    private bool temp;
    private void OnEnable()
    {
        StartCoroutine(TimeDuration());
        temp = false;
    }

    void Update()
    {
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            if (p.player == GameManager.HideOrSeek.Hide && !p.isSlow/*&& Vector3.Distance(p.transform.position, owner.transform.position) <= range*/)
            {
                foreach (GameObject r in GameManager.instance.detectedEffect)
                {
                    if (!r.activeSelf)
                    {
                        //r.SetActive(true);
                        p.StartCoroutine(p.SlowTime(4f, p, r,3f));
                        if (!temp)
                        {
                            temp = true;
                            StartCoroutine(FearTime(2f,p));
                        }
                        //r.transform.position = p.transform.position;
                        //r.transform.SetParent(p.transform);
                        break;
                    }
                }
            }
        }
    }

    public IEnumerator FearTime(float time, PlayerSetup p)
    {
        p.isFear = true;
        yield return new WaitForSeconds(time);
        p.isFear = false;
    }
    IEnumerator TimeDuration()
    {
        yield return new WaitForSeconds(4f);
        foreach (GameObject r in GameManager.instance.detectedEffect)
        {
            if (r.activeSelf)
            {
                r.SetActive(false);
            }
        }
        gameObject.SetActive(false);
    }
}
