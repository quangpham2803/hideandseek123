using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestManager : MonoBehaviour
{
    public static QuestManager quest;
    public QuestItem[] listQuest;
    public GameObject[] starQuest;
    //public int numberQuestFinish;
    public bool hasClaim;
    public GameObject claimBtn;
    public GameObject checkClaim;
    public GameObject popUpReward;
    public TMPro.TextMeshProUGUI coinText, cupText;
    private void Awake()
    {
        quest = this;
    }
    private void Start()
    {

    }
    public void ResetDaily()
    {
        hasClaim = false;
        claimBtn.SetActive(false);
        checkClaim.SetActive(false);
        foreach (var item in starQuest)
        {
            item.SetActive(false);
        }
        //numberQuestFinish = 0;
        if (LocalizationManager.CurrentLanguage == "English")
        {
            listQuest[0].SetUpQuest(1, 0, 150, 0, "First Game Of Day", "Play 1 game");
            listQuest[1].SetUpQuest(0, 1, 150, 1, "First Win Of Day", "Win 1 game");
            listQuest[2].SetUpQuest(5, 0, 100, 5, "Collect Trophies", "Play 5 times");
            listQuest[3].SetUpQuest(0, 3, 200, 2, "Collect Gold Coins", "Win 3 games");
        }
        else
        {
            listQuest[0].SetUpQuest(1, 0, 150, 0, "Trận đầu tiên", "Chơi 1 ván");
            listQuest[1].SetUpQuest(0, 1, 150, 1, "Trận thắng đầu tiên", "Thắng 1 ván");
            listQuest[2].SetUpQuest(5, 0, 100, 5, "Thu thập huy hiệu", "Chơi 5 ván");
            listQuest[3].SetUpQuest(0, 3, 200, 2, "Thu thập vàng", "Thắng 3 ván");
        }
        PlayerPrefs.SetInt("newday", 0);
        StatusPlayer.Instance.dataPlayer["newday"] = "0";
        StatusPlayer.Instance.UpdateUserData();
        Debug.Log("Reset Daily Quest");
    }
    public void AddDataAll()
    {
        int numberTm = int.Parse(StatusPlayer.Instance.dataPlayer["numberQuestFinish"]);
        if (StatusPlayer.Instance.dataPlayer["hasClaimTotal"] == "1")
        {
            hasClaim = true;
        }
        else
        {
            hasClaim = false;
        }
        if (hasClaim == false && numberTm == 4)
        {
            claimBtn.SetActive(true);
        }
        else
        {
            claimBtn.SetActive(false);
            if(hasClaim == true)
            {
                checkClaim.SetActive(true);
            }
            else
            {
                checkClaim.SetActive(false);
            }
        }
        if (LocalizationManager.CurrentLanguage == "English")
        {
            listQuest[0].SetUpQuest(1, 0, 150, 0, "First Game Of Day", "Play 1 game");
            listQuest[1].SetUpQuest(0, 1, 150, 1, "First Win Of Day", "Win 1 game");
            listQuest[2].SetUpQuest(5, 0, 100, 5, "Collect Trophies", "Play 5 times");
            listQuest[3].SetUpQuest(0, 3, 200, 2, "Collect Gold Coins", "Win 3 games");
        }
        else
        {
            listQuest[0].SetUpQuest(1, 0, 150, 0, "Trận đầu tiên", "Chơi 1 ván");
            listQuest[1].SetUpQuest(0, 1, 150, 1, "Trận thắng đầu tiên", "Thắng 1 ván");
            listQuest[2].SetUpQuest(5, 0, 100, 5, "Thu thập huy hiệu", "Chơi 5 ván");
            listQuest[3].SetUpQuest(0, 3, 200, 2, "Thu thập vàng", "Thắng 3 ván");
        }
        if (numberTm > 0)
        {
            for (int i = 0; i < numberTm; i++)
            {
                starQuest[i].SetActive(true);
            }
        }
        int match = int.Parse(StatusPlayer.Instance.dataPlayer["match"]);
        int win = int.Parse(StatusPlayer.Instance.dataPlayer["win"]);
        AddData(match, win);
    }
    public void UpdateStar()
    {
        int numberTm = int.Parse(StatusPlayer.Instance.dataPlayer["numberQuestFinish"]);
        numberTm++;
        if (numberTm > 0)
        {
            for(int i = 0; i < numberTm; i++)
            {
                starQuest[i].SetActive(true);
            }
        }
        if (hasClaim == false && numberTm == 4)
        {
            claimBtn.SetActive(true);
        }
        StatusPlayer.Instance.dataPlayer["numberQuestFinish"] = numberTm.ToString();
        StatusPlayer.Instance.UpdateUserData();
    }
    public void AddData(int match, int win)
    {
        foreach(var item in listQuest)
        {
            item.CheckFinish(match, win);
        }
    }
    public void Claim()
    {
        if(hasClaim == false)
        {
            int coin = Random.RandomRange(100, 1000);
            int cup = Random.RandomRange(1, 10);
            coinText.text = coin.ToString();
            cupText.text = cup.ToString();
            popUpReward.SetActive(true);
            StatusPlayer.Instance.GetSingleData(0);
            StatusPlayer.Instance.GetSingleData(1);
            StatusPlayer.Instance.Money += coin;
            InventoryManager.inventory.UpdateMoney();
            StatusPlayer.Instance.cup += cup;
            StatusPlayer.Instance.UpdateDataPlayer(StatusPlayer.Instance.cup, "cup");
            hasClaim = true;
            StatusPlayer.Instance.dataPlayer["hasClaimTotal"] = "1";
            StatusPlayer.Instance.UpdateUserData();
            claimBtn.SetActive(false);
            checkClaim.SetActive(true);
        }
    }
}
