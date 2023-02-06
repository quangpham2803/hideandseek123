using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLight : MonoBehaviour
{
    public GameObject hideLight, seekLight;
    PlayerSetup owner;
    // Start is called before the first frame update
    void Start()
    {
        owner = GetComponentInParent<PlayerSetup>();
        if (owner.photonView.IsMine)
        {
            StartCoroutine(WaitToPlay());
        }
    }
    bool isPlaying()
    {
        return GameManager.instance.state == GameManager.GameState.Starting;
    }
    IEnumerator WaitToPlay()
    {
        yield return new WaitUntil(isPlaying);
        if(owner.player == GameManager.HideOrSeek.Hide)
        {
            hideLight.SetActive(true);
        }
        else
        {
            seekLight.SetActive(true);
        }
    }
}
