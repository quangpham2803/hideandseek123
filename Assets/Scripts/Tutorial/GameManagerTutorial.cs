
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManagerTutorial : MonoBehaviour
{
    public enum HideOrSeek
    {
        Hide,
        Seek
    };
    public enum GameState
    {
        LoadingMap = 0,
        SettingPlayer = 1,
        InstantiateObject = 2,
        Intro = 3,
        Waiting = 4,
        Starting = 5,
        Playing = 6,
        Ending = 7
    }
    public static GameManagerTutorial instance;
    public Transform[] hidePos;
    public ArrayList hideorseek = new ArrayList();

    public GameState state;
    public int numberPlayer;

    public GameObject pingObject;

    [SerializeField] public float _matchLength = 300;
    public float currentMatchTime;
    public Coroutine timerCoroutine;
    public int rate;
    public int rateS;
    public JoystickComponentCtrl joystick;
    public bool isInvisible;

    public GameObject UI;

    public int numberSeek;
    public int numberHide;

    public bool seekWin;
    public PlayerSetupTutorial mainPlayer;
    public List<PlayerSetupTutorial> players = new List<PlayerSetupTutorial>();
    public List<PlayerSetupTutorial> playersMatch = new List<PlayerSetupTutorial>();
    public List<PlayerSetupTutorial> playerList = new List<PlayerSetupTutorial>();
    public List<GameObject> button = new List<GameObject>();
    public List<GameObject> buttonList = new List<GameObject>();
    public List<GameObject> grass = new List<GameObject>();
    public List<Material> grassMaterial = new List<Material>();
    public GameObject grassGroup;
    [SerializeField] public float deltaTime;
    [SerializeField] public float _fps;
    [SerializeField] public bool end = false;

    //
    //public bool startTimer = false;
    [SerializeField] public double timer = 5;
    //

    public bool startGame;
    public bool startIntro;
    public GameObject loadingScene;

    public float time;
    public List<PingTutorial> pings = new List<PingTutorial>();
   
    //Invisible Skill
    public float timeMatch;
    public Text uiTimer;
    //Door 
    
    public bool doorOpenIn30s;
    //public Slider sliderDoor;
    public Text processDoorText;
    public Image imageButton;
    public GameObject doorUI;
    public GameObject doorOpening;
    public GameObject buttonUI;
    public List<GameObject> buttonWarning = new List<GameObject>();
    public List<Transform> buttonDoor = new List<Transform>();
    //public List<GameObject> buttonOff=new List<GameObject>();
    public Text escapeText;
    //Start Game
    public GameObject startGameUI;
    public Text missionTextButton;
    public GameObject missionUI;
    public GameObject missionSeek;
    public GameObject missionHide;
    public Text timeText;
    public GameObject seekRoleUI;
    public GameObject hideRoleUI;
    public GameObject canavasGame;
    public GameObject canavasPlayingGame;
    public GameObject introMap;
    //End Game
    public GameObject uiEndGame;
    public Text winText;
    public Text loseText;
    //Other
    public List<GameObject> wallObject = new List<GameObject>();
    //Scroll View
    //public GameObject scrollViewKill;
    public GameObject itemSeekerAppear;
    public GameObject itemKill;
    public Text playerName1;
    public Text playerName2;
    public GameObject itemHelp;
    public Text playerHelpName1;
    public Text playerHelpName2;
    public GameObject itemDead;
    public Text playerDeadName1;
    public Text textSeekerAppear;
    public Text timeSeekerAppear;
    //Match Info
    public Text seekNumberText;
    public Text hideNumberText;
    //public Text deadNumberText;


    public Text hideNumberChangeText;
    //public Text deadNumberChangeText;

    public int numberSeekCurrent;
    public int numberHideCurrent;
    //public int numberDeadCurrent;
    public PlayerNameData nameData;
    public bool isPlayerRevive;
    //UI Info
    public List<GameObject> listItemInfo = new List<GameObject>();
    public GameObject itemDoorOpen;
    public GameObject itemDoor30s;
    public GameObject itemButtonActive;
    public GameObject itemButtonDisactive;

    public Text buttonChangeNumber;
    public Text buttonNumber;
    public GameObject itemDoorAndButton;
    public Button actionBtn;
    public Animator scanAnim;
    public float firstTimeColdown;
    public bool changeModelSeeker;
    public GameObject seekNear;
    public GameObject waypointCanavas;
    public Light lightMap;
    public bool isHighQuality;
    public Text uiAlert;
    public GameObject crack;
    public bool isSeeker;
    public GoHome goHome;
    public SceneTutorial1 tutorial;
    public GameObject tutorialUI;
    public GameObject tutorialUI0;
    public GameObject hideBot;
    public GameObject seekBot;
    public CameraFollowTutorial camPlayer;
    public GameObject dashSkill;
    public GameObject step;
    public GameObject stepTutorial;
    public GameObject joyStickimage;
    public Animator stepAnim;
    public TextMeshProUGUI stepText;
    public GameObject skip;
    public GameObject intro2;
    public TextMeshProUGUI intro2text;
    public GameObject arrow;
    public GameObject grassTutorial;
    public GameObject grassTutorial2;
    public GameObject infinity;
    public GameObject timeMatchUI;
    public Text numberHiderTutorial;
    public Text timeSeekerTutorial;
    public Image backGroundBlack;
    public TextMeshProUGUI touchToContinue;
    public List<PlayerSetupTutorial> botList = new List<PlayerSetupTutorial>();
    public bool firstTarget;
    public List<Transform> targetCameraStep3a = new List<Transform>();

    public GameObject infoRing;
    public TextMeshProUGUI textInfoRingAndDoor;
    public Image nextRingToOutRing;
    public Image nextOutRingToRing;
    public Image nextDoorToOutRing;
    public Image nextOutRingToDoor;
    public Image dot1;
    public Image dot2;
    public Image dot2_2;
    public Image imageInfoRing;
    public Image imageInfoDoor;
    public Image imageInfoOutRing;
    public Animator imageButtonDoor;
    public GameObject buttonXacNhan;

    public GameObject infoSave;
    public TextMeshProUGUI textInfoSaveAndSeekNear;
    public Image nextSaveAndSeekNear;
    public Image nextSeekNearAndSave;
    public Image dot3;
    public Image dot4;
    public Image imageInfoSave;
    public Image imageInfoSeekNear;
    public Animator imageSaveSeekNear;
    public GameObject buttonXacNhan2;

    public GameObject infoSeekerAppear;
    public TextMeshProUGUI textinfoSeekerAppear;
    public Image next5sandMisson;
    public Image nextMissonand5s;
    public Image dot5;
    public Image dot6;
    public Image imageInfo5s;
    public Image imageInfoMisson;
    public Animator image5sMisson;
    public GameObject buttonXacNhan3;


    public List<Transform> tableAI = new List<Transform>();
    public LineRenderer lineRenderer;
    public Transform targetSeek;
    public Transform posEscape1;
    public Transform posEscape2;
    public isScene scene;
    public bool stop;
    public GameObject zone;
    public GameObject alertZone;
    public TMPro.TextMeshProUGUI alertTimeZone;
    public GameObject currentSafeZone;
    public FadeScene fadeScene;
    public GameObject timeUI;
    public GameObject infoMatchNew;
    public GameObject arrowZone;
    public GameObject arrowScan;
    public GameObject arrowInfomatch;
    public GameObject popupMenu;
    public GameObject popupSkip;
    public GameObject backgroundBlack2;
    public enum isScene
    {
        Scene1,
        Scene2,
        Scene3
    }
    public enum SceneTutorial1
    {
        Step1,
        Step2,
        Step3,
        Step4,
        Step5,
        Step6,
        Step7,
        Step8
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        numberPlayer = RoomManager2.Instance.players;
        numberSeekCurrent = numberSeek;
        numberHideCurrent = numberHide;
        seekNumberText.text = numberSeekCurrent.ToString();
        hideNumberText.text = numberHideCurrent.ToString();
        //deadNumberText.text = "0";
        isSeeker = false;
    }

    public void SetSeekKillHide(string name1, string name2, GameObject item)
    {
        if (item.activeSelf)
        {
            item.SetActive(false);
        }
        item.SetActive(true);
        playerName1.text = name1;
        playerName2.text = name2;
        numberHideCurrent--;
        StartCoroutine(IESetSeekKillHide(item));
    }
    public void ResetListItemInfo()
    {
        foreach (GameObject item in listItemInfo)
        {
            item.SetActive(false);
        }
    }
    public void SetHideReviveHide(string name1, string name2, GameObject item)
    {
        if (item.activeSelf)
        {
            item.SetActive(false);
        }
        item.SetActive(true);
        playerHelpName1.text = name2;
        playerHelpName2.text = name1;
        numberHideCurrent++;
        StartCoroutine(IESetSeekKillHide(item));
    }
    public void BackToMenu()
    {
        Time.timeScale = 1;
        fadeScene.FadeToLevel("Menu");
    }
    public void Cancel()
    {
        Time.timeScale = 1;
        popupSkip.SetActive(false);
        backgroundBlack2.SetActive(false);
    }
    public void Skip()
    {
        FirebaseManager.LogEventTutorial("skip", "SKIP");
        popupSkip.SetActive(true);
        backgroundBlack2.SetActive(true);
        Time.timeScale = 0;
    }
    public void SetHideDie(string name1, GameObject item)
    {
        if (item.activeSelf)
        {
            item.SetActive(false);
        }
        item.SetActive(true);
        playerDeadName1.text = name1;
        //numberDeadCurrent++;
        StartCoroutine(IESetSeekKillHide(item));
    }
    public void SetSeekerAppear(GameObject item)
    {
        if (item.activeSelf)
        {
            item.SetActive(false);
        }
        item.SetActive(true);
        StartCoroutine(IESetSeekKillHide(item));
    }
    public void ItemInfoActive(GameObject item)
    {
        item.SetActive(true);
        StartCoroutine(IESetSeekKillHide(item));
    }
    IEnumerator IESetSeekKillHide(GameObject item)
    {
        yield return new WaitForSeconds(4f);
        item.SetActive(false);
    }
    
}
