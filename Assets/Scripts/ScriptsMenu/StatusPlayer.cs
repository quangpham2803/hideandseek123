using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlayFab;
using PlayFab.ServerModels;
using PlayFab.ClientModels;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.Net;
using System.Globalization;

public class StatusPlayer : MonoBehaviour
{
    public static StatusPlayer Instance;
    public bool isLogging;
    public string MyPlayfabID;
    public DateTime currentDay;
    public DateTime lastTime;
    public TMPro.TMP_FontAsset[] fontList;
    //Data player
    public int cup;
    public int numberMatch;
    public int numberMatchFinish;
    public int numberMatchWin;
    public int numberSeekWin;
    public int numberHideWin;
    public bool showAD;
    public bool showRate;
    public int levelPlayer;
    public int Money;
    public int status;
    public int isRemoveAd;
    public int losing;
    public int dailyPack;
    public bool firstTime;
    public int winning;
    public bool isloadAd = true;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            isLogging = false;
            showRate = false;
            currentDay = GetNetTime();
            status = 0;
            if (!Application.isEditor)
            {
                StartCoroutine(checkCheat());
            }
            winning = PlayerPrefs.GetInt("winning");
            //StartCoroutine(UpdateStatcWithTime());
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void OnApplicationQuit()
    {
        if (isLogging == true)
        {
            SetOnlineOrOffline(0);
            PhotonNetwork.Disconnect();
        }
    }
    private void OnApplicationPause(bool pause)
    {
        Debug.Log(pause);
        if (isLogging == true)
        {
            if (pause == true)
            {
                Debug.Log("Dis");
                SetOnlineOrOffline(0);
                //PhotonNetwork.Disconnect();
            }
            else if (pause == false)
            {
                Debug.Log("Re");
                SetOnlineOrOffline(1);
                //PhotonNetwork.Reconnect();
            }
        }
    }
    bool isReconnect()
    {
        return PhotonNetwork.NetworkingClient.LoadBalancingPeer.PeerState == PeerStateValue.Disconnected;
    }
    public void SetOnlineOrOffline(int i)
    {
        PlayFabServerAPI.UpdatePlayerStatistics(new PlayFab.ServerModels.UpdatePlayerStatisticsRequest
        {
            PlayFabId = MyPlayfabID,
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<PlayFab.ServerModels.StatisticUpdate> {
        new PlayFab.ServerModels.StatisticUpdate { StatisticName = "isOnline", Value = i},
    }
        },
        result =>
        {
            if (i == 0)
            {
                SetingOffline();
            }
        },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }
    IEnumerator UpdateStatcWithTime()
    {
        yield return new WaitForSeconds(5f);
        if (isLogging == true)
        {
            if (Launcher2.Instance != null && !Launcher2.Instance.roomMenu.activeSelf)
            {
                UpdateDataPlayer(0, "status");
            }
            else
            {
                UpdateDataPlayer(1, "status");
            }
        }
        StartCoroutine(UpdateStatcWithTime());
    }
    public void CheckIfDayPass()
    {
        Debug.Log(lastTime);
        int dayoff = lastTime.Day;
        int monthoff = lastTime.Month;
        int yearoff = lastTime.Year;
        if (dayoff != 0 && monthoff != 0 && yearoff != 0)
        {
            if (monthoff == currentDay.Month && yearoff == currentDay.Year)
            {
                if (currentDay.Day - dayoff > 0)
                {
                    Debug.Log("new day");
                    ResetValuePlayer();
                }
            }
            else
            {
                Debug.Log("new day");
                ResetValuePlayer();
            }
        }
        else
        {
            Debug.Log("Start game day");
            ResetValuePlayer();
        }
    }
    public void SetUserLastTime(DateTime date)
    {
        PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
            {"DateTime", date.ToString()},
             {"newday", "1"},
             {"hasClaimTotal", "0"},
             {"hasClaim1", "0"},
             {"hasClaim2", "0"},
             {"hasClaim3", "0"},
             {"hasClaim4", "0"},
             {"numberQuestFinish", "0"},
             {"match", "0"},
             {"win", "0"},
        }
        },
        result => {
            Debug.Log("Successfully updated user data");
        },
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    public void SaveLastTime()
    {
        lastTime = GetNetTime();
        dataPlayer["DateTime"] = lastTime.ToString();
        UpdateUserData();
    }
    public void GetUserLastTime(string myPlayFabeId)
    {
        PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest()
        {
            PlayFabId = myPlayFabeId,
            Keys = null,
        }, result => {
            if (result.Data.Count < 10 || result.Data == null)
            {
                SetUserLastTime(currentDay);
            }
            else
            {
                lastTime = Convert.ToDateTime(result.Data["DateTime"].Value);
            }
            dataPlayer.Clear();
            foreach (var i in result.Data)
            {
                dataPlayer.Add(i.Key, i.Value.Value);
            }
            CheckIfDayPass();

        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    void SetingOffline()
    {
        SaveLastTime();
    }
    public static DateTime GetNetTime()
    {

        return DateTime.Now;
        
        //var myhttpwebrequest = (httpwebrequest)webrequest.create("http://worldtimeapi.org/api/ip");
        //myhttpwebrequest.protocolversion = httpversion.version11;
        //myhttpwebrequest.servicepoint.expect100continue = false;
        //var response = myhttpwebrequest.getresponse();
        //string todaysdates = response.headers["date"];
        //return datetime.parseexact(todaysdates,
        //                           "ddd, dd mmm yyyy hh:mm:ss 'gmt'",
        //                           cultureinfo.invariantculture.datetimeformat,
        //                           datetimestyles.assumeuniversal);
    }
    public void ResetValuePlayer()
    {
        lastTime = GetNetTime();
        dataPlayer["DateTime"] = GetNetTime().ToString();
        dataPlayer["newday"] = "1";
        dataPlayer["hasClaimTotal"] = "0";
        dataPlayer["hasClaim1"] = "0";
        dataPlayer["hasClaim2"] = "0";
        dataPlayer["hasClaim3"] = "0";
        dataPlayer["hasClaim4"] = "0";
        dataPlayer["numberQuestFinish"] = "0";
        dataPlayer["match"] = "0";
        dataPlayer["win"] = "0";
        UpdateUserData();
        UpdateMultiDataDaily();
    }

    public Dictionary<string, string> dataPlayer = new Dictionary<string, string>();
    public void UpdateUserData()
    {
        PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest()
        {
            Data = dataPlayer
        },
        result =>
        {
            Debug.Log("Update success");
        },
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    public void UpdateUserDataSingle(string key)
    {
        PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest()
        {
            
        },
        result =>
        {
            Debug.Log("Update success");
        },
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    public IEnumerator waitUntilHaveData()
    {
        yield return new WaitUntil(isHaveData);
        ResetDaily();
        InventoryManager.inventory.CheckDayPass();
    }
    bool isHaveData()
    {
        return dataPlayer.Count > 1;
    }
    void ResetDaily()
    {
        if (dataPlayer["newday"] == "1")
        {
            Launcher2.Instance.RandomGoldDaily();
            QuestManager.quest.ResetDaily();
        }
        else
        {
            QuestManager.quest.AddDataAll();
        }
    }
    // isLoaded;
    public void UpdateMultiDataPlayer(int cup, int isOnline, int Money, int levelPlayer, int numberMatch, int numberMatchFinish,
        int numberMatchWin, int numberHideWin, int numberSeekWin, int region, int status, int losing, int dailyPack)
    {
        PlayFabServerAPI.UpdatePlayerStatistics(new PlayFab.ServerModels.UpdatePlayerStatisticsRequest
        {
            PlayFabId = MyPlayfabID,
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<PlayFab.ServerModels.StatisticUpdate>()
        },
        result =>
        {
            if (cup != -99)
            {
                new PlayFab.ServerModels.StatisticUpdate { StatisticName = "cup", Value = cup };
            }
            if (levelPlayer != -99)
            {
                new PlayFab.ServerModels.StatisticUpdate { StatisticName = "levelPlayer", Value = levelPlayer };
            }
            if (isOnline != -99)
            {
                new PlayFab.ServerModels.StatisticUpdate { StatisticName = "isOnline", Value = isOnline };
            }
            if (numberMatch != -99)
            {
                new PlayFab.ServerModels.StatisticUpdate { StatisticName = "numberMatch", Value = numberMatch };
            }
            if (numberMatchFinish != -99)
            {
                new PlayFab.ServerModels.StatisticUpdate { StatisticName = "numberMatchFinish", Value = numberMatchFinish };
            }
            if (numberMatchWin != -99)
            {
                new PlayFab.ServerModels.StatisticUpdate { StatisticName = "numberMatchWin", Value = numberMatchWin };
            }
            if (numberHideWin != -99)
            {
                new PlayFab.ServerModels.StatisticUpdate { StatisticName = "numberHideWin", Value = numberHideWin };
            }
            if (numberSeekWin != -99)
            {
                new PlayFab.ServerModels.StatisticUpdate { StatisticName = "numberSeekWin", Value = numberSeekWin };
            }
            if (region != -99)
            {
                new PlayFab.ServerModels.StatisticUpdate { StatisticName = "region", Value = region };
            }
            if (status != -99)
            {
                new PlayFab.ServerModels.StatisticUpdate { StatisticName = "status", Value = status };
            }
            if(losing != -99)
            {
                new PlayFab.ServerModels.StatisticUpdate { StatisticName = "losing", Value = losing };
            }
            if(dailyPack != -99)
            {
                new PlayFab.ServerModels.StatisticUpdate { StatisticName = "dailyPack", Value = dailyPack };
            }
        },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }
    public void UpdateMultiDataDaily()
    {
        PlayFabServerAPI.UpdatePlayerStatistics(new PlayFab.ServerModels.UpdatePlayerStatisticsRequest
        {
            PlayFabId = MyPlayfabID,
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<PlayFab.ServerModels.StatisticUpdate>()
            {
                new PlayFab.ServerModels.StatisticUpdate { StatisticName = "dailyPack", Value = 3 },
                new PlayFab.ServerModels.StatisticUpdate { StatisticName = "losing", Value = 0 }
            }
        },
        result =>
        {
            losing = 0;
            dailyPack = 3;
            Debug.Log("Update daily");
        },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }
    public void UpdateDataPlayer(int i, string dataName)
    {
        PlayFabServerAPI.UpdatePlayerStatistics(new PlayFab.ServerModels.UpdatePlayerStatisticsRequest
        {
            PlayFabId = MyPlayfabID,
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<PlayFab.ServerModels.StatisticUpdate> {
        new PlayFab.ServerModels.StatisticUpdate { StatisticName = dataName, Value = i},
    }
        },
        result => { Debug.Log("User statistics updated"); },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    IEnumerator checkCheat()
    {
        foreach (System.Diagnostics.Process pro in System.Diagnostics.Process.GetProcesses())
        {
            if (pro.ProcessName.ToLower().Contains("cheat") && pro.ProcessName.ToLower().Contains("engine"))
            {
                pro.Kill();
            }
        }
        yield return new WaitForSeconds(15f);
        StartCoroutine(checkCheat());
    }
    //Get data when player login success
    public void GetDataPlayer(int match, int finished, int win, int hide, int seek)
    {
        numberMatch = match;
        numberMatchWin = win;
        numberMatchFinish = finished;
        numberHideWin = hide;
        numberSeekWin = seek;
    }
    //Update number match for player
    public void ServerGetTitleData()
    {
        PlayFabServerAPI.GetTitleData(new PlayFab.ServerModels.GetTitleDataRequest(),
            result => {
                
                string json = result.Data["ConfigDataGame"];
                ProcessjsonGameDate(json);
                
            },
            error => {
                Debug.Log("Got error getting titleData:");
                Debug.Log(error.GenerateErrorReport());
            });
    }
    [Serializable]
    public class ConfigItem
    {
        public string name;
        public float value;
    }
    [Serializable]
    public class ListItem
    {
        [SerializeField]
        public ConfigItem[] lsConfig;
    }
    private void ProcessjsonGameDate(string jsonString)
    {
        Debug.Log("jsonData:" + jsonString);
        ListItem parsejson = new ListItem();
        parsejson = JsonUtility.FromJson<ListItem>(jsonString);
        for (int i = 0; i < parsejson.lsConfig.Length; i++)
        {
            //Data Game
            switch (parsejson.lsConfig[i].name)
            {
                //speed
                case "speedHide":
                    {
                        ConfigDataGame.speedHide = parsejson.lsConfig[i].value;
                        break;
                    }
                case "speedSeek":
                    {
                        ConfigDataGame.speedSeek = parsejson.lsConfig[i].value;
                        break;
                    }
                case "speedHideBot":
                    {
                        ConfigDataGame.speedHideBot = parsejson.lsConfig[i].value;
                        break;
                    }
                case "speedSeekBot":
                    {
                        ConfigDataGame.speedSeekBot = parsejson.lsConfig[i].value;
                        break;
                    }
                //Zone time
                case "delay1":
                    {
                        ConfigDataGame.delay1 = parsejson.lsConfig[i].value;
                        break;
                    }
                case "delay3":
                    {
                        ConfigDataGame.delay3 = parsejson.lsConfig[i].value;
                        break;
                    }
                case "delay4":
                    {
                        ConfigDataGame.delay4 = parsejson.lsConfig[i].value;
                        break;
                    }
                case "shrink1":
                    {
                        ConfigDataGame.shrink1 = parsejson.lsConfig[i].value;
                        break;
                    }
                case "shrink3":
                    {
                        ConfigDataGame.shrink3 = parsejson.lsConfig[i].value;
                        break;
                    }
                case "shrink4":
                    {
                        ConfigDataGame.shrink4 = parsejson.lsConfig[i].value;
                        break;
                    }
                //Cooldown Skill
                case "dashSpeedMan":
                    {
                        ConfigDataGame.dashSpeedMan = parsejson.lsConfig[i].value;
                        break;
                    }
                case "buffWizard":
                    {
                        ConfigDataGame.buffWizard = parsejson.lsConfig[i].value;
                        break;
                    }
                case "fakeTransformMan":
                    {
                        ConfigDataGame.fakeTransformMan = parsejson.lsConfig[i].value;
                        break;
                    }
                case "summonCloneJoker":
                    {
                        ConfigDataGame.summonCloneJoker = parsejson.lsConfig[i].value;
                        break;
                    }
                case "teleJoker":
                    {
                        ConfigDataGame.teleJoker = parsejson.lsConfig[i].value;
                        break;
                    }
                case "summonRaven":
                    {
                        ConfigDataGame.summonRaven = parsejson.lsConfig[i].value;
                        break;
                    }
                case "hookMummy":
                    {
                        ConfigDataGame.hookMummy = parsejson.lsConfig[i].value;
                        break;
                    }
                //Price
                case "wizardPrice":
                    {
                        ConfigPriceAndReward.wizardPrice = int.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "transformPrice":
                    {
                        ConfigPriceAndReward.transformPrice = int.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "ravenPrice":
                    {
                        ConfigPriceAndReward.ravenPrice = int.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "mummyPrice":
                    {
                        ConfigPriceAndReward.mummyPrice = int.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                //Reward
                case "gold":
                    {
                        ConfigPriceAndReward.gold = int.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "goldBonus":
                    {
                        ConfigPriceAndReward.goldBonus = int.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "exp":
                    {
                        ConfigPriceAndReward.exp = int.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "expBonus":
                    {
                        ConfigPriceAndReward.expBonus = int.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "cupWin":
                    {
                        ConfigPriceAndReward.cupWin = int.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "cupLose":
                    {
                        ConfigPriceAndReward.cupLose = int.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "levelPlayWithFriends":
                    {
                        ConfigRequireLevel.levelPlayWithFriends = int.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "levelChangeName":
                    {
                        ConfigRequireLevel.levelChangeName = int.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "levelWatchAdd":
                    {
                        ConfigRequireLevel.levelWatchAdd = int.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "energyPrice":
                    {
                        ConfigEnergyShop.energyPrice = int.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "energyGift":
                    {
                        ConfigEnergyShop.energyGift = float.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "energyAD":
                    {
                        ConfigEnergyShop.energyAD = float.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
                case "energyCost":
                    {
                        ConfigEnergyShop.energyCost = float.Parse(parsejson.lsConfig[i].value.ToString());
                        break;
                    }
            }
        }
    }
    public void GetSingleData(int data)
    {
        PlayFabClientAPI.GetPlayerProfile(new PlayFab.ClientModels.GetPlayerProfileRequest()
        {
            PlayFabId = MyPlayfabID,
            ProfileConstraints = new PlayFab.ClientModels.PlayerProfileViewConstraints()
            {
                ShowStatistics = true
            }
        },
        result =>
        {
            switch (data)
            {
                case 0:
                    {
                        foreach (var i in result.PlayerProfile.Statistics)
                        {

                            if (i.Name == "cup")
                            {
                                cup = i.Value;
                                break;
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        foreach (var i in result.PlayerProfile.Statistics)
                        {

                            if (i.Name == "Money")
                            {
                                Money = i.Value;
                                break;
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        foreach (var i in result.PlayerProfile.Statistics)
                        {

                            if (i.Name == "levelPlayer")
                            {
                                levelPlayer = i.Value;
                                break;
                            }
                        }
                        break;
                    }
            }
            Debug.Log("Get data");

        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }

}

