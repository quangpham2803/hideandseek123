using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharButton : MonoBehaviour
{
    public Image charImg;
    public Text charName;
    public string describe;
    public bool isUsed;
    public GameObject useCheck;
    public bool isHide;
    public GameObject model;
    // Start is called before the first frame update
    void Awake()
    {
        useCheck.SetActive(false);
        describe = "Comming soon...";
    }
    public void ChoseChar()
    {
        if(isHide == true)
        {
            CharManager.instance.describeTextHide.text = describe;
            if (isUsed == false && describe != "Comming soon...")
            {
                CharManager.instance.btnHideChose.isUsed = false;
                CharManager.instance.btnHideChose.useCheck.SetActive(false);
                CharManager.instance.hideModel.SetActive(false);
                CharManager.instance.btnHideChose = this;
                isUsed = true;
                useCheck.SetActive(true);
                CharManager.instance.isHideModelShow = false;
                CharManager.instance.hideModel = model;
                CharManager.instance.SwithModel();

            }
        }
        else
        {
            CharManager.instance.describeTextSeek.text = describe;
            if (isUsed == false && describe != "Comming soon...")
            {
                CharManager.instance.btnSeekChose.isUsed = false;
                CharManager.instance.btnSeekChose.useCheck.SetActive(false);
                CharManager.instance.seekModel.SetActive(false);
                CharManager.instance.btnSeekChose = this;
                isUsed = true;
                useCheck.SetActive(true);
                CharManager.instance.isHideModelShow = true;
                CharManager.instance.seekModel = model;
                CharManager.instance.SwithModel();
            }
        }
    }
}
