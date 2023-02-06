using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSeekNearTutorial : MonoBehaviour
{
    public AudioSource audio;
    public PlayerSetupTutorial owner;
    bool isPlaySound;
    void Start()
    {
        Physics.IgnoreCollision(owner.GetComponent<Collider>(), GetComponent<Collider>());
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerSetupTutorial>().player == GameManagerTutorial.HideOrSeek.Seek)
        {
            if (owner.cam != null)
            {
                owner.cam.GetComponent<Animator>().SetBool("isSeekNear", true);
                if (isPlaySound == false)
                {
                    audio.Play();
                    isPlaySound = true;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerSetupTutorial>().player == GameManagerTutorial.HideOrSeek.Seek && !owner.dead)
        {
            if (owner.cam != null)
            {
                owner.cam.GetComponent<Animator>().SetBool("isSeekNear", true);
                if (isPlaySound == false)
                {
                    audio.Play();
                    isPlaySound = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerSetupTutorial>().player == GameManagerTutorial.HideOrSeek.Seek)
        {
            if (owner.cam != null)
            {
                owner.cam.GetComponent<Animator>().SetBool("isSeekNear", false);
                audio.Stop();
                isPlaySound = false;
            }
        }
    }
}
