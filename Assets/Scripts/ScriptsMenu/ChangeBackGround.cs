using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeBackGround : MonoBehaviour
{
    public Image imgBack;
    public Sprite[] spriteBackList;
    // Start is called before the first frame update
    void Start()
    {
        int i = Random.RandomRange(0, spriteBackList.Length);
        imgBack.sprite = spriteBackList[i];
    }
}
