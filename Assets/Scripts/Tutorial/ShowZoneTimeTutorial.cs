using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowZoneTimeTutorial : MonoBehaviour
{
    public TMPro.TextMeshProUGUI TimeText;
    public Image greenImg, redImg;
    public TMPro.TextMeshProUGUI ringText;
    public TMPro.TextMeshProUGUI alertText;
    public GameObject alertObject;
    public bool isAlert;
    // Method for showing shrinking timer
    public void ShowShrinkingTime(ZoneTutorial.Timer timer)
    {
        redImg.gameObject.SetActive(true);
        greenImg.gameObject.SetActive(false);
        ringText.text = (ZoneTutorial.Instance.CurStep + 1).ToString();
        TimeText.text = "<color=red>" + (timer.EndTime - timer.CurrentTime).ToString("F0") + "</b></color>";
        if (isAlert == false)
        {
            isAlert = true;
            alertCmd();
        }
    }

    // Method for clearing text on end of last circle/step
    public void EndOfLastStep(ZoneTutorial.Timer timer) { TimeText.text = ""; }

    // Method for showing waiting timer before shrinking
    public void ShowWaitingTime(ZoneTutorial.Timer timer)
    {
        greenImg.gameObject.SetActive(true);
        redImg.gameObject.SetActive(false);
        ringText.text = (ZoneTutorial.Instance.CurStep + 1).ToString();
        TimeText.text = (timer.EndTime - timer.CurrentTime).ToString("F0");
        if ( isAlert == true)
        {
            isAlert = false;
            alert2Cmd((float)(ZoneTutorial.Instance.CurStep + 1));
        }
    }
    IEnumerator alertTime()
    {
        if (GameManagerTutorial.instance.scene == GameManagerTutorial.isScene.Scene2 && GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step1)
        {
            yield return new WaitForSeconds(1.2f);
            GameManagerTutorial.instance.tutorial = GameManagerTutorial.SceneTutorial1.Step2;
        }
        isAlert = true;
        alertObject.SetActive(true);
        alertObject.GetComponent<Animator>().Play("zonescaleanim");
        alertText.text = "RING CLOSING !!!";
        alertText.color = Color.red;
        yield return new WaitForSeconds(2.5f);
        alertObject.SetActive(false);
    }
    IEnumerator alertTime2(float time)
    {

        isAlert = false;
        alertObject.SetActive(true);
        alertObject.GetComponent<Animator>().Play("zonescaleanim");
        if (time < 4)
        {
            alertText.text = "RING " + time + "/"+ZoneTutorial.Instance.StepsToEnd/*"/4"*//*(Zone.Instance.CurStep + 1)*/;
        }
        else
        {
            alertText.text = "LAST RING";
        }
        alertText.color = Color.white;
        yield return new WaitForSeconds(2.5f);
        alertObject.SetActive(false);
    }
    
    void alertCmd()
    {
        StartCoroutine(alertTime());
    }
    void alert2Cmd(float time)
    {
        StartCoroutine(alertTime2(time));
    }
}

