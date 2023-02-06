using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintTutorial : MonoBehaviour
{
    public float time;
    public Animator anim;
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        time = FootPrintManagerTutorial.manager.timePrinter;
        StartCoroutine(Fade());
    }
    IEnumerator Fade()
    {
        anim.Play("footfade");
        yield return new WaitForSeconds(time);
        FootPrintManagerTutorial.manager.notUse.Add(this);
        FootPrintManagerTutorial.manager.inUse.Remove(this);
        gameObject.SetActive(false);
    }
}
