using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Firebase.Analytics;

public class MatchManager : MonoBehaviourPunCallbacks
{
    GameManager manager;
    float buttonTime;
    private void Awake()
    {
        manager = GetComponent<GameManager>();
        manager.state = GameManager.GameState.LoadingMap;
        playAnim = false;
    }
    void Start()
    {
        manager.zone.SetActive(false);
        manager.currentMatchTime = GameSetting.MATCH_TIME;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        PhotonNetwork.SendRate = manager.rate;
        PhotonNetwork.SerializationRate = manager.rateS;
        isSpeedUpHide = false;
        StartCoroutine(Check());
        buttonTime = 60;
        timeScanstart = 0;
        //StartCoroutine(WaitForIntro());
    }
    IEnumerator Check()
    {
        yield return new WaitUntil(CheckPlayerSettingDone);
        manager.state = GameManager.GameState.Intro;
        manager.loadingScene.SetActive(false);
        manager.introMap.SetActive(true);
        manager.randomRoleUI.SetActive(true);
        StartCoroutine(WaitForWaiting());
    }
    IEnumerator WaitForWaiting()
    {
        yield return new WaitForSeconds(3f);
        manager.state = GameManager.GameState.Waiting;
        StartCoroutine(WaitForStarting());
    }
    IEnumerator WaitForStarting()
    {
        yield return new WaitForSeconds(4f);
        manager.state = GameManager.GameState.Starting;
        manager.introMap.SetActive(false);
        manager.canavasGame.SetActive(true);
        manager.startGameUI.SetActive(true);
        manager.UI.SetActive(true);
        //SoundManager.Instance.backGroundInGame.clip = SoundManager.Instance.backGroundInGameClip;
        //SoundManager.Instance.backGroundInGame.Play();
        StartCoroutine(WaitForPlaying());
    }
    bool CheckPlayGame()
    {
        return manager.startGameUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
    }
    bool CheckState()
    {
        return manager.state == GameManager.GameState.Waiting;
    }
    IEnumerator CheckReady()
    {
        yield return new WaitUntil(CheckPlayerReady);
        photonView.RPC("StartGame", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void StartGame()
    {
        manager.state = GameManager.GameState.Playing;
    }
    bool CheckPlayerReady()
    {
        if (manager.state != GameManager.GameState.Starting)
        {
            return false;
        }
        int temp1 = 0;
        foreach (PlayerSetup p in manager.players)
        {
            if (p.isReady)
            {
                temp1++;
            }
        }
        if (temp1 == manager.players.Count)
        {
            manager.startGame = true;
        }
        else
        {
            manager.startGame = false;
        }
        return manager.startGame == true;
    }
    bool CheckPlayerSettingDone()
    {
        if (manager.state != GameManager.GameState.InstantiateObject)
        {
            return false;
        }
        int numberPlayerDone = 0;
        foreach (PlayerSetup p in manager.players)
        {
            if (p.isSettingDone)
            {
                numberPlayerDone++;
            }
        }
        if (numberPlayerDone == manager.players.Count)
        {
            manager.startIntro = true;
        }
        else
        {
            manager.startIntro = false;
            foreach (PlayerSetup p in manager.players)
            {
                if (!p.isSettingDone)
                {
                    photonView.RPC("RequestSend", RpcTarget.All);
                }
            }
        }
        return manager.startIntro == true;
    }
    [PunRPC]
    void RequestSend()
    {
        manager.requestSend = true;
    }
    public void InitializeTimer()
    {
        RefreshTimerUI();
        StartCoroutine(Timer());
    }
    private void RefreshTimerUI()
    {
        if(DoorGame.door.escape.GetComponent<Escape>().opened == true)
        {
            manager.uiAlert.text = "Time for escape !";
            string minutes = ((int)manager.currentMatchTime / 60).ToString("0");
            string seconds = ((int)manager.currentMatchTime % 60).ToString("00");
            manager.uiTimer.text = $"{minutes}:{seconds}";
        }
        else if(DoorGame.door.escape.GetComponent<Escape>().opening == true)
        {
            int timeGateOpen = (int)buttonTime;
            string minutes = (timeGateOpen / 60).ToString("0");
            string seconds = (timeGateOpen % 60).ToString("00");
            //manager.timeCountDownImage.fillAmount = (float)(manager.currentMatchTime) / manager._matchLength;
            manager.uiTimer.text = $"{minutes}:{seconds}";
        }
        else
        {
            int timeGateOpen = (int)manager.currentMatchTime - 30;
            string minutes = (timeGateOpen / 60).ToString("0");
            string seconds = (timeGateOpen % 60).ToString("00");
            //manager.timeCountDownImage.fillAmount = (float)(manager.currentMatchTime) / manager._matchLength;
            manager.uiTimer.text = $"{minutes}:{seconds}";
        }
    }
    IEnumerator WaitForPlaying()
    {
        //SoundManager.Instance.PlaySound(SoundManager.AUDIOLIST.three);
        yield return new WaitForSeconds(1f);
        //SoundManager.Instance.PlaySound(SoundManager.AUDIOLIST.two);
        yield return new WaitForSeconds(1f);
        //SoundManager.Instance.PlaySound(SoundManager.AUDIOLIST.one);
        yield return new WaitForSeconds(1f);
        //SoundManager.Instance.PlaySound(SoundManager.AUDIOLIST.go);
        yield return new WaitForSeconds(1f);
        manager.startGameUI.SetActive(false);
        //manager.itemDoorAndButton.SetActive(true);
        manager.zone.SetActive(true);
        manager.state = GameManager.GameState.Playing;
        //manager.timeCage.SetActive(true);
        InitializeTimer();
    }
    float timeScanstart;
    public ScanTimeUI scantimeUI;
    [PunRPC]
    void AddTime()
    {
        manager.currentMatchTime -= 1;
        RefreshTimerUI();
    }
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("AddTime",RpcTarget.AllBuffered);
        }
        
        if (DoorGame.door.escape.GetComponent<Escape>().opening == true && buttonTime > 0)
        {
            buttonTime -= 1;
        }
        if (manager.currentMatchTime <= 0)
        {
            manager.timerCoroutine = null;
            manager.state = GameManager.GameState.Ending;
            manager.seekWin = true;
        }
        else
        {
            manager.timerCoroutine = StartCoroutine(Timer());
        }
    }
    IEnumerator WaitForOffPing()
    {
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            if (p.isBot && p.inRange.Count == 0 && !p.dead)
            {
                p.GetComponent<Player>().caseBot = Player.CaseBot.Scan;
                if (p.player == GameManager.HideOrSeek.Seek)
                {
                    p.GetComponent<Player>().CaseSeekerBot();
                }
                else if (p.player == GameManager.HideOrSeek.Hide)
                {
                    p.GetComponent<Player>().CaseHiderBot();
                }
            }

        }
        timeScanstart = 0;
        GameManager.instance.scanAnim.SetBool("isScaning", true);
        yield return new WaitForSeconds(0.2f);
        GameManager.instance.scanAnim.SetBool("isScaning", false);
        
        if(GameManager.instance.mainPlayer.player == GameManager.HideOrSeek.Seek)
        {
            foreach (PlayerSetup p in GameManager.instance.players)
            {
                if (p.player == GameManager.HideOrSeek.Hide && !p.dead)
                {
                    foreach (Ping ping in GameManager.instance.pings)
                    {
                        if (ping.owner == p)
                        {
                            ping.gameObject.SetActive(true);
                            p.target.enabled = true;
                        }
                    }
                }
            }
        }
        else
        {
            foreach (PlayerSetup p in GameManager.instance.players)
            {
                if (p.player == GameManager.HideOrSeek.Seek)
                {
                    foreach (Ping ping in GameManager.instance.pings)
                    {
                        if (ping.owner == p)
                        {
                            ping.gameObject.SetActive(true);
                            p.target.enabled = true;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(2f);
        foreach (Ping p in GameManager.instance.pings)
        {
            if (p.gameObject.activeSelf)
            {
                p.owner.target.enabled = false;
                p.gameObject.SetActive(false);
            }
        }
    }
    float timeMission = 6;
    bool isSpeedUpHide;
    float timeSeekerAppear = 5f;
    bool playAnim;
    void Update()
    {
        if (manager.state == GameManager.GameState.Playing)
        {
            if (manager.textSeekerAppear.gameObject.activeSelf)
            {
                timeSeekerAppear -= Time.deltaTime;
                int timeTemp = (int)timeSeekerAppear;
                manager.timeSeekerAppear.text = timeTemp + "s";
                if (timeSeekerAppear < 0)
                {
                    manager.textSeekerAppear.gameObject.SetActive(false);
                    manager.timeSeekerAppear.gameObject.SetActive(false);
                }
            }
            if ((manager.playersMatch.Count == manager.numberSeek || manager.currentMatchTime <= 0|| Input.GetKeyDown(KeyCode.P)) && 
                manager.state != GameManager.GameState.Ending &&
                GameManager.instance.players.Count > 1)
            {
                manager.state = GameManager.GameState.Ending;
                if (manager.mainPlayer.player == GameManager.HideOrSeek.Hide)
                {
                    
                    foreach (PlayerSetup p in manager.players)
                    {
                        if (p.player == GameManager.HideOrSeek.Seek)
                        {
                            GameManager.instance.mainPlayer.cam.cinemachine.LookAt = p.transform;
                            GameManager.instance.mainPlayer.cam.cinemachine.Follow = p.transform;
                            GameManager.instance.mainPlayer.GetComponent<Player>().rigidbody.velocity = Vector3.zero;
                            break;
                        }
                    }
                }
                EndSceneManager.manager.StartCoroutine(EndSceneManager.manager.OpenSceneSeekWin());
                playAnim = true;               
                manager.seekWin = true;
            }
            if (manager.playerList.Count == manager.numberSeek + 1 && isSpeedUpHide == false)
            {
                foreach (PlayerSetup p in manager.playersMatch)
                {
                    if (p.player == GameManager.HideOrSeek.Hide)
                    {
                        p.speedDoorOpen = 2f;
                    }
                }
                isSpeedUpHide = true;
            }
            if (manager.isPlayerRevive)
            {
                foreach (PlayerSetup p in manager.players)
                {
                    if (p.player == GameManager.HideOrSeek.Hide)
                    {
                        p.speedDoorOpen = 0f;
                    }
                }
                manager.isPlayerRevive = false;
            }
            if (manager.currentMatchTime > 45 && !DoorGame.door.escape.opened && !DoorGame.door.escape.opening)
            {
                timeScanstart += Time.deltaTime;
                if (scantimeUI.imageFiller.fillAmount <= 1)
                {
                    scantimeUI.imageFiller.fillAmount = (float)timeScanstart / 45;
                }
                else
                {
                    scantimeUI.imageFiller.fillAmount = 0;
                }
                if (timeScanstart >= 5)
                {
                    scantimeUI.textAnim.SetBool("5s", true);
                    //scantimeUI.numbertext.GetComponent<Animator>().SetBool("5s", true);
                    if (timeScanstart >= 45)
                    {
                        StartCoroutine(WaitForOffPing());
                    }
                }
                else
                {
                    scantimeUI.textAnim.SetBool("5s", false);
                    //scantimeUI.numbertext.GetComponent<Animator>().SetBool("5s", false);
                }
            }
            else if (DoorGame.door.escape.opened || DoorGame.door.escape.opening)
            {
                timeScanstart += Time.deltaTime;
                //scantimeUI.numbertext.text = (30 - timeScanstart).ToString();
                if (scantimeUI.imageFiller.fillAmount <= 1)
                {
                    scantimeUI.imageFiller.fillAmount = (float)timeScanstart / 30;
                }
                else
                {
                    scantimeUI.imageFiller.fillAmount = 0;
                }
                if (timeScanstart >= 25)
                {
                    scantimeUI.textAnim.SetBool("5s", true);
                    //scantimeUI.numbertext.GetComponent<Animator>().SetBool("5s", true);
                    if (timeScanstart >= 30)
                    {
                        StartCoroutine(WaitForOffPing());
                    }
                }
                else
                {
                    scantimeUI.textAnim.SetBool("5s", false);
                    //scantimeUI.numbertext.GetComponent<Animator>().SetBool("5s", false);
                }
            }
        }
        if (manager.state == GameManager.GameState.Ending && !manager.end)
        {
            manager.uiEndGame.SetActive(true);
            if (!manager.mainPlayer.photonView.IsMine)
            {
                return;
            }
            if (manager.seekWin)
            {
                FirebaseManager.LogEventType(GameSetting.SEEK_EVENT, "SeekWin");
            }
            else
            {
                FirebaseManager.LogEventType(GameSetting.HIDE_EVENT, "HideWin");
            }
            if (manager.seekWin && manager.mainPlayer.player == GameManager.HideOrSeek.Seek)
            {
                manager.winText.gameObject.SetActive(true);
                //SoundManager.Instance.PlaySound(SoundManager.AUDIOLIST.win);
            }
            else if (manager.seekWin && manager.mainPlayer.player == GameManager.HideOrSeek.Hide)
            {
                manager.loseText.gameObject.SetActive(true);
                //SoundManager.Instance.PlaySound(SoundManager.AUDIOLIST.lose);
            }
            else if (!manager.seekWin && manager.mainPlayer.player == GameManager.HideOrSeek.Hide)
            {
                manager.winText.gameObject.SetActive(true);
                //SoundManager.Instance.PlaySound(SoundManager.AUDIOLIST.win);
            }
            else
            {
                manager.loseText.gameObject.SetActive(true);
                //SoundManager.Instance.PlaySound(SoundManager.AUDIOLIST.lose);
            }
            manager.end = true;
        }
        //if (playAnim == true && GameManager.instance.mainPlayer.cam.cinemachine.m_Lens.OrthographicSize > 2)
        //{
        //    GameManager.instance.mainPlayer.cam.cinemachine.m_Lens.OrthographicSize -= Time.deltaTime;
        //}
    }
    public IEnumerator waitToExplo(PlayerSetup owner, float time)
    {
        GameManager.instance.waypointCanavas.SetActive(false);
        //owner.teleEffect.SetActive(true);
        yield return new WaitForSeconds(time);
        owner.deadArrow.SetActive(false);
        //owner.model.SetActive(false);
        //owner.deadComplete = true;
        //owner.transform.Find("Other").gameObject.SetActive(false);
    }
}