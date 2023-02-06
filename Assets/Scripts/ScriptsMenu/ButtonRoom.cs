using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRoom : MonoBehaviour
{
    public bool isClick;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        isClick = false;
        anim.SetBool("isClick", false);
    }

    public void Click()
    {
        if (!isClick)
        {
            anim.SetBool("isClick", true);
            isClick = true;
        }
        else
        {
            anim.SetBool("isClick", false);
            isClick = false;
        }
    }
}
