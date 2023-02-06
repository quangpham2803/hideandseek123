using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;
#if UNITY_ANDROID
#elif UNITY_IOS
    using UnityEngine.iOS;
#endif
public class ShowAd : MonoBehaviour
{
    public GameObject popupRemoveAds;
    public GameObject rateLobby;
    public GameObject backgroundRate;
    public TextMeshProUGUI textRemoveAds;
    public Button bttRemoveAds;
    private void OnEnable()
    {
        if (StatusPlayer.Instance.firstTime && StatusPlayer.Instance.isRemoveAd == 0 && StatusPlayer.Instance.losing <= 3)
        {
            ShowAds();
        }
        StatusPlayer.Instance.firstTime = true;
        //else if (StatusPlayer.Instance.showRate)
        //{
        //    ShowRate();
        //}
        //ShowRate();
    }
    public void ShowAds()
    {
        IronSourceManager.instance.ShowAd();
    }
    public void ShowRewardAd()
    {
        IronSourceManager.instance.ShowAdOnClick();
    }
    private void Update()
    {
        if (IronSourceManager.instance !=null && IronSourceManager.instance.showRate)
        {
            IronSourceManager.instance.showRate = false;
            ShowRate();
        }
    }
    public void ShowRate()
    {
#if UNITY_ANDROID
        rateLobby.SetActive(true);
        backgroundRate.SetActive(true);
#elif UNITY_IOS
        Device.RequestStoreReview();
#endif
    }
    public void PopupRemoveAds()
    {
        if (StatusPlayer.Instance.isRemoveAd == 1)
        {
            if (textRemoveAds.text != "You have purchase package")
            {
                textRemoveAds.text = "You have purchase package";
            }
            if (bttRemoveAds.gameObject.activeSelf)
            {
                bttRemoveAds.gameObject.SetActive(false);
            }
        }
        popupRemoveAds.SetActive(true);
    }
}
