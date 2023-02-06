using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject boughtImage;
    public GameObject shopPannel;
    public GameObject infoDailyPack;
    public TextMeshProUGUI textDailypack;
    public TextMeshProUGUI resetTimeDailyPack;
    public TextMeshProUGUI moneyText;
    public void ShopOnClick()
    {
        moneyText.text = StatusPlayer.Instance.Money.ToString();
        textDailypack.text = "Lucky Chest<size=40> x" + StatusPlayer.Instance.dailyPack + "</size>";
        shopPannel.SetActive(true);
        if (StatusPlayer.Instance.isRemoveAd == 1)
        {
            boughtImage.SetActive(true);
        }
    }
    public void InfoDailyOnClick()
    {
        infoDailyPack.SetActive(true);
    }

    private void Update()
    {
        if (shopPannel.activeSelf)
        {
            resetTimeDailyPack.text = (24 -DateTime.Now.Hour) + "h " + (60-DateTime.Now.Minute) + "m";
        }
    }
}
