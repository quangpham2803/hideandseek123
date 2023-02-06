using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
public class ResultMenu : MonoBehaviour
{
    public Image filler;
    public Text currentLevelTxt;
    public Text nextLevelTxt;
    public Text LevelUpTxt;
    public Text rewardTxt;
    public Animator anim;
    public bool isBonus;  
    bool isMoreExp;
    bool isFullExp;


    //Reward
    public int exp;
    public Text expTxt;
    public int expBonus;
    public Text expBonusTxt;
    public int gold;
    public Text goldTxt;
    public int goldBonus;
    public Text goldBonusTxt;
    public Text cupTxt;
    public int cupWin;    
    public int cupLose;
    // Start is called before the first frame update
    void Start()
    {
        GetDataFromConfig();
        float currentExp = PlayerPrefs.GetFloat("playerEXP");
        float requirExp = (float)(StatusPlayer.Instance.levelPlayer*50 + StatusPlayer.Instance.levelPlayer*0.1*25);
        int currentLevel = StatusPlayer.Instance.levelPlayer;
        int nextLevel = StatusPlayer.Instance.levelPlayer + 1;
        float timePlay = 390 - GameManager.instance.currentMatchTime;
        filler.fillAmount = 0;
        StatusPlayer.Instance.numberMatchFinish += 1;
        if (isBonus)
        {
            StatusPlayer.Instance.winning += 1;
            PlayerPrefs.SetInt("winning", StatusPlayer.Instance.winning);
            int i = int.Parse(StatusPlayer.Instance.dataPlayer["match"]);
            StatusPlayer.Instance.dataPlayer["match"] = (i+1).ToString();
            int j = int.Parse(StatusPlayer.Instance.dataPlayer["win"]);
            StatusPlayer.Instance.dataPlayer["win"] = (j + 1).ToString();
            //gold
            StatusPlayer.Instance.Money += (int)(gold + (timePlay / 10) + goldBonus);
            goldTxt.text = ((int)(gold + timePlay / 10)).ToString();
            goldBonusTxt.text = "+      " + goldBonus.ToString();

    //exp
            currentExp += exp + expBonus;
            expTxt.text = exp.ToString();
            expBonusTxt.text = "+      " + expBonus.ToString();

            //cup
            cupTxt.text = cupWin.ToString();
            StatusPlayer.Instance.cup += cupWin;
            StatusPlayer.Instance.numberMatchWin += 1;


            //Update value win
            if (GameManager.instance.mainPlayer.player == GameManager.HideOrSeek.Hide)
            {
                StatusPlayer.Instance.numberHideWin += 1;
            }
            else
            {
                StatusPlayer.Instance.numberSeekWin += 1;
            }
            StatusPlayer.Instance.losing = 0;
        }
        else
        {
            StatusPlayer.Instance.winning = 0;
            PlayerPrefs.SetInt("winning", StatusPlayer.Instance.winning);
            int i = int.Parse(StatusPlayer.Instance.dataPlayer["match"]);
            StatusPlayer.Instance.dataPlayer["match"] = (i + 1).ToString();
            //gold
            StatusPlayer.Instance.Money += (int)(gold + (timePlay / 10));
            goldTxt.text = ((int)(gold + (timePlay / 10))).ToString();

            //exp
            currentExp += 50;
            expTxt.text = exp.ToString();

            //cup
            cupTxt.text = cupLose.ToString();
            StatusPlayer.Instance.cup += cupLose;
            StatusPlayer.Instance.losing += 1;
        }
        //Level player
        currentLevelTxt.text = currentLevel.ToString();
        nextLevelTxt.text = nextLevel.ToString();
        LevelUpTxt.text = nextLevel.ToString();
        if (currentExp >= requirExp)
        {
            Debug.Log("Level up");
            anim.SetBool("isLevelUp", true);
            PlayerPrefs.SetFloat("playerEXP", currentExp - requirExp);
            StatusPlayer.Instance.levelPlayer = nextLevel;
            PlayerPrefs.SetFloat("expFiller", PlayerPrefs.GetFloat("playerEXP") / requirExp);
            isFullExp = true;
            RewardLevel();
        }
        else
        {
            anim.SetBool("isLevelUp", false);
            PlayerPrefs.SetFloat("playerEXP", currentExp);
            PlayerPrefs.SetFloat("expFiller", PlayerPrefs.GetFloat("playerEXP") / requirExp);
        }
        //Update data for player
        StatusPlayer.Instance.UpdateMultiDataPlayer(StatusPlayer.Instance.cup, 1, StatusPlayer.Instance.Money, 
            StatusPlayer.Instance.levelPlayer, StatusPlayer.Instance.numberMatch, StatusPlayer.Instance.numberMatchFinish, 
            StatusPlayer.Instance.numberMatchWin, StatusPlayer.Instance.numberHideWin, StatusPlayer.Instance.numberSeekWin,
            - 99, -99, StatusPlayer.Instance.losing, -99);
        StatusPlayer.Instance.UpdateUserData();

    }
    void RewardLevel()
    {
        int level = StatusPlayer.Instance.levelPlayer;
        float du = level % 5;
        if (du > 0)
        {
            rewardTxt.text = "+ 100 gold";
            StatusPlayer.Instance.Money += 100;
        }
        else
        {
            rewardTxt.text = "+ 150 gold";
            StatusPlayer.Instance.Money += 150;
        }
    }
    void GetDataFromConfig()
    {
        gold = ConfigPriceAndReward.gold;
        goldBonus = ConfigPriceAndReward.goldBonus;
        exp = ConfigPriceAndReward.exp;
        expBonus = ConfigPriceAndReward.expBonus;
        cupWin = ConfigPriceAndReward.cupWin;
        cupLose = ConfigPriceAndReward.cupLose;
    }
}
