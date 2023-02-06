using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public Animator animator;
    public GameObject laucher;

    public void FadeToLevel(int indexIndex)
    {
        animator.SetTrigger("FadeOut");
    }
    public void OnFadeComplete()
    {
        laucher.GetComponent<Launcher2>().StartGame();
    }
}
