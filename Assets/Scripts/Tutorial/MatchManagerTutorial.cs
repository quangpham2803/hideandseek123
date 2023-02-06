using CoolBattleRoyaleZone;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using I2.Loc;
using UnityEngine.Events;

public class MatchManagerTutorial : MonoBehaviour
{
    GameManagerTutorial manager;
    float buttonTime;
    private void Awake()
    {
        manager = GetComponent<GameManagerTutorial>();
        manager.state = GameManagerTutorial.GameState.LoadingMap;
    }
    void Start()
    {
        manager.currentMatchTime = manager._matchLength;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        foreach (Transform p in manager.grassGroup.transform)
        {
            manager.grass.Add(p.gameObject);
        }
        StartCoroutine(Check());
        buttonTime = 60;
        timeScanstart = 0;
        //StartCoroutine(WaitForIntro());
    }
    IEnumerator Check()
    {
        yield return new WaitForSeconds(0f);
        manager.state = GameManagerTutorial.GameState.Intro;
        StartCoroutine(WaitForWaiting());
    }
    IEnumerator WaitForWaiting()
    {
        yield return new WaitForSeconds(1f);
        manager.timeText.gameObject.SetActive(true);
        manager.state = GameManagerTutorial.GameState.Waiting;
        if (manager.scene == GameManagerTutorial.isScene.Scene1)
        {
            if (LocalizationManager.CurrentLanguage == "English")
            {
                manager.intro2text.text = "Part 1: <color=green>Move</color>";
                manager.intro2text.font = StatusPlayer.Instance.fontList[2];
            }
            else
            {
                manager.intro2text.text = "Phần 1: <color=green>Di chuyển</color>";
                manager.intro2text.font = StatusPlayer.Instance.fontList[1];
            }
            manager.intro2.SetActive(true);
        }
        else if (manager.scene == GameManagerTutorial.isScene.Scene2)
        {            
            if (LocalizationManager.CurrentLanguage == "English")
            {
                manager.intro2text.text = "Part 2: <color=green>Ring</color>";
                manager.intro2text.font = StatusPlayer.Instance.fontList[2];
            }
            else
            {
                manager.intro2text.text = "Phần 2: <color=green>Vòng bo</color>";
                manager.intro2text.font = StatusPlayer.Instance.fontList[1];
            }
            manager.intro2.SetActive(true);
        }
        else if (manager.scene == GameManagerTutorial.isScene.Scene3)
        {            
            if (LocalizationManager.CurrentLanguage == "English")
            {
                manager.intro2text.text = "Part 3: <color=green>Victory</color>";
                manager.intro2text.font = StatusPlayer.Instance.fontList[2];
            }
            else
            {
                manager.intro2text.text = "Phần 3: <color=green>Chiến thắng</color>";
                manager.intro2text.font = StatusPlayer.Instance.fontList[1];
            }
            manager.intro2.SetActive(true);
        }
        if (LocalizationManager.CurrentLanguage == "English")
        {
            manager.touchToContinue.text = "Touch to continue";
            manager.touchToContinue.font = StatusPlayer.Instance.fontList[0];
        }
        else
        {
            manager.touchToContinue.text = "Chạm để tiếp tục";
            manager.touchToContinue.font = StatusPlayer.Instance.fontList[1];
        }
        StartCoroutine(WaitForStarting());
    }
    IEnumerator WaitForStarting()
    {
        yield return new WaitForSeconds(2f);
        manager.state = GameManagerTutorial.GameState.Starting;
        manager.introMap.SetActive(false);
        //SoundManager.Instance.backGroundInGame.clip = SoundManager.Instance.backGroundInGameClip;
       // SoundManager.Instance.backGroundInGame.Play();
        StartCoroutine(WaitForPlaying());
    }


    public void InitializeTimer()
    {
        
        StartCoroutine(Timer());
    }
    private void RefreshTimerUI()
    {

        if (DoorGameTutorial.door.escape.GetComponent<EscapeTutorial>().opened == true)
        {
            manager.uiAlert.text = "Time for escape !";
            string minutes = ((int)manager.currentMatchTime / 60).ToString("0");
            string seconds = ((int)manager.currentMatchTime % 60).ToString("00");
            manager.uiTimer.text = $"{minutes}:{seconds}";
        }
        else if (DoorGameTutorial.door.escape.GetComponent<EscapeTutorial>().opening == true)
        {
            int timeGateOpen = (int)buttonTime;
            string minutes = (timeGateOpen / 60).ToString("0");
            string seconds = (timeGateOpen % 60).ToString("00");
            manager.uiTimer.text = $"{minutes}:{seconds}";
        }
        else
        {
            int timeGateOpen = (int)manager.currentMatchTime - 30;
            string minutes = (timeGateOpen / 60).ToString("0");
            string seconds = (timeGateOpen % 60).ToString("00");
            manager.uiTimer.text = $"{minutes}:{seconds}";
        }
    }
    bool click;
    IEnumerator WaitForPlaying()
    {
        yield return new WaitForSeconds(0f);
        if (manager.scene == GameManagerTutorial.isScene.Scene1)
        {
            manager.tutorialUI.SetActive(true);
            //manager.touchToContinue.gameObject.SetActive(true);
            //manager.backGroundBlack.gameObject.SetActive(true);
            manager.stepAnim.SetTrigger("Active");
            if (LocalizationManager.CurrentLanguage == "English")
            {
                manager.stepText.text = "Welcome to <color=yellow>Tutorial</color>, you will be show how to play this game";
                manager.stepText.font = StatusPlayer.Instance.fontList[2];
            }
            else
            {
                manager.stepText.text = "Chào mừng bạn đến với <color=yellow>Hướng dẫn</color>, bạn sẽ được hướng dẫn về game";
                manager.stepText.font = StatusPlayer.Instance.fontList[1];
            }
            StartCoroutine(Step2Scene1());
        }
        else if (manager.scene == GameManagerTutorial.isScene.Scene2)
        {
            manager.tutorialUI.SetActive(true);
            manager.stepAnim.SetTrigger("Active");
            if (LocalizationManager.CurrentLanguage == "English")
            {
                manager.stepText.text = "Welcome to <color=yellow>Tutorial</color>, you will be show how to play this game";
                manager.stepText.font = StatusPlayer.Instance.fontList[2];
            }
            else
            {// Edit
                manager.stepText.text = "Ở phần hướng dẫn này, bạn sẽ được chỉ dẫn về <color=green>VÒNG BO</color>";
                manager.stepText.font = StatusPlayer.Instance.fontList[1];
            }
            manager.canavasGame.gameObject.SetActive(true);
            manager.state = GameManagerTutorial.GameState.Playing;
            //InitializeTimer();
        }
        else
        {
            manager.tutorialUI.SetActive(true);
            manager.stepAnim.SetTrigger("Active");
            manager.touchToContinue.gameObject.SetActive(false);
            manager.touchToContinue.gameObject.SetActive(true);
            manager.backGroundBlack.gameObject.SetActive(true);
            manager.arrowInfomatch.SetActive(true);
            if (LocalizationManager.CurrentLanguage == "English")
            {
                manager.stepText.text = "This is number player of 2 team. <color=red>Red</color> -> <color=red>SEEKER</color>, <color=green>Green</color> -> <color=green>HIDER</color>";
                manager.stepText.font = StatusPlayer.Instance.fontList[2];
            }
            else
            {
                manager.stepText.text = "Đây là số lượng 2 đội chơi. <color=red>Màu đỏ</color> -> <color=red>Tìm</color>, <color=green>Màu xanh</color> -> <color=green>Trốn</color>.";
                manager.stepText.font = StatusPlayer.Instance.fontList[1];
            }           
            manager.canavasGame.gameObject.SetActive(true);
        }
    }
    IEnumerator Step2Scene1()
    {
        yield return new WaitForSeconds(5f);
        manager.state = GameManagerTutorial.GameState.Playing;
        manager.tutorial = GameManagerTutorial.SceneTutorial1.Step2;
        manager.backGroundBlack.gameObject.SetActive(true);
        manager.stepAnim.SetTrigger("Active");        
        if (LocalizationManager.CurrentLanguage == "English")
        {
            manager.stepText.text = "Use <color=blue>Joystick</color> to move";
            manager.stepText.font = StatusPlayer.Instance.fontList[2];
        }
        else
        {
            manager.stepText.text = "Hãy sử dụng <color=blue>phím tròn</color> để di chuyển";
            manager.stepText.font = StatusPlayer.Instance.fontList[1];
        }
        manager.canavasGame.SetActive(true);
    }
    IEnumerator Intro2()
    {
        yield return new WaitForSeconds(1f);
        manager.state = GameManagerTutorial.GameState.Playing;
        InitializeTimer();
    }
    bool debug;
    public void ClickOnScene()
    {
        if (manager.scene == GameManagerTutorial.isScene.Scene1)
        {
            //if (manager.tutorial == GameManagerTutorial.SceneTutorial1.Step1)
            //{
            //    //manager.touchToContinue.GetComponent<Animator>().SetTrigger("Touch");
            //    manager.tutorial = GameManagerTutorial.SceneTutorial1.Step2;
            //    manager.stepAnim.SetTrigger("Active");
            //    manager.stepText.text = "Hãy sử dụng Joystick để di chuyển";
            //    manager.canavasGame.SetActive(true);
            //    click = true;
            //}
            if (manager.tutorial == GameManagerTutorial.SceneTutorial1.Step3 && !delayclick)
            {
                //manager.touchToContinue.GetComponent<Animator>().SetTrigger("Touch");
                manager.tutorial = GameManagerTutorial.SceneTutorial1.Step4;
                manager.stepAnim.SetTrigger("Active");
                manager.arrowZone.SetActive(false);
                manager.touchToContinue.gameObject.SetActive(false);

                if (LocalizationManager.CurrentLanguage == "English")
                {
                    manager.stepText.text = "Next, let's try this <color=yellow>skill</color>";
                    manager.stepText.font = StatusPlayer.Instance.fontList[2];
                }
                else
                {
                    manager.stepText.text = "Tiếp theo hãy thử sử dụng <color=yellow>tuyệt chiêu</color>skill này xem";
                    manager.stepText.font = StatusPlayer.Instance.fontList[1];
                }
                manager.dashSkill.SetActive(true);
                manager.arrow.SetActive(true);
                delayclick = true;
            }
        }
        else if (manager.scene == GameManagerTutorial.isScene.Scene2)
        {
            if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step2 && !delayclick)
            {
                GameManagerTutorial.instance.tutorial = GameManagerTutorial.SceneTutorial1.Step3;
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    manager.textInfoRingAndDoor.text = "- There are 4 points in all, there is always time to wait and shrink for each Ring \n- Inside the white circle is <color=green> Safe zone </color> \n- Outside the red ring is <color=red > Danger zone </color>";
                    manager.textInfoRingAndDoor.font = StatusPlayer.Instance.fontList[2];
                }
                else
                {
                    manager.textInfoRingAndDoor.text = "- Có 4 bo tất cả, luôn có thời gian đợi và co lại cho mỗi Bo\n- Bên trong vòng trắng là <color=green>Vùng an toàn</color>\n- Ngoài vòng Ring đỏ là <color=red>Vùng nguy hiểm</color>";
                    manager.textInfoRingAndDoor.font = StatusPlayer.Instance.fontList[1];
                }
                manager.infoRing.SetActive(true);
                manager.joystick.gameObject.SetActive(false);
                manager.mainPlayer.vertical = 0f;
                manager.mainPlayer.horizontal = 0f;
                manager.joystick.OnPointerUp(null);
                click = true;
            }
            if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step4 && !delayclick)
            {
                GameManagerTutorial.instance.tutorial = GameManagerTutorial.SceneTutorial1.Step5;
                manager.joystick.gameObject.SetActive(true);
                click = true;
            }
        }
        else
        {
            if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step1 && !delayclick)
            {
                GameManagerTutorial.instance.tutorial = GameManagerTutorial.SceneTutorial1.Step2;
                manager.stepAnim.SetTrigger("Active");
                manager.arrowScan.SetActive(true);
                manager.arrowInfomatch.SetActive(false);
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    manager.stepText.text = "This is <color=orange>scan-bar</color>. When it full, you will be know other team's position.";
                    manager.stepText.font = StatusPlayer.Instance.fontList[2];
                }
                else
                {
                    manager.stepText.text = "Đây là <color=orange>thanh quét</color>. Khi thanh đầy, bạn sẽ biết được vị trí đội kia";
                    manager.stepText.font = StatusPlayer.Instance.fontList[1];
                }
                manager.touchToContinue.gameObject.SetActive(false);
                manager.touchToContinue.gameObject.SetActive(true);
                delayclick = true;
            }
            else if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step2 && !delayclick)
            {
                GameManagerTutorial.instance.tutorial = GameManagerTutorial.SceneTutorial1.Step3;
                manager.stepAnim.SetTrigger("Active");
                manager.arrowScan.SetActive(false);
                
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    manager.stepText.text = "We have to wait 5s to know who is <color=red>SEEKER</color>";
                    manager.stepText.font = StatusPlayer.Instance.fontList[2];
                }
                else
                {
                    manager.stepText.text = "Chúng ta sẽ phải đợi 5s để biết xem ai là <color=red>Tìm</color>";
                    manager.stepText.font = StatusPlayer.Instance.fontList[1];
                }
                manager.state = GameManagerTutorial.GameState.Playing;
                //manager.zone.SetActive(true);
                //InitializeTimer();
                click = true;
            }
        }
        if (click)
        {
            manager.touchToContinue.gameObject.SetActive(false);
            manager.backGroundBlack.gameObject.SetActive(false);
            click = false;
        }
    }
    bool delayclick = true;
    Transform playerNeed;
    public void CheckPlayerNeedNearest()
    {
        playerNeed = manager.botList[0].transform;
        float minRange = Vector3.Distance(playerNeed.position, transform.position);
        foreach (PlayerSetupTutorial p in manager.botList)
        {
            if (Vector3.Distance(transform.position, p.transform.position) > minRange)
            {
                minRange = Vector3.Distance(transform.position, p.transform.position);
                playerNeed = p.transform;
            }
        }
    }
    bool timeSeeker;
    bool seekerAppear;
    float timeScanstart;
    public ScanTimeUI scantimeUI;
    
    void AddTime()
    {

        timeScanstart += 1;
        RefreshTimerUI();
    }
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    photonView.RPC("AddTime", RpcTarget.AllBuffered);
        //}
        manager.currentMatchTime -= 1;
        RefreshTimerUI();
        if (DoorGameTutorial.door.escape.GetComponent<EscapeTutorial>().opening == true && buttonTime > 0)
        {
            buttonTime -= 1;
        }
        if (manager.currentMatchTime <= 0)
        {
            manager.timerCoroutine = null;
            manager.state = GameManagerTutorial.GameState.Ending;
            manager.seekWin = true;
        }
        else
        {
            manager.timerCoroutine = StartCoroutine(Timer());
        }
    }
    float timeJoystick;
    bool temp;
    float timeSeekerAppear = 5f;
    void Update()
    {
        if (manager.touchToContinue.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Touch") && manager.touchToContinue.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.9f)
        {
            if (delayclick)
            {
                delayclick = false;
            }
        }
        if (manager.state != GameManagerTutorial.GameState.Playing)
        {
            return;
        }
        if (manager.scene == GameManagerTutorial.isScene.Scene1)
        {
            if (manager.tutorial == GameManagerTutorial.SceneTutorial1.Step2 && (manager.mainPlayer.joystick.Horizontal != 0 || manager.mainPlayer.joystick.Vertical != 0))
            {
                timeJoystick += Time.deltaTime;
                manager.backGroundBlack.gameObject.SetActive(false);
                manager.joyStickimage.SetActive(false);
                if (timeJoystick > 3f)
                {
                    timeJoystick = 0f;
                    manager.joyStickimage.SetActive(false);
                    manager.tutorial = GameManagerTutorial.SceneTutorial1.Step3;
                    manager.backGroundBlack.gameObject.SetActive(true);
                    manager.mainPlayer.vertical = 0f;
                    manager.mainPlayer.horizontal = 0f;
                    manager.joystick.OnPointerUp(null);
                    manager.joystick.gameObject.SetActive(false);
                    manager.stepAnim.SetTrigger("Active");
                    manager.arrowZone.SetActive(true);
                    manager.touchToContinue.transform.localPosition = new Vector3(0, -200, 0);

                    if (LocalizationManager.CurrentLanguage == "English")
                    {
                        manager.stepText.text = "This is your role -> <color=green>Hider</color>. <color=red>SEEKER</color> will be <color=red>red</color>";
                        manager.stepText.font = StatusPlayer.Instance.fontList[2];
                    }
                    else
                    {
                        manager.stepText.text = "Đây là vai trò của bạn -> <color=green>Người trốn</color>. Bên cạnh đó <color=red>Người tìm</color>Người tìm sẽ có màu đỏ";
                        manager.stepText.font = StatusPlayer.Instance.fontList[1];
                    }
                    manager.touchToContinue.gameObject.SetActive(false);
                    manager.touchToContinue.gameObject.SetActive(true);
                }
            }
            else if (manager.tutorial == GameManagerTutorial.SceneTutorial1.Step2 && (manager.mainPlayer.joystick.Horizontal == 0 && manager.mainPlayer.joystick.Vertical == 0))
            {
                manager.joyStickimage.SetActive(true);
            }
            if (manager.tutorial == GameManagerTutorial.SceneTutorial1.Step5 && !temp)
            {
                manager.stepAnim.SetTrigger("Active");
                
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    manager.stepText.text = "Let's use <color=yellow>skill</color>.";
                    manager.stepText.font = StatusPlayer.Instance.fontList[2];
                }
                else
                {
                    manager.stepText.text = "Hãy sử dụng <color=yellow>tuyệt chiêu</color> vài lần nữa xem";
                    manager.stepText.font = StatusPlayer.Instance.fontList[1];
                }
                manager.joystick.gameObject.SetActive(false);
                manager.backGroundBlack.gameObject.SetActive(false);
                manager.joystick.gameObject.SetActive(true);
                manager.arrow.SetActive(false);
                temp = true;
                StartCoroutine(IETemp());
            }
            if (manager.tutorial == GameManagerTutorial.SceneTutorial1.Step6 && temp)
            {
                manager.stepAnim.SetTrigger("Active");
                manager.joystick.gameObject.SetActive(true);
                
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    manager.stepText.text = "- <color=orange> Gate </color> is open!Let's run away.";
                    manager.stepText.font = StatusPlayer.Instance.fontList[2];
                }
                else
                {
                    manager.stepText.text = "- <color=orange>Cổng</color> đã mở! Hãy chạy thoát.";
                    manager.stepText.font = StatusPlayer.Instance.fontList[1];
                }
                temp = false;
            }
            if (manager.tutorial == GameManagerTutorial.SceneTutorial1.Step6 || manager.tutorial == GameManagerTutorial.SceneTutorial1.Step7)
            {
                manager.lineRenderer.gameObject.SetActive(true);
                manager.lineRenderer.SetPosition(0, manager.mainPlayer.transform.position);
                manager.lineRenderer.SetPosition(1, DoorGameTutorial.door.escape.transform.position);
            }
        }
        else if (manager.scene == GameManagerTutorial.isScene.Scene2)
        {
            if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step2 && !temp)
            {
                temp = true;
                manager.stepAnim.SetTrigger("Active");
                
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    manager.stepText.text = "This is the current circle. <color=green> Green </color> is pending. <color=red> Red </color> is live";
                    manager.stepText.font = StatusPlayer.Instance.fontList[2];
                }
                else
                {//Edit
                    manager.stepText.text = "Đây là vòng bo hiện tại. <color=green>Màu xanh</color> là đang chờ. <color=red>Màu đỏ</color> là đang co";
                    manager.stepText.font = StatusPlayer.Instance.fontList[1];
                }
                manager.hideRoleUI.SetActive(false);
                manager.arrowZone.SetActive(true);
                manager.canavasGame.SetActive(true);
                manager.touchToContinue.gameObject.SetActive(false);
                manager.touchToContinue.gameObject.SetActive(true);
                manager.backGroundBlack.gameObject.SetActive(true);
                manager.joystick.gameObject.SetActive(false);
                manager.mainPlayer.vertical = 0f;
                manager.mainPlayer.horizontal = 0f;
                manager.joystick.OnPointerUp(null);
            }
            if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step2)
            {
                if (manager.touchToContinue.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Touch") && manager.touchToContinue.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.9f)
                {
                    Time.timeScale = 0;
                }
            }
            if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step4 && temp)
            {
                temp = false;
                manager.hideRoleUI.SetActive(true);
                manager.arrowZone.SetActive(false);
                manager.stepAnim.SetTrigger("Active");
                GameManagerTutorial.instance.waypointCanavas.SetActive(true);
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    manager.stepText.text = "Oops, Your teammate have been defeated, let's help him.";
                    manager.stepText.font = StatusPlayer.Instance.fontList[2];
                }
                else
                {//Edit
                    manager.stepText.text = "Oops, có vẻ đồng đội đang cần sự giúp đỡ";
                    manager.stepText.font = StatusPlayer.Instance.fontList[1];
                }
                manager.joystick.gameObject.SetActive(true);
                manager.botList[0].gameObject.SetActive(true);
                manager.touchToContinue.gameObject.SetActive(false);
                manager.touchToContinue.gameObject.SetActive(true);
                manager.backGroundBlack.gameObject.SetActive(true);
                manager.joystick.gameObject.SetActive(false);
                manager.mainPlayer.vertical = 0f;
                manager.mainPlayer.horizontal = 0f;
                manager.joystick.OnPointerUp(null);
                //StartCoroutine(CamTarget(manager.botList[0].gameObject));
            }
            if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step5)
            {
                manager.lineRenderer.gameObject.SetActive(true);
                manager.lineRenderer.SetPosition(0, manager.mainPlayer.transform.position);
                manager.lineRenderer.SetPosition(1, manager.botList[0].transform.position);
                if (Vector3.Distance(manager.mainPlayer.transform.position, manager.botList[0].transform.position) < 8f)
                {
                    if (LocalizationManager.CurrentLanguage == "English")
                    {
                        if (manager.stepText.text != "Let's move next to your teammate to <color=green>revive</color> him (Auto)")
                        {
                            manager.stepAnim.SetTrigger("Active");
                            manager.stepText.text = "Let's move next to your teammate to <color=green>revive</color> him (Auto)";
                        }
                        manager.stepText.font = StatusPlayer.Instance.fontList[2];
                    }
                    else
                    {
                        if (manager.stepText.text != "Hãy tiến lại gần để <color=green>hồi sinh</color> đồng đội mình (Tự động)")
                        {
                            manager.stepAnim.SetTrigger("Active");
                            manager.stepText.text = "Hãy tiến lại gần để <color=green>hồi sinh</color> đồng đội mình (Tự động)";
                        }
                        manager.stepText.font = StatusPlayer.Instance.fontList[1];
                    }

                }
            }
            if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step6 && !temp)
            {
                temp = true;
                manager.lineRenderer.gameObject.SetActive(false);
                //manager.stepAnim.SetTrigger("Active");
                //manager.stepText.text = "Rất tốt, bạn đã hồi sinh được đồng đội";
                manager.botList[0].gameObject.SetActive(false);
                GameManagerTutorial.instance.tutorial = GameManagerTutorial.SceneTutorial1.Step7;
                manager.infoSave.SetActive(true);
                
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    manager.textInfoSaveAndSeekNear.text = "- Move to your teammate to <color=green>revive</color> him (Auto)\n - Beside that, you will know where your teammates need help.";
                    manager.textInfoSaveAndSeekNear.font = StatusPlayer.Instance.fontList[2];
                }
                else
                {//edit
                    manager.textInfoSaveAndSeekNear.text = "Khi tiến lại đủ gần, bạn có thể <color=green>HỒI SINH</color> đồng đội mình\n - Bạn cũng có thể thấy được vị trí của đồng đội khác cần đang cần hồi sinh";
                    manager.textInfoSaveAndSeekNear.font = StatusPlayer.Instance.fontList[1];
                }
                manager.joystick.gameObject.SetActive(false);
                manager.mainPlayer.vertical = 0f;
                manager.mainPlayer.horizontal = 0f;
                manager.joystick.OnPointerUp(null);
            }
            if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step8)
            {
                manager.lineRenderer.gameObject.SetActive(true);
                manager.lineRenderer.SetPosition(0, manager.mainPlayer.transform.position);
                manager.lineRenderer.SetPosition(1, DoorGameTutorial.door.escape.transform.position);
            }
        }
        else
        {
            if (GameManagerTutorial.instance.numberHideCurrent == 0)
            {
                GameManagerTutorial.instance.state = GameManagerTutorial.GameState.Ending;
                GameManagerTutorial.instance.popupMenu.SetActive(true);
                GameManagerTutorial.instance.backgroundBlack2.SetActive(true);
            }
            if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step4 && !temp)
            {
                temp = true;
                manager.infoSeekerAppear.SetActive(true);
                
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    manager.textinfoSeekerAppear.text = "After 5s, The <color=red>SEEKER</color> will appear.";
                    manager.textinfoSeekerAppear.font = StatusPlayer.Instance.fontList[2];
                }
                else
                {
                    manager.textinfoSeekerAppear.text = "Khi 5s đếm ngược kết thúc, người trở thành <color=red>Tìm</color> sẽ xuất hiện.";
                    manager.textinfoSeekerAppear.font = StatusPlayer.Instance.fontList[1];
                }
                manager.joystick.gameObject.SetActive(false);
                manager.mainPlayer.vertical = 0f;
                manager.mainPlayer.horizontal = 0f;
                manager.joystick.OnPointerUp(null);
                GameManagerTutorial.instance.mainPlayer.GetComponent<PlayerTutorial>().rigidbody.velocity = Vector3.zero;
                manager.state = GameManagerTutorial.GameState.Starting;
                GameManagerTutorial.instance.tutorial = GameManagerTutorial.SceneTutorial1.Step5;
            }
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
            if (manager.currentMatchTime > 30 && !DoorGameTutorial.door.escape.opened && !DoorGameTutorial.door.escape.opening)
            {
                timeScanstart += Time.deltaTime;
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
            else if (DoorGameTutorial.door.escape.opened || DoorGameTutorial.door.escape.opening)
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
        LogEvent();
    }
    void LogEvent()
    {
        if (manager.tutorial == GameManagerTutorial.SceneTutorial1.Step1 && !debug)
        {
            debug = true;
            FirebaseManager.LogEventTutorial(manager.scene.ToString(), manager.tutorial.ToString());
        }
        else if (manager.tutorial == GameManagerTutorial.SceneTutorial1.Step2 && debug)
        {
            debug = false;
            FirebaseManager.LogEventTutorial(manager.scene.ToString(), manager.tutorial.ToString());
        }
        else if (manager.tutorial == GameManagerTutorial.SceneTutorial1.Step3 && !debug)
        {
            debug = true;
            FirebaseManager.LogEventTutorial(manager.scene.ToString(), manager.tutorial.ToString());
        }
        else if (manager.tutorial == GameManagerTutorial.SceneTutorial1.Step4 && debug)
        {
            debug = false;
            FirebaseManager.LogEventTutorial(manager.scene.ToString(), manager.tutorial.ToString());
        }
        else if (manager.tutorial == GameManagerTutorial.SceneTutorial1.Step5 && !debug)
        {
            debug = true;
            FirebaseManager.LogEventTutorial(manager.scene.ToString(), manager.tutorial.ToString());
        }
        else if (manager.tutorial == GameManagerTutorial.SceneTutorial1.Step6 && debug)
        {
            debug = false;
            FirebaseManager.LogEventTutorial(manager.scene.ToString(), manager.tutorial.ToString());
        }
        else if (manager.tutorial == GameManagerTutorial.SceneTutorial1.Step7 && !debug)
        {
            debug = true;
            FirebaseManager.LogEventTutorial(manager.scene.ToString(), manager.tutorial.ToString());
        }
    }
    IEnumerator IETemp()
    {
        yield return new WaitForSeconds(10f);
        manager.tutorial = GameManagerTutorial.SceneTutorial1.Step6;
        DoorGameTutorial.door.StartCoroutine(DoorGameTutorial.door.OpenDoorScene());
    }

    public IEnumerator CamTarget(GameObject pobject)
    {
        //GameManagerTutorial.instance.waypointCanavas.SetActive(false);
        GameManagerTutorial.instance.mainPlayer.cam.cinemachine.LookAt = pobject.transform;
        GameManagerTutorial.instance.mainPlayer.cam.cinemachine.Follow = pobject.transform;
        foreach (PlayerSetupTutorial p in GameManagerTutorial.instance.players)
        {
            p.speedStun = 0;
        }
        yield return new WaitForSeconds(3f);
        GameManagerTutorial.instance.mainPlayer.cam.cinemachine.LookAt = GameManagerTutorial.instance.mainPlayer.cam.owner.transform;
        GameManagerTutorial.instance.mainPlayer.cam.cinemachine.Follow = GameManagerTutorial.instance.mainPlayer.cam.owner.transform;
        foreach (PlayerSetupTutorial p in GameManagerTutorial.instance.players)
        {
            p.speedStun = 1;
        }
    }
    IEnumerator WaitForOffPing()
    {
        timeScanstart = 0;
        GameManagerTutorial.instance.scanAnim.SetBool("isScaning", true);
        yield return new WaitForSeconds(0.2f);
        GameManagerTutorial.instance.scanAnim.SetBool("isScaning", false);

        if (GameManagerTutorial.instance.mainPlayer.player == GameManagerTutorial.HideOrSeek.Seek)
        {
            foreach (PlayerSetupTutorial p in GameManagerTutorial.instance.players)
            {
                if (p.player == GameManagerTutorial.HideOrSeek.Hide && !p.dead)
                {
                    foreach (PingTutorial ping in GameManagerTutorial.instance.pings)
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
            foreach (PlayerSetupTutorial p in GameManagerTutorial.instance.players)
            {
                if (p.player == GameManagerTutorial.HideOrSeek.Seek)
                {
                    foreach (PingTutorial ping in GameManagerTutorial.instance.pings)
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
        foreach (PingTutorial p in GameManagerTutorial.instance.pings)
        {
            if (p.gameObject.activeSelf)
            {
                p.owner.target.enabled = false;
                p.gameObject.SetActive(false);
            }
        }
    }
    public void SaveNextSeekNear()
    {
        manager.dot3.color = Color.white;
        manager.dot4.color = Color.yellow;
        manager.nextSaveAndSeekNear.gameObject.SetActive(false);
        manager.nextSeekNearAndSave.gameObject.SetActive(true);
        manager.imageSaveSeekNear.SetBool("Button", false);
        manager.imageSaveSeekNear.SetBool("Door", true);
        manager.imageInfoSave.gameObject.SetActive(true);
        manager.imageInfoSeekNear.gameObject.SetActive(false);
        
        if (LocalizationManager.CurrentLanguage == "English")
        {
            manager.textInfoSaveAndSeekNear.text = "- This is the time to wait for <color=green> to revive </color>\n - If this time expires, you will die, and can no longer be able to <color=green> revive </color> ";
            manager.textInfoSaveAndSeekNear.font = StatusPlayer.Instance.fontList[2];
        }
        else
        {//edit
            manager.textInfoSaveAndSeekNear.text = "- Đây là thời gian chờ <color=green>hồi sinh</color>\n- Nếu hết thời gian này, bạn sẽ chết, không thể được <color=green>hồi sinh</color> nữa";
            manager.textInfoSaveAndSeekNear.font = StatusPlayer.Instance.fontList[1];
        }
        manager.buttonXacNhan2.SetActive(true);
    }
    public void ButtonNextOutRing()
    {
        manager.dot1.color = Color.white;
        manager.dot2.color = Color.yellow;
        manager.dot2_2.color = Color.white;
        manager.nextRingToOutRing.gameObject.SetActive(false);
        manager.nextOutRingToDoor.gameObject.SetActive(true);
        manager.nextOutRingToRing.gameObject.SetActive(true);
        manager.nextDoorToOutRing.gameObject.SetActive(false);
        manager.imageButtonDoor.SetBool("Button", false);
        manager.imageButtonDoor.SetBool("Door", true);
        manager.imageButtonDoor.SetBool("Ring", false);
        manager.imageButtonDoor.SetBool("OutRing", false);
        manager.imageInfoOutRing.gameObject.SetActive(true);
        manager.imageInfoRing.gameObject.SetActive(false);
        manager.imageInfoDoor.gameObject.SetActive(false);
        
        if (LocalizationManager.CurrentLanguage == "English")
        {
            manager.textInfoRingAndDoor.text = "- When you leave<color=red> Danger Zone </color>, there will be an alert \n - When you leave the Zone, you will be detected by the enemy team \n - If you do not return <color=green> Safe zone</color> in time, you will die";
            manager.textInfoRingAndDoor.font = StatusPlayer.Instance.fontList[2];
        }
        else
        {
            manager.textInfoRingAndDoor.text = "- Khi ra <color=red>Vùng nguy hiểm</color>, sẽ có cảnh báo\n- Khi ra Vùng đó, bạn sẽ bị team địch phát hiện\n- Nếu không quay trở lại <color=green>Vùng an toàn</color> kịp lúc, bạn sẽ chết";
            manager.textInfoRingAndDoor.font = StatusPlayer.Instance.fontList[1];
        }
        manager.buttonXacNhan.SetActive(false);
    }

    private void OnBuySuccesful(string id)
    {
        // swich id 100 gold 
        // add gold
    }    
    public void OutRingToDoor()
    {

        manager.dot1.color = Color.white;
        manager.dot2.color = Color.white;
        manager.dot2_2.color = Color.yellow;
        manager.nextRingToOutRing.gameObject.SetActive(false);
        manager.nextOutRingToDoor.gameObject.SetActive(false);
        manager.nextOutRingToRing.gameObject.SetActive(false);
        manager.nextDoorToOutRing.gameObject.SetActive(true);
        manager.imageButtonDoor.SetBool("Button", false);
        manager.imageButtonDoor.SetBool("Door", false);
        manager.imageButtonDoor.SetBool("Ring", true);
        manager.imageButtonDoor.SetBool("OutRing", false);
        manager.imageInfoOutRing.gameObject.SetActive(false);
        manager.imageInfoRing.gameObject.SetActive(false);
        manager.imageInfoDoor.gameObject.SetActive(true);
       
        if (LocalizationManager.CurrentLanguage == "English")
        {
            manager.textInfoRingAndDoor.text = "- <color=orange> The gate </color> will open once all four have been passed.The <color=green> Hider </color> team only needs one person to escape to win.";
            manager.textInfoRingAndDoor.font = StatusPlayer.Instance.fontList[2];
        }
        else
        {//edit
            manager.textInfoRingAndDoor.text = "- <color=orange>Cổng</color> sẽ mở khi đã trải qua đủ 4 Bo\n- Đội <color=green>Trốn</color> chỉ cần một người trốn thoát được là sẽ thắng.";
            manager.textInfoRingAndDoor.font = StatusPlayer.Instance.fontList[1];
        }
        manager.buttonXacNhan.SetActive(true);
    }
    public void OutRingToRing()
    {
        manager.dot1.color = Color.yellow;
        manager.dot2.color = Color.white;
        manager.dot2_2.color = Color.white;
        manager.nextRingToOutRing.gameObject.SetActive(true);
        manager.nextOutRingToDoor.gameObject.SetActive(false);
        manager.nextOutRingToRing.gameObject.SetActive(false);
        manager.nextDoorToOutRing.gameObject.SetActive(false);
        manager.imageButtonDoor.SetBool("Button", false);
        manager.imageButtonDoor.SetBool("Door", false);
        manager.imageButtonDoor.SetBool("Ring", false);
        manager.imageButtonDoor.SetBool("OutRing", true);
        manager.imageInfoOutRing.gameObject.SetActive(false);
        manager.imageInfoRing.gameObject.SetActive(true);
        manager.imageInfoDoor.gameObject.SetActive(false);
        if (LocalizationManager.CurrentLanguage == "English")
        {
            manager.textInfoRingAndDoor.text = "- There are 4 circles in all, there is always time to wait and shrink for each Ring \n- Inside the white circle is <color=green> Safe zone </color> \n- Outside the red ring is <color=red > Danger zone </color>";
            manager.textInfoRingAndDoor.font = StatusPlayer.Instance.fontList[2];
        }
        else
        {// edit
            manager.textInfoRingAndDoor.text = "- Có 4 bo tất cả, luôn có thời gian đợi và co lại cho mỗi Bo\n- Bên trong vòng trắng là <color=green>Vùng an toàn</color>\n- Ngoài vòng Ring đỏ là <color=red>Vùng nguy hiểm</color>";
            manager.textInfoRingAndDoor.font = StatusPlayer.Instance.fontList[1];
        }
        manager.buttonXacNhan.SetActive(false);
    }
    public void DoorNextOutRing()
    {
        manager.dot1.color = Color.white;
        manager.dot2.color = Color.yellow;
        manager.dot2_2.color = Color.white;
        manager.nextRingToOutRing.gameObject.SetActive(false);
        manager.nextOutRingToDoor.gameObject.SetActive(true);
        manager.nextOutRingToRing.gameObject.SetActive(true);
        manager.nextDoorToOutRing.gameObject.SetActive(false);
        manager.imageButtonDoor.SetBool("Button", true);
        manager.imageButtonDoor.SetBool("Door", false);
        manager.imageButtonDoor.SetBool("Ring", false);
        manager.imageButtonDoor.SetBool("OutRing", false);
        manager.imageInfoOutRing.gameObject.SetActive(true);
        manager.imageInfoRing.gameObject.SetActive(false);
        manager.imageInfoDoor.gameObject.SetActive(false);
        
        if (LocalizationManager.CurrentLanguage == "English")
        {
            manager.textInfoRingAndDoor.text = "- When you leave <color=red> Danger Zone </color>, there will be an alert \n- When you leave the Zone, you will be detected by the enemy team \n- If you do not return <color=green> Safe zone </color> in time, you will die.";
            manager.textInfoRingAndDoor.font = StatusPlayer.Instance.fontList[2];
        }
        else
        {
            manager.textInfoRingAndDoor.text = "- Khi ra <color=red>Vùng nguy hiểm</color>, sẽ có cảnh báo\n- Khi ra Vùng đó, bạn sẽ bị team địch phát hiện\n- Nếu không quay trở lại <color=green>Vùng an toàn</color> kịp lúc, bạn sẽ chết";
            manager.textInfoRingAndDoor.font = StatusPlayer.Instance.fontList[1];
        }
        manager.buttonXacNhan.SetActive(false);
    }
    public void SeeknearNextSave()
    {
        manager.dot3.color = Color.yellow;
        manager.dot4.color = Color.white;
        manager.nextSaveAndSeekNear.gameObject.SetActive(true);
        manager.nextSeekNearAndSave.gameObject.SetActive(false);
        manager.imageSaveSeekNear.SetBool("Button", true);
        manager.imageSaveSeekNear.SetBool("Door", false);
        manager.imageInfoSeekNear.gameObject.SetActive(false);
        manager.imageInfoSave.gameObject.SetActive(true);
        
        if (LocalizationManager.CurrentLanguage == "English")
        {
            manager.textInfoSaveAndSeekNear.text = "- Get closer to <color=green> revive </color> your teammates(Automatic) \n - Besides, you can also find out other hider's locations that need help.";
            manager.textInfoSaveAndSeekNear.font = StatusPlayer.Instance.fontList[2];
        }
        else
        {
            manager.textInfoSaveAndSeekNear.text = "- Hãy tiến lại gần để <color=green>hồi sinh</color> đồng đội mình (Tự động)\n - Bên cạnh đó, bạn cũng có thể biết được vị trí trốn khác cần sự giúp đỡ";
            manager.textInfoSaveAndSeekNear.font = StatusPlayer.Instance.fontList[1];
        }
        manager.buttonXacNhan2.SetActive(false);
    }
    public void MissonNext5s()
    {
        manager.dot5.color = Color.yellow;
        manager.dot6.color = Color.white;
        manager.next5sandMisson.gameObject.SetActive(false);
        manager.nextMissonand5s.gameObject.SetActive(true);
        manager.image5sMisson.SetBool("Button", true);
        manager.image5sMisson.SetBool("Door", false);
        manager.imageInfoMisson.gameObject.SetActive(false);
        manager.imageInfo5s.gameObject.SetActive(true);

        if (LocalizationManager.CurrentLanguage == "English")
        {
            manager.textinfoSeekerAppear.text = "- After 5s, <color=red>SEEKER</color> will appear.";
            manager.textinfoSeekerAppear.font = StatusPlayer.Instance.fontList[2];
        }
        else
        {
            manager.textinfoSeekerAppear.text = "- Khi 5s đếm ngược kết thúc, người trở thành <color=red>Tìm</color> sẽ xuất hiện.";
            manager.textinfoSeekerAppear.font = StatusPlayer.Instance.fontList[1];
        }
        manager.buttonXacNhan3.SetActive(false);
    }
    public void FivesNextMisson()
    {
        manager.dot5.color = Color.white;
        manager.dot6.color = Color.yellow;
        manager.next5sandMisson.gameObject.SetActive(true);
        manager.nextMissonand5s.gameObject.SetActive(false);
        manager.image5sMisson.SetBool("Button", false);
        manager.image5sMisson.SetBool("Door", true);
        manager.imageInfoMisson.gameObject.SetActive(true);
        manager.imageInfo5s.gameObject.SetActive(false);
        
        if (LocalizationManager.CurrentLanguage == "English")
        {
            manager.textinfoSeekerAppear.text = "- The task of<color=red> SEEKER </color> now is to find and prevent Hider";
            manager.textinfoSeekerAppear.font = StatusPlayer.Instance.fontList[2];
        }
        else
        {
            manager.textinfoSeekerAppear.text = "- Nhiệm vụ của <color=red>Tìm</color> lúc này là truy tìm và ngăn cản trốn";
            manager.textinfoSeekerAppear.font = StatusPlayer.Instance.fontList[1];
        }
        manager.buttonXacNhan3.SetActive(true);
    }
    public void XacNhan()
    {
        Time.timeScale = 1;
        manager.infoRing.SetActive(false);
        manager.tutorial = GameManagerTutorial.SceneTutorial1.Step4;
    }
    public void XacNhan2()
    {
        Time.timeScale = 1;
        manager.infoSave.SetActive(false);
        manager.tutorial = GameManagerTutorial.SceneTutorial1.Step8;
        DoorGameTutorial.door.StartCoroutine(DoorGameTutorial.door.OpenDoorScene());
        manager.stepAnim.SetTrigger("Active");
        
        if (LocalizationManager.CurrentLanguage == "English")
        {
            manager.stepText.text = "Please run to <color=orange> Gate </color> ";
            manager.stepText.font = StatusPlayer.Instance.fontList[2];
        }
        else
        {
            manager.stepText.text = "Hãy chạy đến <color=orange>cổng</color>";
            manager.stepText.font = StatusPlayer.Instance.fontList[1];
        }
        manager.joystick.gameObject.SetActive(true);
    }
    public void XacNhan3()
    {
        manager.infoSeekerAppear.SetActive(false);
        manager.state = GameManagerTutorial.GameState.Playing;
        GameManagerTutorial.instance.tutorial = GameManagerTutorial.SceneTutorial1.Step6;
        manager.stepAnim.SetTrigger("Active");
        
        if (LocalizationManager.CurrentLanguage == "English")
        {
            manager.stepText.text = "Use <color=orange> Gate </color> to win";
            manager.stepText.font = StatusPlayer.Instance.fontList[2];
        }
        else
        {
            manager.stepText.text = "Hãy ra <color=orange>cổng</color> để chiến thắng";
            manager.stepText.font = StatusPlayer.Instance.fontList[1];
        }
        manager.joystick.gameObject.SetActive(true);
        manager.skip.SetActive(true);
        manager.zone.SetActive(true);
        InitializeTimer();
    }
}
