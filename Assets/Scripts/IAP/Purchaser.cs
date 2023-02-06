using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
using UnityEngine.Purchasing.Security;


public class Purchaser : MonoBehaviour, IStoreListener
{
    public static Purchaser instance;
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    CrossPlatformValidator validator;
    // Product identifiers for all products capable of being purchased:
    // "convenience" general identifiers for use with Purchasing, and their store-specific identifier
    // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers
    // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)
    // General product identifiers for the consumable, non-consumable, and subscription products.
    // Use these handles in the code to reference which product to purchase. Also use these values
    // when defining the Product Identifiers on the store. Except, for illustration purposes, the
    // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
    // specific mapping to Unity Purchasing's AddProduct, below.
    public static string kProductIDRemoveAds = "com.heallios.hideandseek.remove_ads";
    public static string kProductIDpack1_5usd = "com.heallios.hideandseek.pack_currency_1.5";
    public static string kProductIDpack4usd = "com.heallios.hideandseek.pack_currency_4";
    public static string kProductIDpack12usd = "com.heallios.hideandseek.pack_currency_12";
    public static string kProductIDdailyoffer1 = "com.heallios.hideandseek.iap_daily_offer_1";
    // Apple App Store-specific product identifier for the subscription product.
    private static string kProductNameAppleConsumable = "";
    private static string kProductNameAppleNonConsumable = "com.choosecircle.alphabet.KIDPediaFull";
    private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";
    // Google Play Store-specific product identifier subscription product.
    private static string kProductNameGooglePlayConsumable = "com.unity3d.subscription.original";
    private static string kProductNameGooglePlayNonConsumable = "com.unity3d.subscription.original";
    private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
        //		if (m_StoreController.products.WithID(kProductIDNonConsumable).hasReceipt) {
        //			iap.IAPBought ();
        //		 }
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Add a product to sell / restore by way of its identifier, associating the general identifier
        // with its store-specific identifiers.
        //		builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
        // Continue adding the non-consumable product.
        builder.AddProduct(kProductIDRemoveAds, ProductType.Consumable, new IDs() {
            { kProductNameAppleNonConsumable, AppleAppStore.Name },
            { kProductNameGooglePlayNonConsumable, GooglePlay.Name}
        });
        builder.AddProduct(kProductIDpack1_5usd, ProductType.Consumable, new IDs() {
            { kProductNameAppleConsumable, AppleAppStore.Name },
            { kProductNameGooglePlayConsumable, GooglePlay.Name}
        });
        builder.AddProduct(kProductIDpack4usd, ProductType.Consumable, new IDs() {
            { kProductNameAppleConsumable, AppleAppStore.Name },
            { kProductNameGooglePlayConsumable, GooglePlay.Name}
        });
        builder.AddProduct(kProductIDpack12usd, ProductType.Consumable, new IDs() {
            { kProductNameAppleConsumable, AppleAppStore.Name },
            { kProductNameGooglePlayConsumable, GooglePlay.Name}
        });
        builder.AddProduct(kProductIDdailyoffer1, ProductType.Consumable, new IDs() {
            { kProductNameAppleConsumable, AppleAppStore.Name },
            { kProductNameGooglePlayConsumable, GooglePlay.Name}
        });
        // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
        // if the Product ID was configured differently between Apple and Google stores. Also note that
        // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
        // must only be referenced here. 
        //		builder.AddProduct(kProductIDAppleSubscription, ProductType.Subscription);

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }


    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    //	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
    //	{
    //	    bool validPurchase = true; // Presume valid for platforms with no R.V.
    //
    //	    // Unity IAP's validation logic is only included on these platforms.
    //	#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
    //	    // Prepare the validator with the secrets we prepared in the Editor
    //	    // obfuscation window.
    //	    validator = new CrossPlatformValidator(null,
    //	        AppleTangle.Data(), Application.bundleIdentifier);
    //
    //	    try {
    //	        // On Google Play, result has a single product ID.
    //	        // On Apple stores, receipts contain multiple products.
    //	        var result = validator.Validate(e.purchasedProduct.receipt);
    //	        // For informational purposes, we list the receipt(s)
    //			iap.labelCenter.text += "Receipt is valid. Contents:\n";
    //	        foreach (IPurchaseReceipt productReceipt in result) {
    //	            iap.labelCenter.text += productReceipt.productID + "\n";
    //				iap.labelCenter.text += productReceipt.purchaseDate + "\n";
    //				iap.labelCenter.text += productReceipt.transactionID + "\n";
    //	        }
    //	    } catch (IAPSecurityException) {
    //			iap.labelCenter.text += "Invalid receipt, not unlocking content\n";
    //	        validPurchase = false;
    //	    }
    //	#endif
    //
    //	    if (validPurchase) {
    //				iap.IAPBought ();
    //	    }
    //
    //	    return PurchaseProcessingResult.Complete;
    //	}

    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) =>
            {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
        //ReceiptCheck ();
    }

    private Action<string> _OnBuySuccesful;
    private Action _OnBuyFail;
    public void BuyProductById(string productId, Action<string> OnBuySuccesful, Action OnBuyFail)
    {
        _OnBuySuccesful = OnBuySuccesful;
        _OnBuyFail = OnBuyFail;
        BuyProductID(productId);
    }
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // A consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, kProductIDRemoveAds, StringComparison.Ordinal))
        {
            if (_OnBuySuccesful != null)
            {
                _OnBuySuccesful.Invoke(kProductIDRemoveAds);
                _OnBuySuccesful = null;
            }
            else
            {
                Debug.LogError("_OnBuySuccesful  null!!!");
            }
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
        }
        // Or ... a non-consumable product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDpack1_5usd, StringComparison.Ordinal))
        {
            if (_OnBuySuccesful != null)
            {
                _OnBuySuccesful.Invoke(kProductIDpack1_5usd);
                _OnBuySuccesful = null;
            }
            else
            {
                Debug.LogError("_OnBuySuccesful  null!!!");
            }
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            //iap.IAPBought ();
            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        }
        // Or ... a subscription product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDdailyoffer1, StringComparison.Ordinal))
        {
            if (_OnBuySuccesful != null)
            {
                _OnBuySuccesful.Invoke(kProductIDdailyoffer1);
                _OnBuySuccesful = null;
            }
            else
            {
                Debug.LogError("_OnBuySuccesful  null!!!");
            }
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // TODO: The subscription item has been successfully purchased, grant this to the player.
        }
        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDpack4usd, StringComparison.Ordinal))
        {
            if (_OnBuySuccesful != null)
            {
                _OnBuySuccesful.Invoke(kProductIDpack4usd);
                _OnBuySuccesful = null;
            }
            else
            {
                Debug.LogError("_OnBuySuccesful  null!!!");
            }
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
        }
        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDpack12usd, StringComparison.Ordinal))
        {
            if (_OnBuySuccesful != null)
            {
                _OnBuySuccesful.Invoke(kProductIDpack12usd);
                _OnBuySuccesful = null;
            }
            else
            {
                Debug.LogError("_OnBuySuccesful  null!!!");
            }
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
        }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        if (_OnBuyFail != null)
        {
            _OnBuyFail.Invoke();
            _OnBuyFail = null;
        }
        else
        {
            Debug.LogError("_OnBuyFail  null!!!");
        }
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    //public void ReceiptCheck ()
    //{
    //	iap.labelCenter.text = "RECEIPT CHECK\n";

    //	if (Application.platform == RuntimePlatform.Android ||
    //	    Application.platform == RuntimePlatform.IPhonePlayer ||
    //	    Application.platform == RuntimePlatform.OSXPlayer) {

    //		Product[] products = m_StoreController.products.all;
    //		iap.labelCenter.text += products.Length + "\n";
    //		CrossPlatformValidator validator = new CrossPlatformValidator (null, AppleTangle.Data (), Application.bundleIdentifier);
    //		for (int i = 0; i < products.Length; i++) {
    //			iap.labelCenter.text += "Entered for " + i + "\n";
    //			var result = validator.Validate(products[i].receipt);
    //			iap.labelCenter.text += result;
    //			if (result[i].productID == kProductIDNonConsumable) {
    //				iap.labelCenter.text += "Entered if " + i + "\n";
    //				iap.labelCenter.text += result[i].productID + "\n";
    //				iap.labelCenter.text = "IAP SUCESS";
    //				iap.IAPBought();
    //			}
    //		}
    //	}
    //}

    public void ComboPurchaseRestore()
    {
        //BuyNonConsumable();
        RestorePurchases();
    }
}