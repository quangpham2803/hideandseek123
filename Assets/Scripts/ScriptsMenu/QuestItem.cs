using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestItem : MonoBehaviour
{
    public GameObject unfinish, finish;
    public int match, winMatch;
    public int rewardCoin, rewardCup;
    public bool hasClaimed;
    public GameObject claimBtn;
    public string mission,describe;
    public TMPro.TextMeshProUGUI missionName, missionName2;
    public TMPro.TextMeshProUGUI missionDescribe, missionDescribe2;
    public TMPro.TextMeshProUGUI numberFinished;
    public TMPro.TextMeshProUGUI numberRequire;
    public TMPro.TextMeshProUGUI numberRewardCoin, numberRewardCoin2;
    public TMPro.TextMeshProUGUI numberRewardCup, numberRewardCup2;
    public GameObject rewardCoinPanel, rewardCoinPanel2;
    public GameObject rewardCupPanel, rewardCupPanel2;
    public int questId;
    public void SetUpQuest(int numberMatch, int numberWin, int coin, int cup, string mission2, string describe2)
    {
        match = numberMatch;
        winMatch = numberWin;
        rewardCoin = coin;
        rewardCup = cup;
        hasClaimed = false;
        claimBtn.SetActive(true);
        mission = mission2;
        describe = describe2;
        CheckFinish(0, 0);
        if (LocalizationManager.CurrentLanguage == "English")
        {
            missionName.font = StatusPlayer.Instance.fontList[0];
            missionName2.font = StatusPlayer.Instance.fontList[0];
            missionDescribe.font = StatusPlayer.Instance.fontList[0];
            missionDescribe2.font = StatusPlayer.Instance.fontList[0];
        }
        else
        {
            missionName.font = StatusPlayer.Instance.fontList[1];
            missionName2.font = StatusPlayer.Instance.fontList[1];
            missionDescribe.font = StatusPlayer.Instance.fontList[1];
            missionDescribe2.font = StatusPlayer.Instance.fontList[1];
        }
    }
    public void CheckFinish(int numberMatch, int numberWin)
    {
            if (numberMatch >= match && numberWin >= winMatch)
        {
            finish.SetActive(true);
            unfinish.SetActive(false);
            hasClaimed = CheckClaim();
            if(hasClaimed == true)
            {
                claimBtn.SetActive(false);
            }
            missionName2.text = mission;
            missionDescribe2.text = describe;
            if(rewardCoin == 0)
            {
                rewardCoinPanel2.SetActive(false);
            }
            else
            {
                rewardCoinPanel2.SetActive(true);
                numberRewardCoin2.text = rewardCoin.ToString();
            }
            if (rewardCup == 0)
            {
                rewardCupPanel2.SetActive(false);
            }
            else
            {
                rewardCupPanel2.SetActive(true);
                numberRewardCup2.text = rewardCup.ToString();
            }
        }
        else
        {
            unfinish.SetActive(true);
            finish.SetActive(false);
            missionName.text = mission;
            missionDescribe.text = describe;            
            if (rewardCoin == 0)
            {
                rewardCoinPanel.SetActive(false);
            }
            else
            {
                rewardCoinPanel.SetActive(true);
                numberRewardCoin.text = rewardCoin.ToString();
            }
            if (rewardCup == 0)
            {
                rewardCupPanel.SetActive(false);
            }
            else
            {
                rewardCupPanel.SetActive(true);
                numberRewardCup.text = rewardCup.ToString();
            }
            if(match != 0)
            {
                numberRequire.text = "/" + match.ToString();
                numberFinished.text = StatusPlayer.Instance.dataPlayer["match"];
            }
            if(winMatch != 0)
            {
                numberRequire.text = "/" + winMatch.ToString();
                numberFinished.text = StatusPlayer.Instance.dataPlayer["win"];
            }
        }
    }
    public void ClaimMission()
    {
        if (hasClaimed == false)
        {
            //StatusPlayer.Instance.GetSingleData(0);
            //StatusPlayer.Instance.GetSingleData(1);
            StatusPlayer.Instance.Money += rewardCoin;
            StatusPlayer.Instance.cup += rewardCup;
            InventoryManager.inventory.UpdateMoney();
            StatusPlayer.Instance.UpdateDataPlayer(StatusPlayer.Instance.cup, "cup");
            hasClaimed = true;
            claimBtn.SetActive(false);
            QuestManager.quest.UpdateStar();
            SetClaim();
        }
    }
    bool CheckClaim()
    {
        bool tm = false;
        if (questId == 1)
        {
            if(StatusPlayer.Instance.dataPlayer["hasClaim1"] == "1")
            {
                tm = true;
            }
            else
            {
                tm = false;
            }
        }
        else if(questId == 2)
        {

            if (StatusPlayer.Instance.dataPlayer["hasClaim2"] == "1")
            {
                tm = true;
            }
            else
            {
                tm = false;
            }
        }
        else if(questId == 3)
        {
            if (StatusPlayer.Instance.dataPlayer["hasClaim3"] == "1")
            {
                tm = true;
            }
            else
            {
                tm = false;
            }
        }
        else
        {
            if (StatusPlayer.Instance.dataPlayer["hasClaim4"] == "1")
            {
                tm = true;
            }
            else
            {
                tm = false;
            }
        }
        return tm;
    }
    void SetClaim()
    {
        bool tm = false;
        if (questId == 1)
        {
            StatusPlayer.Instance.dataPlayer["hasClaim1"] = "1";
        }
        else if (questId == 2)
        {

            StatusPlayer.Instance.dataPlayer["hasClaim2"] = "1";
        }
        else if (questId == 3)
        {
            StatusPlayer.Instance.dataPlayer["hasClaim3"] = "1";
        }
        else
        {
            StatusPlayer.Instance.dataPlayer["hasClaim4"] = "1";
        }
        StatusPlayer.Instance.UpdateUserData();
    }
}
