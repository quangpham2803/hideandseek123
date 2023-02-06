using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrint : MonoBehaviour
{
    public float time;
    public Animator anim;
    private void OnEnable()
    {
        time = FootPrintManager.manager.timePrinter;
        StartCoroutine(Fade());
    }
    IEnumerator Fade()
    {
        anim.Play("footfade");
        yield return new WaitForSeconds(time);
        FootPrintManager.manager.notUse.Add(this);
        FootPrintManager.manager.inUse.Remove(this);
        gameObject.SetActive(false);
    }
}
