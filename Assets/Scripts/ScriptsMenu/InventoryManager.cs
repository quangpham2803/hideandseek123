using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager inventory;
    public int[] hideChar;
    public int[] seekChar;
    public TMPro.TextMeshProUGUI moneyTxt;
    public Text gemTxt;
    public Image energyFiller;
    public TMPro.TextMeshProUGUI energyText;
    public float energy;
    float timeCheck;
    public Image expFiller;
    public TMPro.TextMeshProUGUI Leveltext;
    public CharactorMenu charMenu;
    public GameObject btnPlayReal;
    public GameObject btnPlayBot;
    public TMPro.TextMeshProUGUI cupText;
    private void Awake()
    {
        if(inventory == null)
        {
            inventory = this;
        }
        timeCheck = 0;
    }
    public void CheckDayPass()
    {
        energy = PlayerPrefs.GetFloat("energy");
        if (energy < 100)
        {
            double realtime = TimeSpan.FromTicks(StatusPlayer.GetNetTime().Ticks).TotalSeconds;
            double lasttime = TimeSpan.FromTicks(StatusPlayer.Instance.lastTime.Ticks).TotalSeconds;
            double timeSleep = realtime - lasttime;
            Debug.Log("Time Sleep : " + timeSleep);
            energy += (float)(timeSleep * 0.0333333333333333f);
        }
        else
        {
            energy = 100;            
        }
        energyText.text = ((int)energy).ToString() + "/100";
    }
    public void AddText()
    {
        moneyTxt.text = StatusPlayer.Instance.Money.ToString();
    }
    public void UpdateMoney()
    {
        StatusPlayer.Instance.UpdateDataPlayer(StatusPlayer.Instance.Money, "Money");
        AddText();
    }
    public void SetCharList()
    {
        PlayerPrefsX.SetIntArray("hideChar", hideChar);
        PlayerPrefsX.SetIntArray("seekChar", seekChar);
    }
    public void GetCharList()
    {
        if(PlayerPrefsX.GetIntArray("hideChar").Length != 0)
        {
            if(PlayerPrefsX.GetIntArray("hideChar").Length < charMenu.hiderModels.Length)
            {
                int tm = charMenu.hiderModels.Length - PlayerPrefsX.GetIntArray("hideChar").Length;
                List<int> listTm = PlayerPrefsX.GetIntArray("hideChar").ToList();
                for(int i = 0; i< tm; i++)
                {
                    listTm.Add(0);
                }
                PlayerPrefsX.SetIntArray("hideChar", listTm.ToArray());
            }
            if (PlayerPrefsX.GetIntArray("seekChar").Length < charMenu.seekerModels.Length)
            {
                int tm = charMenu.seekerModels.Length - PlayerPrefsX.GetIntArray("seekChar").Length;
                List<int> listTm = PlayerPrefsX.GetIntArray("seekChar").ToList();
                for (int i = 0; i < tm; i++)
                {
                    listTm.Add(0);
                }
                PlayerPrefsX.SetIntArray("seekChar", listTm.ToArray());
            }
            hideChar = PlayerPrefsX.GetIntArray("hideChar");
            seekChar = PlayerPrefsX.GetIntArray("seekChar");
            hideChar = PlayerPrefsX.GetIntArray("hideChar");
        }
        else
        {
            SettingMenu.setting.FirstTimeSettingInventory();
        }
    }
    private void Update()
    {
        if(energy < 100)
        {
            timeCheck += Time.deltaTime;
            if(timeCheck >= 1)
            {
                energy += 0.0333333333333333f;
                energyText.text = ((int)energy).ToString() + "/100";
                timeCheck = 0;
            }
        }
        if(energy > 100)
        {
            energy = 100;
            energyText.text = ((int)energy).ToString() + "/100";
        }
    }
    public void SaveTimeBeforeStartGame()
    {
        energy -= ConfigEnergyShop.energyCost;
        PlayerPrefs.SetFloat("energy", energy);
        StatusPlayer.Instance.numberMatch += 1;
        StatusPlayer.Instance.UpdateMultiDataPlayer(-99, 1, -99, -99, StatusPlayer.Instance.numberMatch, -99, -99, -99, -99, -99, 1,-99, -99);
        StatusPlayer.Instance.SaveLastTime();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause == true)
        {
            PlayerPrefs.SetFloat("energy", energy);
        }
    }

    public void UpdateExp()
    {
        expFiller.fillAmount = PlayerPrefs.GetFloat("expFiller");
        Leveltext.text = StatusPlayer.Instance.levelPlayer.ToString();
        if(StatusPlayer.Instance.levelPlayer >= 5)
        {
            btnPlayReal.SetActive(true);
            btnPlayBot.SetActive(false);
        }
        else
        {
            btnPlayReal.SetActive(false);
            btnPlayBot.SetActive(true);
        }
    }
    public void BuyEnergy()
    {
        if(energy < 100)
        {
            if (StatusPlayer.Instance.Money >= ConfigEnergyShop.energyPrice)
            {
                Launcher2.Instance.AlertPopup("BUY SUCCESSFULLY!!!");
                StatusPlayer.Instance.Money -= ConfigEnergyShop.energyPrice;
                UpdateMoney();
                energy += ConfigEnergyShop.energyGift;               
                if(energy >= 100)
                {
                    energy = 100;
                    Launcher2.Instance.popupBuyEnergy.SetActive(false);
                }
                energyText.text = ((int)energy).ToString() + "/100";
                PlayerPrefs.SetFloat("energy", energy);
            }
            else
            {
                Launcher2.Instance.AlertPopup("NOT ENOUGHT MONEY");
            }
        }
        else 
        {
            Launcher2.Instance.AlertPopup("YOUR ENERGY IS FULL");
            Launcher2.Instance.popupBuyEnergy.SetActive(false);
        }
    }
}
