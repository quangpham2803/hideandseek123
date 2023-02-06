using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using I2.Loc;

public class SettingMenu : MonoBehaviour
{
    public static SettingMenu setting;
    public TMPro.TextMeshProUGUI QuailyTxt;
    public int deviceMem;
    public int deviceMemNeeded;
    public int deviceMemHight;
    public int isHight;
    public int isShadow;
    public int hideId;
    public int seekId;
    public int id;
    private void Awake()
    {
        if(setting == null)
        {
            setting = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("isFirstTime") == 0)
        {
            if(Application.systemLanguage == SystemLanguage.Vietnamese)
            {
                LocalizationManager.CurrentLanguage = "Viet Nam";
            }
            else
            {
                LocalizationManager.CurrentLanguage = "English";
            }
            FirstTimeSettingQuality();
            FirstTimeSettingInventory();
            SceneManager.LoadScene("Scene1");
        }
        else
        {
            GetQuality();
            GetInventory();
        }

        AddQuality();
    }

    void FirstTimeSettingQuality()
    {
        deviceMem = SystemInfo.systemMemorySize;
        if (deviceMem < deviceMemNeeded)
        {
            isHight = 0;
            isShadow = 0;
        }
        else if(deviceMemNeeded <= deviceMem && deviceMem < deviceMemHight)
        {
            isHight = 0;
            isShadow = 1;
        }
        else
        {
            isHight = 1;
            isShadow = 1;
        }
        PlayerPrefs.SetInt("isFirstTime", 1);
        PlayerPrefs.SetInt("isHight", isHight);
        PlayerPrefs.SetInt("isShadow", isShadow);       
    }
    public void OnMusic()
    {
        PlayerPrefs.SetInt("Music", 0);
    }
    public void OffMusic()
    {
        PlayerPrefs.SetInt("Music", 1);
    }
    public void FirstTimeSettingInventory()
    {
        PlayerPrefs.SetInt("hideId", 2);
        PlayerPrefs.SetInt("seekId", 0);
        hideId = 2;
        seekId = 0;
        InventoryManager.inventory.energy = 100;
        PlayerPrefs.SetFloat("energy", 100);
        InventoryManager.inventory.hideChar = new int[]{0,0,1};
        InventoryManager.inventory.seekChar = new int[]{1, 0, 0};
        InventoryManager.inventory.SetCharList();
        InventoryManager.inventory.AddText();
        InventoryManager.inventory.UpdateExp();
    }
    public void FirstTimeSettingFriends()
    {
        PlayerPrefsX.GetStringArray("FriendList");
    }
    void GetQuality()
    {
        isHight = PlayerPrefs.GetInt("isHight");
        isShadow = PlayerPrefs.GetInt("isShadow");
        hideId = PlayerPrefs.GetInt("hideId");
        seekId = PlayerPrefs.GetInt("seekId");
    }
    void GetInventory()
    {
        InventoryManager.inventory.GetCharList();
        InventoryManager.inventory.AddText();
        InventoryManager.inventory.UpdateExp();
    }
    void AddQuality()
    {
        if (isHight == 0 && isShadow == 0)
        {
            if (LocalizationManager.CurrentLanguage == "English")
            {
                QuailyTxt.text = "LOW";
                QuailyTxt.font = StatusPlayer.Instance.fontList[0];
            }
            else
            {
                QuailyTxt.text = "Thấp";
                QuailyTxt.font = StatusPlayer.Instance.fontList[1];
            }
            
            id = 0;
        }
        else if (isHight == 0 && isShadow == 1)
        {
            if (LocalizationManager.CurrentLanguage == "English")
            {
                QuailyTxt.text = "MEDIUM";
                QuailyTxt.font = StatusPlayer.Instance.fontList[0];
            }
            else
            {
                QuailyTxt.text = "Trung bình";
                QuailyTxt.font = StatusPlayer.Instance.fontList[1];
            }
            
            id = 1;
        }
        else
        {
            if (LocalizationManager.CurrentLanguage == "English")
            {
                QuailyTxt.text = "HIGH";
                QuailyTxt.font = StatusPlayer.Instance.fontList[0];
            }
            else
            {
                QuailyTxt.text = "Cao";
                QuailyTxt.font = StatusPlayer.Instance.fontList[1];
            }
            
            id = 2;
        }
    }
    public void NextQuality()
    {
        id++;
        if(id > 2)
        {
            id = 0;
        }
        UpdateQuality();
    }
    public void BackQuality()
    {
        id--;
        if (id < 0)
        {
            id = 2;
        }
        UpdateQuality();
    }
    void UpdateQuality()
    {
        if(id == 0)
        {
            PlayerPrefs.SetInt("isHight", 0);
            PlayerPrefs.SetInt("isShadow", 0);
            isHight = 0;
            isShadow = 0;
        }
        else if( id == 1)
        {
            PlayerPrefs.SetInt("isHight", 0);
            PlayerPrefs.SetInt("isShadow", 1);
            isHight = 0;
            isShadow = 1;
        }
        else if( id == 2)
        {
            PlayerPrefs.SetInt("isHight", 1);
            PlayerPrefs.SetInt("isShadow", 1);
            isHight = 1;
            isShadow = 1;
        }
        AddQuality();
    }
}
