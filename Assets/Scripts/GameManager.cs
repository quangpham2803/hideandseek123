using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.IO;
using UnityEngine.AI;
using Photon.Pun.UtilityScripts;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks,IPunObservable
{
    public static GameManager instance;
    public List<Transform> hidePos;
    public ArrayList hideorseek = new ArrayList();

    public GameState state;
    public int numberPlayer;
    public GameObject pingObject;

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
    public Text playerWin;
    public Text playerLose;
    public float currentMatchTime;
    public Coroutine timerCoroutine;
    public int rate;
    public int rateS;
    public JoystickComponentCtrl joystick;
    public List<PlayerSetup> players = new List<PlayerSetup>();
    public List<PlayerSetup> playersMatch = new List<PlayerSetup>();
    public List<PlayerSetup> playerList = new List<PlayerSetup>();
    public bool isInvisible;

    public GameObject UI;
    public int numberSeek;
    public int numberHide;
    public bool seekWin;
    public PlayerSetup mainPlayer;
    public List<GameObject> button = new List<GameObject>();
    public List<GameObject> buttonList = new List<GameObject>();
    public List<GameObject> grass = new List<GameObject>();
    public List<Material> grassMaterial = new List<Material>();
    public GameObject invisiblePrefab;
    public GameObject minimapCam;
    public GameObject grassGroup;
    [SerializeField] public float deltaTime;
    [SerializeField] public float _fps;
    [SerializeField] public bool end = false;
    public bool startGame;
    public bool startIntro;
    public GameObject loadingScene;
    public float time;
    public List<Ping> pings = new List<Ping>();
    // Character
    public ArrayList seekCharacter = new ArrayList() { Character.Jokers, Character.DarkRaven, Character.Davros/*, Character.DarkHunter, Character.Killer, Character.Voldemort */};
    public ArrayList seekCharacterBot = new ArrayList() {Character.Jokers, Character.DarkRaven,  Character.Davros/*, Character.DarkHunter, Character.Killer, Character.Voldemort */};
    public ArrayList hideCharacter = new ArrayList() { Character.TransformMan, Character.Wizard, Character.SpeedMan };
    public ArrayList hideCharacterBot = new ArrayList() { Character.TransformMan, Character.Wizard, Character.SpeedMan };
    public ArrayList seekBotList = new ArrayList();
    public ArrayList hideBotList = new ArrayList();
    public List<PlayerSetup> botList = new List<PlayerSetup>();
    public List<PlayerSetup> playerListRoom = new List<PlayerSetup>();
    //JokerSkill
    public GameObject pingToxicObject;
    public GameObject jokerToxicObject;
    public List<PlayerJoker> summonJoker = new List<PlayerJoker>();
    public List<JokerToxic> jokerToxic = new List<JokerToxic>();
    public List<JokerBomb> jokerBombList = new List<JokerBomb>();
    public List<PingToxic> pingToxic = new List<PingToxic>();
    //DarkRavenSkill
    public List<Raven> summonRaven = new List<Raven>();
    public List<Pulse> shotRaven = new List<Pulse>();
    public List<GameObject> effectRaven = new List<GameObject>();
    public List<GameObject> effectWizard = new List<GameObject>();
    //Invisible Skill
    public List<Invisible> invisibleObject = new List<Invisible>();

    //Wizard
    public List<WizardObject> wizardObj = new List<WizardObject>();
    //Davros Skill
    public List<ScanDavros> scanDavros = new List<ScanDavros>();
    public List<GameObject> detectedEffect = new List<GameObject>();
    public List<MuscleCramp> muscleCrampList = new List<MuscleCramp>();
    public List<Bandage> bandageList = new List<Bandage>();
    public GameObject slowEffectAffterRunningPrefab;
    //Time
    public float timeMatch;
    public TextMeshProUGUI uiTimer;
    //Door 
    public bool doorOpenIn30s;
    //public Slider sliderDoor;
    public Text processDoorText;
    public Image imageButton;
    //public GameObject doorUI;
    public GameObject doorOpening;
    public GameObject buttonUI;
    public List<GameObject> buttonWarning=new List<GameObject>();
    public List<Transform> buttonDoor=new List<Transform>();
    //public List<GameObject> buttonOff=new List<GameObject>();
    public TextMeshProUGUI escapeText;
    //Start Game
    public GameObject startGameUI;
    public GameObject seekRoleUI;
    public GameObject hideRoleUI;
    public GameObject randomRoleUI;
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
    public TextMeshProUGUI seekNumberText;
    public TextMeshProUGUI hideNumberText;
    public Text hideNumberChangeText;
    public int numberSeekCurrent;
    public int numberHideCurrent;
    //public int numberDeadCurrent;
    public PlayerNameData nameData;
    public bool isPlayerRevive;
    public bool playerNeedRevive;
    //UI Info
    public List<GameObject> listItemInfo = new List<GameObject>();
    public Button actionBtn;
    public Animator scanAnim;
    public float firstTimeColdown;
    public bool changeModelSeeker;
    public GameObject seekNear;
    public GameObject waypointCanavas;
    public Light lightMap;
    public bool isHighQuality;
    public TextMeshProUGUI uiAlert;
    public GameObject crack;
    public bool requestSend;
    public bool isSeeker;

    public TextMeshProUGUI debuff;
    public Color colorBuff;
    public Color colorDebuff;
    public GoHome goHome;
    //public GameObject silenceUI;
    public List<Transform> grassPointAI = new List<Transform>(); 
    public List<Transform> grassPointAIHideMove = new List<Transform>(); 
    public List<Transform> cloneTranform = new List<Transform>();
    public List<Transform> tableAI = new List<Transform>();
    public Transform escapePoint;
    //public int numberPlayerCup;
    public GameObject zone;
    public GameObject alertZone;
    public TMPro.TextMeshProUGUI alertTimeZone;
    public Target currentSafeZone;
    public List<LineRender> lineRenderer = new List<LineRender>();
    public RectTransform rect;
    
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
        if (RoomManager2.Instance.needBot)
        {
            SetPlayersInRoom(PhotonNetwork.CurrentRoom.MaxPlayers);
        }
        else
        {
            SetPlayersInRoom(numberPlayer);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < numberHide; i++)
            {
                hideorseek.Add(HideOrSeek.Hide);
            }
            for (int i = 0; i < numberSeek; i++)
            {
                hideorseek.Add(HideOrSeek.Seek);
            }
        }
        //SettingMap();
        numberSeekCurrent = numberSeek;
        numberHideCurrent = numberHide;
        seekNumberText.text = "0";
        hideNumberText.text = (numberHideCurrent+ numberSeekCurrent).ToString();
        //deadNumberText.text = "0";
        isSeeker = false;
    }
    public int isHight;
    public int isShadow;
    void SettingMap()
    {
        isHight = PlayerPrefs.GetInt("isHight");
        isShadow = PlayerPrefs.GetInt("isShadow");
        if (isHight == 1)
        {
            crack.SetActive(true);
        }
        else
        {
            crack.SetActive(false);
        }
        if (isShadow == 1)
        {
            lightMap.shadows = LightShadows.Hard;
        }
        else
        {
            lightMap.shadows = LightShadows.None;
        }
    }
    void SetPlayersInRoom(int _numberPlayer)
    {
        switch (_numberPlayer)
        {
            case 1:
                {
                    numberHide = 0;
                    numberSeek = 1;
                    break;
                }
            case 2:
                {
                    numberHide = 1;
                    numberSeek = 1;
                    break;
                }
            case 3:
                {
                    numberHide = 2;
                    numberSeek = 1;
                    break;
                }
            case 4:
                {
                    numberHide = 2;
                    numberSeek = 2;
                    break;
                }
            case 5:
                {
                    numberHide = 4;
                    numberSeek = 1;
                    break;
                }
            case 6:
                {
                    numberHide = 5;
                    numberSeek = 1;
                    break;
                }
            case 7:
                {
                    numberHide = 5;
                    numberSeek = 2;
                    break;
                }
            case 8:
                {
                    numberHide = 5;
                    numberSeek = 3;
                    break;
                }
            case 9:
                {
                    numberHide = 6;
                    numberSeek = 3;
                    break;
                }
            case 10:    
                {
                    numberHide = 7;
                    numberSeek = 3;
                    break;
                }
        }
    }
   

    public void SetSeekKillHide(string name1,string name2,GameObject item)
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
        playerHelpName1.text = name1;
        playerHelpName2.text = name2;
        numberHideCurrent++;
        StartCoroutine(IESetSeekKillHide(item));
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    public enum Character
    {
        Jokers,
        Davros,
        DarkRaven,
        DarkHunter,
        Voldemort,
        Killer,
        SpeedMan,
        Wizard,
        TransformMan
    }
    public enum HideOrSeek
    {
        Hide,
        Seek
    };
}