using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharManager : MonoBehaviour
{
    public static CharManager instance;
    [Header("Hide")]
    public CharContain[] charHide;
    public Sprite[] charHideImg;
    public string[] charHidename;
    public string[] describeHide;
    public GameObject[] modelHide;
    public bool isAddHide;
    public Text describeTextHide;
    public Text describeTextSeek;
    [Header("Seek")]
    public CharContain[] charSeek;
    public Sprite[] charSeekImg;
    public string[] charSeekname;
    public string[] describeSeek;
    public GameObject[] modelSeek;
    public bool isAddSeek;
    [Header("Chosing Hide")]
    public int idHideChose;
    public GameObject hideModel;
    public CharButton btnHideChose;
    [Header("Chosing Seek")]
    public int idSeekChose;
    public GameObject seekModel;
    public CharButton btnSeekChose;
    public bool isHideModelShow;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        isAddHide = false;
        isAddSeek = false;
        modelHide[0].SetActive(true);
        hideModel = modelHide[0];
        charHide[0].listBtn[0].isUsed = true;
        btnHideChose = charHide[0].listBtn[0];
        charSeek[0].listBtn[0].isUsed = true;
        btnSeekChose = charSeek[0].listBtn[0];
        seekModel = modelSeek[0];
        isHideModelShow = true;
    }
    public void AddDataHide()
    {
        if(isAddHide == false)
        {
            charHide[0].listBtn[0].useCheck.SetActive(true);            
            int tm = 0;
            int tm2 = 0;
            for (int i = 0; i < charHidename.Length; i++)
            {
                if (tm == 3)
                {
                    tm2++;
                    tm = 0;
                }
                charHide[tm2].listBtn[tm].charImg.sprite = charHideImg[i];
                charHide[tm2].listBtn[tm].charName.text = charHidename[i];
                charHide[tm2].listBtn[tm].describe = describeHide[i];
                charHide[tm2].listBtn[tm].model = modelHide[i];
                tm++;
            }
            isAddHide = true;
        }
        seekModel.SetActive(false);
        hideModel.SetActive(true);
        isHideModelShow = true;
    }
    public void AddDataSeek()
    {
        if(isAddSeek == false)
        {
            charSeek[0].listBtn[0].useCheck.SetActive(true);
            int tm3 = 0;
            int tm4 = 0;
            for (int i = 0; i < charSeekname.Length; i++)
            {
                if (tm3 == 3)
                {
                    tm4++;
                    tm3 = 0;
                }
                charSeek[tm4].listBtn[tm3].charImg.sprite = charSeekImg[i];
                charSeek[tm4].listBtn[tm3].charName.text = charSeekname[i];
                charSeek[tm4].listBtn[tm3].describe = describeSeek[i];
                charSeek[tm4].listBtn[tm3].model = modelSeek[i];
                tm3++;
            }
            isAddSeek = true;
        }
        seekModel.SetActive(true);
        hideModel.SetActive(false);
        isHideModelShow = false;
    }
    public void SwithModel()
    {
        if (isHideModelShow)
        {
            seekModel.SetActive(true);
            hideModel.SetActive(false);
            isHideModelShow = false;
        }
        else
        {
            seekModel.SetActive(false);
            hideModel.SetActive(true);
            isHideModelShow = true;
        }
    }
}
