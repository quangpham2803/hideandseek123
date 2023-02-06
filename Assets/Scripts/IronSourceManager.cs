using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IronSourceManager : MonoBehaviour
{
    public static IronSourceManager instance;
    string appKey;
    public bool firstTime;
    public bool showRate;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        if (!firstTime)
        {
            firstTime = true;
            LoadAd();
        }
    }
    void Start()
    {
#if UNITY_ANDROID
         appKey = "e9058061";
#elif UNITY_IOS
        appKey = "e90e0a91";
#endif
        IronSource.Agent.validateIntegration();
        //SDK init
        Debug.Log("unity-script: IronSource.Agent.init");
        IronSource.Agent.init(appKey);
    }
    void OnApplicationPause(bool isPaused)
    {
        Debug.Log("unity-script: OnApplicationPause = " + isPaused);
        IronSource.Agent.onApplicationPause(isPaused);
    }
    #region RewardedAd callback handlers

    void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
    {
        Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
    }
    void RewardedVideoAdOpenedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
    }

    void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount() + " name = " + ssp.getRewardName());

    }

    void RewardedVideoAdClosedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
        if (clickAD)
        {
            clickAD = false;
            InventoryManager.inventory.energy += 20;
            InventoryManager.inventory.energyText.text = ((int)InventoryManager.inventory.energy).ToString() + "/100";
            PlayerPrefs.SetFloat("energy", InventoryManager.inventory.energy);
        }
    }

    void RewardedVideoAdStartedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");

    }

    void RewardedVideoAdEndedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
    }

    void RewardedVideoAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
    }
    void RewardedVideoAdClickedEvent(IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedVideoAdClickedEvent, name = " + ssp.getRewardName());
    }
    #endregion
    //Update is called once per frame
    public void LoadAd()
    {
        Debug.Log("LoadAD");
        IronSource.Agent.loadInterstitial();
    }
    public void ShowAd()
    {
        IronSource.Agent.showInterstitial();
    }
    public bool clickAD;
    public void ShowAdOnClick()
    {
        clickAD = true;
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {

            Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
        }
    }
    #region Interstitial
    //Invoked when the initialization process has failed.
    //@param description - string - contains information about the failure.
    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
    }
    //Invoked right before the Interstitial screen is about to open.
    void InterstitialAdShowSucceededEvent()
    {
    }
    //Invoked when the ad fails to show.
    //@param description - string - contains information about the failure.
    void InterstitialAdShowFailedEvent(IronSourceError error)
    {
    }
    // Invoked when end user clicked on the interstitial ad
    void InterstitialAdClickedEvent()
    {
    }
    //Invoked when the interstitial ad closed and the user goes back to the application screen.
    void InterstitialAdClosedEvent()
    {
        StatusPlayer.Instance.isloadAd = true;
        LoadAd();
        if (StatusPlayer.Instance.winning % 3 == 0 && StatusPlayer.Instance.winning >0)
        {
            showRate = true;

        }
    }
    //Invoked when the Interstitial is Ready to shown after load function is called
    void InterstitialAdReadyEvent()
    {
    }
    //Invoked when the Interstitial Ad Unit has opened
    void InterstitialAdOpenedEvent()
    {
    }
    #endregion
    void OnEnable()
    {
        //Add Rewarded Video Events
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
        //Add Interstitial Ad
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
    }
}
