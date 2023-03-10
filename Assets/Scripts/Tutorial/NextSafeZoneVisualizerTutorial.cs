using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSafeZoneVisualizerTutorial : MonoBehaviour
{
    public void UpdateZone(ZoneTutorial.Timer timer)
    {
        if (ZoneTutorial.Instance != null)
        {
            transform.position = ZoneTutorial.Instance.NextSafeZone.Position;
            Vector3 tm = Vector3.one * ZoneTutorial.Instance.NextSafeZone.Radius * 2;
            tm.y = 5;
            transform.localScale = tm;
        }
        else
        {
            Debug.Log("Reset map");
            StartCoroutine(waitForNewGame(timer));
            StartCoroutine(SentDataForClient(transform.position, transform.localScale));
        }
    }
    IEnumerator waitForNewGame(ZoneTutorial.Timer timer)
    {
        yield return new WaitUntil(haveZone);
        UpdateZone(timer);
    }
    bool haveZone()
    {
        return ZoneTutorial.Instance != null;
    }
    void SentDataForClientCmd(Vector3 position, Vector3 size)
    {
        transform.position = position;
        transform.localScale = size;
    }
    IEnumerator SentDataForClient(Vector3 position, Vector3 size)
    {
        SentDataForClientCmd(position, size);
        yield return new WaitForSeconds(10f);
        StartCoroutine(SentDataForClient(transform.position, transform.localScale));
    }
}
