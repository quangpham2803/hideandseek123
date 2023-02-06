using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPProductManager : MonoBehaviour
{
    public GameObject popupBougt;
    public GameObject popupBoughtFail;
    public GameObject shopPannel;
    public TextMeshProUGUI bodyText;
    public GameObject boughtImage;
    public string productID = "";
    public TextMeshProUGUI moneyText;
    public GameObject buySuccesfulPopup;
    public TextMeshProUGUI txtgoldReceive;
    public TextMeshProUGUI textDailypack;
    public TextMeshProUGUI textBuyFail;
    private void Start()
    {
        //load event shop
    }

    public void boughtOnClick()
    {
        //if (productID == Purchaser.kProductIDRemoveAds)
        //{
        //    Purchaser.instance.BuyProductById(Purchaser.kProductIDRemoveAds, OnBuyRemoveAdsSuccesful, OnBuyFail);
        //}
        //else if (productID == Purchaser.kProductIDpack1_5usd)
        //{
        //    Purchaser.instance.BuyProductById(Purchaser.kProductIDpack1_5usd, OnBuyPackGoldSuccesful, OnBuyFail);
        //}
        //else if (productID == Purchaser.kProductIDpack12usd)
        //{
        //    Purchaser.instance.BuyProductById(Purchaser.kProductIDpack12usd, OnBuyPackGoldSuccesful, OnBuyFail);
        //}
        //else if (productID == Purchaser.kProductIDpack4usd)
        //{
        //    Purchaser.instance.BuyProductById(Purchaser.kProductIDpack4usd, OnBuyPackGoldSuccesful, OnBuyFail);
        //}
        //else if (productID == Purchaser.kProductIDdailyoffer1)
        //{
        //    Purchaser.instance.BuyProductById(Purchaser.kProductIDdailyoffer1, OnBuyPackDailyOfferSuccesful, OnBuyFail);
        //}
    }
    public void OnBuyFail()
    {
        textBuyFail.text = "You can't buy this package!";
        popupBoughtFail.SetActive(true);
    }
    public void OnDailyPackOnClick()
    {
        if (StatusPlayer.Instance.dailyPack > 0)
        {
         //   productID = Purchaser.kProductIDdailyoffer1;
            bodyText.text = "Will you buy this daily package at a price $ 2?";
            popupBougt.SetActive(true);
        }
        else
        {
            OnBuyFail();
        }
    }
    private void OnBuyPackDailyOfferSuccesful(string id)
    {
        int gold = UnityEngine.Random.Range(300,3001);
        StatusPlayer.Instance.Money += gold;
        StatusPlayer.Instance.dailyPack--;
        StatusPlayer.Instance.UpdateDataPlayer(StatusPlayer.Instance.dailyPack, "dailyPack");
        textDailypack.text = "Lucky Chest<size=40> x"+ StatusPlayer.Instance.dailyPack + "</size>";
        InventoryManager.inventory.UpdateMoney();
        moneyText.text = StatusPlayer.Instance.Money.ToString();
        txtgoldReceive.text = "You receive " + gold + "<size=100><sprite index=0></size>";
        buySuccesfulPopup.SetActive(true);
        popupBougt.SetActive(false);
    }
    public void OnRemoveAdsClick()
    {
        if (StatusPlayer.Instance.isRemoveAd == 0)
        {
        //    productID = Purchaser.kProductIDRemoveAds;
            bodyText.text = "Will you buy this remove ads package at a price $ 3.99?";
            popupBougt.SetActive(true);
        }
    }
    public void OnPackGold1OnClick()
    {
      //  productID = Purchaser.kProductIDpack1_5usd;
        bodyText.text = "Will you buy this gold package at a price $ 1.5?";
        popupBougt.SetActive(true);
    }
    public void OnPackGold2OnClick()
    {
     //   productID = Purchaser.kProductIDpack4usd;
        bodyText.text = "Will you buy this gold package at a price $ 4?";
        popupBougt.SetActive(true);
    }
    public void OnPackGold3OnClick()
    {
      //  productID = Purchaser.kProductIDpack12usd;
        bodyText.text = "Will you buy this gold package at a price $ 12?";
        popupBougt.SetActive(true);
    }
    private void OnBuyPackGoldSuccesful(string id)
    {
        if (id == Purchaser.kProductIDpack1_5usd)
        {
            StatusPlayer.Instance.Money += 1000;
            txtgoldReceive.text = "You receive " + 1000 + "<size=100><sprite index=0></size>";
        }
        else if (id == Purchaser.kProductIDpack4usd)
        {
            StatusPlayer.Instance.Money += 3000;
            txtgoldReceive.text = "You receive " + 3000 + "<size=100><sprite index=0></size>";
        }
        else if (id == Purchaser.kProductIDpack12usd)
        {
            StatusPlayer.Instance.Money += 10000;
            txtgoldReceive.text = "You receive " + 10000 + "<size=100><sprite index=0></size>";
        }
        InventoryManager.inventory.UpdateMoney();
        moneyText.text = StatusPlayer.Instance.Money.ToString();
        buySuccesfulPopup.SetActive(true);
        popupBougt.SetActive(false);
    }
    private void OnBuyRemoveAdsSuccesful(string id)
    {
        StatusPlayer.Instance.isRemoveAd = 1;
        StatusPlayer.Instance.UpdateDataPlayer(1, "isRemoveAd");
        popupBougt.SetActive(false);
        txtgoldReceive.text = "You receive Remove ads";
        buySuccesfulPopup.SetActive(true);
        boughtImage.SetActive(true);
    }
}
