using I2.Loc;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.SharedModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using I2.Loc;
public class CharactorMenu : MonoBehaviour
{
    [Header("Main")]
    public bool isSeekerPage;
    public Image skillImg1, skillImg2;
    public TMPro.TextMeshProUGUI skillTxt1;
    public TMPro.TextMeshProUGUI nameTxt;
    public int currentId;
    public GameObject btnUse;
    public GameObject btnBuy;
    public TMPro.TextMeshProUGUI price;
    public GameObject lockImg, btnCantuse;
    [Header("Hider")]
    public GameObject[] hiderModels;
    public string[] hidenameChar;
    public string[] hidenameChar2;
    public Sprite[] hideskill1Img;
    public string[] hideskill1Des;
    public string[] hideskill2Des;
    public int[] priceHide;
    public string[] hideId;
    [Header("Seeker")]
    public GameObject[] seekerModels;
    public string[] seeknameChar;
    public string[] seeknameChar2;
    public Sprite[] seekskill1Img;
    public string[] seekskill1Des;
    public string[] seekskill2Des;
    public int[] priceSeek;
    public string[] seekId;
    public SettingMenu setting;
    public ShopManager shopGold;
    // Start is called before the first frame update
    void Start()
    {
        GetInventory();
        isSeekerPage = false;
        currentId = 0;
        btnBuy.SetActive(false);
        btnUse.SetActive(false);
        StartCoroutine(GetPrice());       
    }
    public void Next()
    {
        if (isSeekerPage)
        {
            if (currentId < seekerModels.Length-1)
            {
                currentId++;
                AddInfo();
            }
            else
            {
                currentId = 0;
                AddInfo();
            }
        }
        else
        {
            if (currentId < hiderModels.Length - 1)
            {
                currentId++;
                AddInfo();
            }
            else
            {
                currentId = 0;
                AddInfo();
            }
        }

    }
    public void Back()
    {
        if(currentId > 0)
        {
            currentId--;
            AddInfo();
        }
        else
        {
            if (isSeekerPage)
            {
                currentId = seekerModels.Length - 1;
                AddInfo();
            }
            else
            {
                currentId = hiderModels.Length - 1;
                AddInfo();               
            }
        }
    }
    public void Switch()
    {
        currentId = 0;
        isSeekerPage = !isSeekerPage;
        AddInfo();
    }
    public void Use()
    {
        if (isSeekerPage)
        {
            PlayerPrefs.SetInt("seekId", currentId);
            setting.seekId = PlayerPrefs.GetInt("seekId");
        }           
        else
        {
            PlayerPrefs.SetInt("hideId", currentId);
            setting.hideId = PlayerPrefs.GetInt("hideId");
        }
        btnUse.SetActive(false);
    }
    public void Buy()
    {
        StatusPlayer.Instance.GetSingleData(1);
        if (isSeekerPage)
        {
            if(priceSeek[currentId] <= StatusPlayer.Instance.Money)
            {
                StatusPlayer.Instance.Money -= priceSeek[currentId];
                InventoryManager.inventory.seekChar[currentId] = 1;
                //InventoryManager.inventory.SetCharList();
                InventoryManager.inventory.UpdateMoney();
                btnUse.SetActive(true);
                btnBuy.SetActive(false);
                btnCantuse.SetActive(false);
                lockImg.SetActive(false);
                MakePurchase(seekId[currentId]);
            }
            else
            {
                //Launcher2.Instance.AlertPopup("NOT ENOUGHT MONEY");\
                shopGold.ShopOnClick();
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            if (priceHide[currentId] <= StatusPlayer.Instance.Money)
            {
                StatusPlayer.Instance.Money -= priceHide[currentId];
                InventoryManager.inventory.hideChar[currentId] = 1;
                //InventoryManager.inventory.SetCharList();
                InventoryManager.inventory.UpdateMoney();
                btnUse.SetActive(true);
                btnBuy.SetActive(false);
                btnCantuse.SetActive(false);
                lockImg.SetActive(false);
                MakePurchase(hideId[currentId]);
            }
            else
            {
                //Launcher2.Instance.AlertPopup("NOT ENOUGHT MONEY");
                shopGold.ShopOnClick();
                this.gameObject.SetActive(false);
            }
        }
    }
    void AddInfo()
    {
        if(isSeekerPage == false)
        {
            if(currentId < hiderModels.Length)
            {
                foreach (GameObject p in seekerModels)
                {
                    p.SetActive(false);
                }
                for (int i = 0; i < hiderModels.Length; i++)
                {
                    if (i == currentId)
                    {
                        hiderModels[i].SetActive(true);
                    }
                    else
                    {
                        hiderModels[i].SetActive(false);
                    }
                }
                skillImg1.sprite = hideskill1Img[currentId];

                
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    nameTxt.text = hidenameChar[currentId];
                    nameTxt.font = StatusPlayer.Instance.fontList[0];
                }
                else
                {
                    nameTxt.text = hidenameChar2[currentId];
                    nameTxt.font = StatusPlayer.Instance.fontList[1];
                }
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    skillTxt1.text = hideskill1Des[currentId];
                    skillTxt1.font = StatusPlayer.Instance.fontList[0];
                }
                else
                {
                    skillTxt1.text = hideskill2Des[currentId];
                    skillTxt1.font = StatusPlayer.Instance.fontList[1];
                }
                
                Debug.Log(InventoryManager.inventory.hideChar.Length);
                if(CheckIsHaveChar(hideId[currentId]) || priceHide[currentId] == 0)
                {
                    btnBuy.SetActive(false);
                    btnCantuse.SetActive(false);
                    lockImg.SetActive(false);
                    if (currentId == PlayerPrefs.GetInt("hideId"))
                    {
                        btnUse.SetActive(false);
                    }
                    else
                    {
                        btnUse.SetActive(true);
                    }
                }
                else
                {
                    btnBuy.SetActive(true);
                    btnCantuse.SetActive(true);
                    lockImg.SetActive(true);
                    try
                    {
                        price.text = priceHide[currentId].ToString();
                    }
                    catch
                    {
                        btnBuy.SetActive(false);
                        btnCantuse.SetActive(false);
                        btnUse.SetActive(false);
                    }
                }
            }            
        }
        else
        {
            if (currentId < seekerModels.Length)
            {
                foreach (GameObject p in hiderModels)
                {
                    p.SetActive(false);
                }
                for (int i = 0; i < seekerModels.Length; i++)
                {
                    if (i == currentId)
                    {
                        seekerModels[i].SetActive(true);
                    }
                    else
                    {
                        seekerModels[i].SetActive(false);
                    }
                }
                skillImg1.sprite = seekskill1Img[currentId];
                
                
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    nameTxt.text = seeknameChar[currentId];
                    nameTxt.font = StatusPlayer.Instance.fontList[0];
                }
                else
                {
                    nameTxt.text = seeknameChar2[currentId];
                    nameTxt.font = StatusPlayer.Instance.fontList[1];
                }
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    skillTxt1.text = seekskill1Des[currentId];
                    skillTxt1.font = StatusPlayer.Instance.fontList[0];
                }
                else
                {
                    skillTxt1.text = seekskill2Des[currentId];
                    skillTxt1.font = StatusPlayer.Instance.fontList[1];
                }  
                
                if (CheckIsHaveChar(seekId[currentId]) || priceSeek[currentId] == 0)
                {
                    btnBuy.SetActive(false);
                    btnCantuse.SetActive(false);
                    lockImg.SetActive(false);
                    if (currentId == PlayerPrefs.GetInt("seekId"))
                    {
                        btnUse.SetActive(false);
                    }
                    else
                    {
                        btnUse.SetActive(true);
                    }
                }
                else
                {
                    btnBuy.SetActive(true);
                    btnCantuse.SetActive(true);
                    lockImg.SetActive(true);
                    try
                    {
                        price.text = priceSeek[currentId].ToString();
                    }
                    catch
                    {
                        btnBuy.SetActive(false);
                        btnCantuse.SetActive(false);
                        btnUse.SetActive(false);
                    }
                }
            }
        }        
    }
    bool isLoggin()
    {
        return Launcher2.Instance.isLoggin;
    }
    IEnumerator GetPrice()
    {
        yield return new WaitForSeconds(0.5f);
        AddPriceFromServer();
        AddInfo();
    }
    public void AddPriceFromServer()
    {
        priceHide[0] = ConfigPriceAndReward.transformPrice;
        priceHide[1] = ConfigPriceAndReward.wizardPrice;
        priceSeek[1] = ConfigPriceAndReward.ravenPrice;
        priceSeek[2] = ConfigPriceAndReward.mummyPrice;
    }
    // You will want to receive a specific result-type for your API, and utilize the result parameters
    void LogSuccess(PlayFabResultCommon result)
    {
        GetInventory();
        Debug.Log("Buy successful");
    }

    // Error handling can be very advanced, such as retry mechanisms, logging, or other options
    // The simplest possible choice is just to log it
    void LogFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
    void MakePurchase(string idChar)
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            // In your game, this should just be a constant matching your primary catalog
            CatalogVersion = "v1.0",
            ItemId = idChar,
            Price = 0,
            VirtualCurrency = "GD"
        }, LogSuccess, LogFailure);
    }
    public List<ItemInstance> itemList;
    void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest() { },
            result =>
            {
                itemList = result.Inventory;
            }
            , LogFailure);
    }
    bool CheckIsHaveChar(string charName)
    {
        bool isHave = false;
        foreach(ItemInstance item in itemList)
        {
            if(item.ItemId == charName)
            {
                isHave = true;
                break;
            }
            else
            {
                isHave = false;
            }
        }
        return isHave;
    }
}
