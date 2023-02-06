using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanDavros : MonoBehaviour
{
    public PlayerSetup owner;
    public float range;
    float time = 0;
    private void OnEnable()
    {
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
        transform.position = owner.transform.position;
        if (time >= 3f)
        {
            foreach (PlayerSetup p in GameManager.instance.players)
            {
                if (p.player == GameManager.HideOrSeek.Hide && p.isSilent)
                {
                    p.isSilent = false;
                    //if (p.photonView.IsMine && !p.isBot)
                    //{
                    //    GameManager.instance.silenceUI.GetComponent<Animator>().SetBool("Active", false);
                    //    GameManager.instance.silenceUI.SetActive(false);
                    //}
                }
            }
            gameObject.SetActive(false);
        }
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            if (p.player == GameManager.HideOrSeek.Hide)
            {
                if (Vector3.Distance(p.transform.position, owner.transform.position) <= range && time<3f)
                {
                    //if (p.photonView.IsMine && !p.isBot)
                    //{
                    //    GameManager.instance.silenceUI.SetActive(true);
                    //    GameManager.instance.silenceUI.GetComponent<Animator>().SetBool("Active", true);
                    //}
                    if (!p.isSilent && !p.inGrass)
                    {
                        p.AddDebuffText("SILENT",1);
                        p.isSilent = true;
                    }
                }
                else
                {
                    //if (p.photonView.IsMine && !p.isBot)
                    //{
                    //    GameManager.instance.silenceUI.GetComponent<Animator>().SetBool("Active", false);
                    //    GameManager.instance.silenceUI.SetActive(false);
                    //}
                    p.RemoveDebuffText();
                    p.isSilent = false;
                }    
            }
        }
       

    }
}
