using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveTimeArrow : MonoBehaviour
{
    public Indicator arrow;
    public Text timeTxt;
    public Animator anim;
    // Update is called once per frame
    void Update()
    {
        if (arrow.revive != null)
        {
            string seconds = (arrow.revive.timeDisappear - arrow.revive.currentTimeDisappear % 60).ToString("00");
            timeTxt.text = seconds + "s";
            if ((arrow.revive.timeDisappear - arrow.revive.currentTimeDisappear) < 16)
            {
                anim.SetBool("15s", true);
            }
        }
        else if (arrow.revive1 != null)
        {
            string seconds = (arrow.revive1.timeDisappear - arrow.revive1.currentTimeDisappear % 60).ToString("00");
            timeTxt.text = seconds + "s";
            if ((arrow.revive1.timeDisappear - arrow.revive1.currentTimeDisappear) < 16)
            {
                anim.SetBool("15s", true);
            }
        }
    }
}
