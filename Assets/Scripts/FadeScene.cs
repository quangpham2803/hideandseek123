using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScene : MonoBehaviour
{
    public Animator animator;
    public string levelToLoad;
    public void FadeToLevelPhoton()
    {
        animator.SetTrigger("FadeOut");
    }
    public void PhotonLevelLoad(string level)
    {
        levelToLoad = level;
        PhotonNetwork.LoadLevel(levelToLoad);
    }
    public void FadeToLevel(string level)
    {
        levelToLoad = level;
        animator.SetTrigger("FadeOut");
        StartCoroutine(OnFadeComplete());
    }
    public IEnumerator OnFadeComplete()
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(levelToLoad);
    }
    
}
