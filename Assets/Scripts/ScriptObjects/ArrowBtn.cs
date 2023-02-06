using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBtn : MonoBehaviour
{
    public Indicator arrow;
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        if (arrow.btnGame != null)
        {
            if (arrow.btnGame.isActive == true)
            {
                anim.SetBool("isActive", true);
            }
            else
            {
                anim.SetBool("isActive", false);
            }
        }

    }
}
