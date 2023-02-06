using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSeekNear : MonoBehaviour
{
    public PlayerSetup owner;
    void Start()
    {
        Physics.IgnoreCollision(owner.collider, GetComponent<Collider>());
        if (owner.isBot)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (owner == GameManager.instance.mainPlayer && owner.dead == false)
        {
            if (other.CompareTag("Player") &&
                other.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Seek && 
                !GameManager.instance.seekNear.activeSelf)
            {
                //owner.cam.GetComponent<Animator>().SetBool("isSeekNear", true);
                GameManager.instance.seekNear.SetActive(true);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (owner == GameManager.instance.mainPlayer && owner.dead == false)
        {
            if (other.CompareTag("Player") 
                && other.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Seek && 
                !GameManager.instance.seekNear.activeSelf)
            {
                GameManager.instance.seekNear.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (owner == GameManager.instance.mainPlayer)
        {
            if (other.CompareTag("Player") && 
                other.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Seek)
            {
                GameManager.instance.seekNear.SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        //owner.cam.GetComponent<Animator>().SetBool("isSeekNear", false);
        if (owner.photonView.IsMine)
        {
            GameManager.instance.seekNear.SetActive(false);
        }
    }
}
