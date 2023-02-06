using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateLobby : MonoBehaviour
{
    public GameObject backgroundRate;
    public void ClickNoThanks()
    {
        backgroundRate.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ClickLater()
    {
        backgroundRate.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ClickRateNow()
    {
        backgroundRate.SetActive(false);
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.heallios.HideAndSeek");
        gameObject.SetActive(false);
    }
    public void RateInHome()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.heallios.HideAndSeek");
#elif UNITY_IOS
        Device.RequestStoreReview();
#endif
    }
}
