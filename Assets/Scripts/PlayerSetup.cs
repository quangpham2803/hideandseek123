using I2.Loc;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    //Only player
    public GameManager gameManager;
    public GameObject hideMission;
    public GameObject seekMission;
    public int id;
    //Only bot
    public bool isBot;
    public bool isSummon;
    public NavMeshAgent agent;
    public bool isHitPlayer;
    public bool isScan;
    public bool useSkill1;
    public bool useSkill2;
    public PlayerSetup playerBotHit;
    public bool isReviving;
    //Other

    public float hideSpeed;
    public float seekSpeed;


    public float timeToFire = 2f;
    public bool attack;
    public bool attacked;

    public GameManager.HideOrSeek player;
    public GameManager.HideOrSeek playerTemp;
    public int roleNumber;
    public int actor;
    public float currentSpeed;

    public float activationTime;
    public bool invisible;
    public float runTime;
    public float scanTime;
    public bool run;

    public Target target;
    public bool inGrass=false;
    public Transform cameraPrefab;
    public CameraFollow cam;
    public TMPro.TextMeshProUGUI name;
    public bool changePlayerInGrass;
    public GameObject grass;
    public bool defaultMater;
    public Animator pAnimator;

    public GameObject invisibleObject;
    //Bot
    public List<PlayerSetup> inRange = new List<PlayerSetup>();
    public List<PlayerSetup> listPlayerRevive = new List<PlayerSetup>();
    public float range;
    public List<PlayerSetup> inRangeOfBot = new List<PlayerSetup>();
    public bool isbuttonActive;
    public Transform buttonMove;
    public bool isBlackHole;
    public bool isplay = false;
    public Vector3 targetPoint;

    public GameObject weapon;
    public Image circle;
    public bool isChangeToSeeker;
    public bool isReady = false;
    public bool isSettingDone = false;

    public bool dead;
    public bool deadComplete;
    public GameManager.Character character;
    public GameManager.Character characterTemp;
    // Joker Skill
    public PlayerJoker summonJoker;
    public JokerToxic jokerToxic;
    public bool isJokerInvisible;
    //DarkRaven
    public Raven summonDarkRaven;
    public bool isSummonDarkRaven;

    public GameObject slowEffectObj;
    //Wizard
    //public Image wizardUI;
    public GameObject model;
    public GameObject seekModelHider;
    public GameObject seekModel;

    public List<GameObject> skill=new List<GameObject>();
    public List<GameObject> skillTemp=new List<GameObject>();
    public float defaultSpeed;
    public List<Material> defaultMaterial;
    public List<Material> defaultMaterialSeek;


    public JoystickComponentCtrl joystick;
    public NavMeshAgent agentPlayer;
    public CapsuleCollider collider;

    public PlayerRPC rpc;

    public bool coverGrass;
    public bool outGrass;
    public List<GameObject> grassList = new List<GameObject>();

    public float speedDown;
    public float speedStun =1;
    public float speedHook =1;
    public float speedDie =1 ;
    public float speedUp;
    public float speedAttack;
    public float speedDoorOpen;
    public float speedBot;

    public Material[] invisibleMaterial;
    public Material[] invisibleOtherTeamMaterial;

    public List<GameObject> ObjectInvisible = new List<GameObject>();

    public Quaternion targetRotation;
    public Vector3 direction;
    public Vector3 motion;

    public List<PlayerSetup> playerInRange = new List<PlayerSetup>();
    public List<PlayerSetup> playerFriend = new List<PlayerSetup>();
    public bool isTranform;
    public bool isbeStun;
    public bool isSlow;
    public bool isFear;
    public bool isSlowDarvos;

    //public GameObject fixObject;
    public GameObject shotWeapon;
    public float horizontal;
    public float vertical;

    public bool isImmute;
    public GameObject stunEffect;
    public Sprite iconDead;
    public GameObject fixObject;
    public Image fillBar;
    public GameObject deadArrow;
    public ReviveCircle reviveCircle;
    public bool deadBool;
    public bool isMoving;
    public bool isTired;

    public GameObject runEffect;
    public GameObject walkEffect;
    public GameObject slowEffectAffterRunningObj;
    public bool isFollow;
    public Transform targetFollow;
    public bool isFast;
    public bool isJumping;
    public Button actionBtn;
    public GameObject checkSeekNear;
    public bool isAttack;
    public bool jointMatch;
    public bool isWall;
    public GameObject deadPos;
    public GameObject seekerName;
    public Color hideColor;
    public Color seekColor;
    public Color hideColorTeam;
    public Color seekColorTeam;
    public Color outlineSeek;
    public Color outlineHide;
    public Outline outLineName;
    //Message
    public Image messageHelp;
    public Image messageLike;
    public Image messageNoHelp;
    public List<Image> messageItem = new List<Image>();

    public List<GameObject> objectDisableAfterDead = new List<GameObject>();
    public bool isSilent;
    public int hideId;
    public int seekId;
    public GameObject seekBuff;
    public GameObject wizardSpeedUp;
    public GameObject arrowMain;
    public GameObject debuff;
    public TMPro.TextMeshProUGUI debuffText;

    public bool needHelp;
    public GameObject teleEffect;
    public float winPercent;
    public GameObject[] boxEffect;
    public float speedUpZone;
    public float speedDownZone;
    public SpawnSlowBomb slowBomb;
    public TrashSlide slide;
    public FootPrintSpawner footPrinter;
    public float speedSlide;
    public float speedDash;
    public GameObject stunPT;
    public Target killedTarget;
    //public float a;
    void Start()
    {
        rpc = GetComponent<PlayerRPC>();
        target = GetComponent<Target>();
        collider = GetComponent<CapsuleCollider>();
        target.enabled = false;
        target.owner = gameObject;
        speedStun = 1;
        speedAttack = 1;
        speedSlide = 1;
        speedDash = 1;
        defaultMater = true;
        gameManager = GameManager.instance;
        if (isBot)
        {
            isSettingDone = true;
            defaultSpeed = hideSpeed;
            agent = GetComponent<NavMeshAgent>();
            agent.enabled = true;
            scanTime = 0;
            activationTime = 0;
            runTime = 0;
            invisible = false;
            run = false;
            return;
        }
        else
        {
            gameManager.players.Add(this);
        }
        if (photonView.IsMine)
        {
            agentPlayer.enabled = false;
            GameManager.instance.mainPlayer = this;           
            GameManager.instance.GetComponent<ButtonClick>().id = photonView.ViewID;
            cam.transform.SetParent(null);
            Debug.LogError("SKIP PLAYER INFO FIX LATER");

            //int win = int.Parse(StatusPlayer.Instance.dataPlayer["win"]);
            // int match = int.Parse(StatusPlayer.Instance.dataPlayer["match"]);
            int win = 1;
            int match = 2;
            photonView.RPC("PlayerJointedMatch",
                RpcTarget.AllBuffered, PlayerPrefs.GetInt("hideId"),PlayerPrefs.GetInt("seekId"),
                match,win);
        }
        else
        {
            cam.cinemachine.gameObject.SetActive(false);
            cam.gameObject.SetActive(false);
        }
        fixObject.SetActive(false);
        deadArrow.SetActive(false);
        reviveCircle.gameObject.SetActive(false);
        isMoving = false;
        isFollow = false;
        isFast = false;
        isJumping = false;
        checkSeekNear.SetActive(false);
        teleEffect.SetActive(false);
    }
    IEnumerator activeArrowMain()
    {
        arrowMain.SetActive(true);
        yield return new WaitForSeconds(4f);
        arrowMain.SetActive(false);
    }
    bool start = false;
    bool k = false;
    [PunRPC]
    void PlayerJointedMatch(int hide, int seek, int match, int win)
    {
        jointMatch = true;
        hideId = hide;
        seekId = seek;
        float matchTm = match;
        float winTm = win;
        if (!isBot)
        {
            if(match > 0)
            {
                winPercent = (winTm / matchTm) * 100f;
            }
            else
            {
                winPercent = 0f;
            }
        }
    }
    public float d1;
    public float d2;
    public bool isPause; 
    float timeDead = 0;
    public bool isFake;
    public float rangeDetected;
    public float rangeDetectedIncrease;
    IEnumerator WaitDead()
    {
        yield return new WaitForSeconds(1f);
    }
     void Update()
    {
        if (!photonView.IsMine)
            return;
        if (isSummon)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            isbeStun = true;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(SlowBuff(5, this, slowEffectAffterRunningObj, 1, "Seek"));
        }
        if (dead && !deadBool)
        {
            StartCoroutine(WaitDead());
            if (photonView.IsMine)
            {
                reviveCircle.cam = cam;
            }
            photonView.RPC("DisapearAfterDead", RpcTarget.AllBuffered);
        }

        if (gameManager.state == GameManager.GameState.SettingPlayer && !t && !isBot)
        {
            StartCoroutine(WaitForSync());
        }
        if (!isBot)
        {
            if (gameManager.state==GameManager.GameState.InstantiateObject &&!k)
            {
                k = true;
                for (int i = 0; i < gameManager.players.Count; i++)
                {
                    GameObject pingObj = Instantiate(gameManager.pingObject, Vector3.zero, gameManager.pingObject.transform.rotation);
                    pingObj.GetComponent<Ping>().owner = gameManager.players[i];
                    gameManager.pings.Add(pingObj.GetComponent<Ping>());
                    pingObj.transform.position = pingObj.GetComponent<Ping>().owner.transform.position;
                    pingObj.transform.SetParent(pingObj.GetComponent<Ping>().owner.transform);
                    pingObj.SetActive(false);
                }
                foreach (PlayerSetup p in gameManager.players)
                {
                    gameManager.GetComponent<SetupPlayer>().GetCharacter(p, p.character);
                    if (p.playerTemp == GameManager.HideOrSeek.Seek)
                    {
                        gameManager.GetComponent<SetupPlayer>().GetCharacterSeekerModelHider(p, p.characterTemp);
                        p.SetAnimation(p, p.model);
                        p.ChangeModel();
                    }
                    gameManager.GetComponent<SetupPlayer>().SetObjectPlayerLocal(p, p.character);
                    p.SetAnimation(p, p.model);
                    if (!p.isBot)
                    {
                        
                        p.transform.position = gameManager.hidePos[p.id].transform.position;
                    }
                    else
                    {
                        p.transform.position = gameManager.hidePos[p.id].transform.position;
                        
                        if (PhotonNetwork.IsMasterClient)
                        {
                            p.agent.Warp(p.transform.position);
                            gameManager.GetComponent<SetupPlayer>().SetSkill(p, p.character);
                        }
                    }
                    p.pAnimator = p.model.GetComponent<Animator>();
                }
                if (player == GameManager.HideOrSeek.Seek)
                {
                    gameManager.GetComponent<SetupPlayer>().SetSkillSeekerModelHider(this, characterTemp);
                }
                gameManager.GetComponent<SetupPlayer>().SetSkill(this, character);
                gameManager.GetComponent<SetupPlayer>().SetObjectPlayerPhoton(this, character);
                foreach (GameObject item in skill)
                {
                    item.SetActive(false);
                }
                foreach (GameObject item in skillTemp)
                {
                    item.SetActive(false);
                }
                photonView.RPC("SettingDone", RpcTarget.AllBuffered, true);
            }
            if (gameManager.requestSend && gameManager.state == GameManager.GameState.InstantiateObject)
            {
                photonView.RPC("SettingDone", RpcTarget.AllBuffered, true);
                gameManager.requestSend = false;
            }
            if (gameManager.state==GameManager.GameState.Intro && k)
            {
                k = false;
                for (int i = 0; i < gameManager.players.Count; i++)
                {
                    gameManager.playerList.Add(gameManager.players[i]);
                    gameManager.playersMatch.Add(gameManager.players[i]);
                    if (gameManager.players[i] != this)
                    {
                        gameManager.players[i].rpc.GetMaterial(gameManager.players[i]);
                        gameManager.players[i].rpc.GetInvisibleMaterial(gameManager.players[i]);
                        gameManager.players[i].rpc.GetInvisibleOtherTeamMaterial(gameManager.players[i]);
                        if (gameManager.players[i].player == GameManager.HideOrSeek.Seek)
                        {
                            gameManager.players[i].rpc.GetMaterialSeek(gameManager.players[i]);
                        }
                    }
                }
                if (player == GameManager.HideOrSeek.Seek)
                {
                    rpc.GetMaterialSeek(this);
                }
                rpc.GetMaterial(this);
                rpc.GetInvisibleMaterial(this);
                rpc.GetInvisibleOtherTeamMaterial(this);
            }
            if (gameManager.state==GameManager.GameState.Playing && !k)
            {
                k = true;
                if (photonView.IsMine)
                {
                    StartCoroutine(activeArrowMain());
                }              
                gameManager.textSeekerAppear.gameObject.SetActive(true);
                gameManager.timeSeekerAppear.gameObject.SetActive(true);
                if (playerTemp == GameManager.HideOrSeek.Seek)
                {
                    foreach (GameObject item in skillTemp)
                    {
                        item.SetActive(true);
                    }
                    foreach (GameObject item in skill)
                    {
                        item.SetActive(false);
                    }
                    photonView.RPC("ChangeCharacter",RpcTarget.AllBuffered, (byte)GameManager.HideOrSeek.Hide);
                    StartCoroutine(SeekerAppear());
                }
                else
                {
                    if(this== gameManager.mainPlayer)
                    {
                        StartCoroutine(LookAtSeek());
                    }                   
                    foreach (GameObject item in skill)
                    {
                        item.SetActive(true);
                    }
                }
            }
            if (gameManager.state == GameManager.GameState.Playing && gameManager.changeModelSeeker)
            {
                gameManager.seekNumberText.text = gameManager.numberSeekCurrent.ToString();
                gameManager.hideNumberText.text = gameManager.numberHideCurrent.ToString();
                gameManager.changeModelSeeker = false;
                if (player == GameManager.HideOrSeek.Seek)
                {
                    cam.cinemachine.m_Lens.OrthographicSize = 6;
                }
                else
                {
                    if (model.GetComponent<PlayerStat>().redBody !=null)
                    {
                        model.GetComponent<PlayerStat>().redBody.transform.parent = null;
                    }
                }
                foreach (PlayerSetup p in gameManager.players)
                {
                    if (p.playerTemp == GameManager.HideOrSeek.Seek)
                    {
                        p.seekModel.transform.localPosition = new Vector3(0f, 0.2f, 0f);
                        p.seekModel.SetActive(true);
                        p.model = p.seekModel;
                        p.seekModelHider.SetActive(false);
                        //p.defaultMaterial.Clear();
                        p.invisibleMaterial = null;
                        p.invisibleOtherTeamMaterial = null;
                        p.defaultMaterial = p.defaultMaterialSeek;
                        p.rpc.GetInvisibleMaterial(p);
                        p.rpc.GetInvisibleOtherTeamMaterial(p);
                        p.pAnimator = p.model.GetComponent<Animator>();
                        p.seekerName.SetActive(true);
                    }
                    if (p.playerTemp == player && player == GameManager.HideOrSeek.Seek)
                    {
                        p.name.color = seekColorTeam;
                        p.circle.color = seekColorTeam;
                        p.outLineName.effectColor = outlineSeek;
                    }
                    else if (p.playerTemp != player && player == GameManager.HideOrSeek.Hide)
                    {
                        p.name.color = seekColor;
                        p.circle.color = seekColor;
                        p.outLineName.effectColor = outlineSeek;
                    }
                    else if (p.playerTemp != player && player == GameManager.HideOrSeek.Seek)
                    {
                        p.name.color = hideColor;
                        p.circle.color = hideColor;
                        p.outLineName.effectColor = outlineHide;
                    }
                    if (p.playerTemp != playerTemp)
                    {
                        foreach (Image item in p.messageItem)
                        {
                            item.gameObject.SetActive(false);
                        }
                    }
                    if (player == GameManager.HideOrSeek.Seek)
                    {
                        if (p.playerTemp == GameManager.HideOrSeek.Seek)
                        {
                            p.seekerName.SetActive(false);
                        }
                    }
                    p.pAnimator = p.model.GetComponent<Animator>();
                }
                if (this == gameManager.mainPlayer && playerTemp == GameManager.HideOrSeek.Hide)
                {
                    checkSeekNear.SetActive(true);
                }
                else
                {
                    checkSeekNear.SetActive(false);
                }
                if (player == GameManager.HideOrSeek.Seek)
                {
                    name.color = seekColor;
                    circle.color = seekColor;
                    outLineName.effectColor = outlineSeek;
                }
                else
                {
                    name.color = hideColor;
                    circle.color = hideColor;
                    outLineName.effectColor = outlineHide;
                }
            }
            if (deadTemp)
            {
                //GetComponent<Player>().rigidbody.velocity = 2 * transform.position - playerKill;
                transform.position = Vector3.MoveTowards(transform.position, 2 * transform.position - playerKill, 0.1f);
            }
        }
        else
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            if (gameManager.state == GameManager.GameState.InstantiateObject &&  !k)
            {
                k = true;
                gameManager.GetComponent<SetupPlayer>().SetObjectPlayerPhoton(this, character);
            }
            if (gameManager.state == GameManager.GameState.Playing && !o)
            {
                o = true;
            }
            if (gameManager.state == GameManager.GameState.Playing && k)
            {
                k = false;
                if (gameManager.mainPlayer.playerTemp == GameManager.HideOrSeek.Seek)
                {
                    if (playerTemp == GameManager.HideOrSeek.Seek)
                    {
                        range = 8f + 8f * (50f - gameManager.GetComponent<SetupPlayer>().winPercent)/ 100f;
                        speedBot = 1f * (50f - gameManager.GetComponent<SetupPlayer>().winPercent) / 100f;
                        if (50f - gameManager.GetComponent<SetupPlayer>().winPercent < 0)
                        {
                            rangeDetectedIncrease = 0f;
                        }
                        else
                        {
                            rangeDetectedIncrease = 5f * (50f - gameManager.GetComponent<SetupPlayer>().winPercent) / 100f;
                        }
                    }
                    else
                    {
                        range = 9f - 5f * (50f - gameManager.GetComponent<SetupPlayer>().winPercent) / 100f;
                        speedBot = -1f * (50f - gameManager.GetComponent<SetupPlayer>().winPercent) / 100f;
                        if (50f - gameManager.GetComponent<SetupPlayer>().winPercent > 0)
                        {
                            rangeDetectedIncrease = 0f;
                        }
                        else
                        {
                            rangeDetectedIncrease = -5f * (50f - gameManager.GetComponent<SetupPlayer>().winPercent) / 100f;
                        }
                    }
                }
                else
                {
                    if (playerTemp == GameManager.HideOrSeek.Seek)
                    {
                        range = 8f - 8f * (50f - gameManager.GetComponent<SetupPlayer>().winPercent) / 100f;
                        speedBot = -1f * (50f - gameManager.GetComponent<SetupPlayer>().winPercent) / 100f;
                        if (50f - gameManager.GetComponent<SetupPlayer>().winPercent > 0)
                        {
                            rangeDetectedIncrease = 0f;
                        }
                        else
                        {
                            rangeDetectedIncrease = -5f * (50f - gameManager.GetComponent<SetupPlayer>().winPercent) / 100f;
                        }
                    }
                    else
                    {
                        range = 9f + 5f * (50f - gameManager.GetComponent<SetupPlayer>().winPercent) / 100f;
                        speedBot = 1f * (50f - gameManager.GetComponent<SetupPlayer>().winPercent) / 100f;
                        if (50f - gameManager.GetComponent<SetupPlayer>().winPercent < 0)
                        {
                            rangeDetectedIncrease = 0f;
                        }
                        else
                        {
                            rangeDetectedIncrease = 5f * (50f - gameManager.GetComponent<SetupPlayer>().winPercent) / 100f;
                        }
                    }
                }
                photonView.RPC("UnParentRedBody", RpcTarget.AllBuffered);
                if (playerTemp == GameManager.HideOrSeek.Seek)
                {
                    photonView.RPC("ChangeCharacter", RpcTarget.AllBuffered, (byte)GameManager.HideOrSeek.Hide);
                }
                else
                {
                    foreach (GameObject item in skill)
                    {
                        item.SetActive(true);
                    }
                }
                StartCoroutine(SeekerBotAppear());
            }

        }
    }
    [PunRPC]
    void UnParentRedBody()
    {
        if (model.GetComponent<PlayerStat>().redBody !=null)
        {
            model.GetComponent<PlayerStat>().redBody.transform.parent = null;
        }
    }
    private bool o;
    IEnumerator SeekerBotAppear()
    {
        yield return new WaitForSeconds(5f);
        if (isBot && PhotonNetwork.IsMasterClient)
        {
            agent.enabled = false;
        }
        if (playerTemp == GameManager.HideOrSeek.Seek)
        {
            photonView.RPC("ChangeModelAnimation", RpcTarget.AllBuffered, true);
        }
        
        yield return new WaitForSeconds(1.5f);
        if (playerTemp == GameManager.HideOrSeek.Seek)
        {
            photonView.RPC("ChangeModelAnimation", RpcTarget.AllBuffered, false);
            photonView.RPC("ChangeCharacter", RpcTarget.AllBuffered, (byte)GameManager.HideOrSeek.Seek);
        }
        yield return new WaitForSeconds(0.5f);
        agent.enabled = true;
        GetComponent<Player>().isChangeCharacter = true;
        if (playerTemp == GameManager.HideOrSeek.Seek)
        {
            pAnimator.SetBool("isRunning", true);
            defaultSpeed = seekSpeed;
            foreach (GameObject item in skill)
            {
                item.transform.localPosition = new Vector3(0, 5000f, 0);
                item.SetActive(true);
            }
        }
        StartCoroutine(GetComponent<Player>().DetectPlayers());
    }

    [PunRPC]
    void ChangeCharacter(byte _role)
    {
        player = (GameManager.HideOrSeek)_role;
        if (player == GameManager.HideOrSeek.Seek)
        {
            gameManager.changeModelSeeker = true;
        }
        foreach (PlayerSetup p in gameManager.players)
        {
            p.changePlayerInGrass = true;
        }
    }
    void ChangeModel()
    {
        seekModel = model;
        model = seekModelHider;
        seekModel.SetActive(false);
    }
    IEnumerator HideAppear()
    {
        yield return new WaitForSeconds(5f);
        
    }
    IEnumerator SeekerAppear()
    {
        yield return new WaitForSeconds(5f);
        foreach (Image item in messageItem)
        {
            item.gameObject.SetActive(false);
        }
        gameManager.isSeeker = true;
        photonView.RPC("ChangeModelAnimation", RpcTarget.AllBuffered, true);
        photonView.RPC("UISeekAppear", RpcTarget.AllBuffered);
        gameManager.seekRoleUI.SetActive(true);
        gameManager.seekRoleUI.GetComponent<Animator>().SetBool("Seeker", true);
        gameManager.randomRoleUI.SetActive(false);        
        //gameManager.hideRoleUI.SetActive(false);
        speedStun = 0;
        yield return new WaitForSeconds(1.5f);
        //cam.GetComponent<CameraFilterPack_TV_WideScreenCircle>().enabled = true;
        speedStun = 1;
        photonView.RPC("ChangeModelAnimation", RpcTarget.AllBuffered, false);
        defaultSpeed = seekSpeed;
        cam.view = cam.seekView;
        foreach (GameObject item in skill)
        {
            item.SetActive(true);
        }
        foreach (GameObject item in skillTemp)
        {
            item.SetActive(false);
        }
        photonView.RPC("ChangeCharacter", RpcTarget.AllBuffered, (byte)GameManager.HideOrSeek.Seek);
        if (player == GameManager.HideOrSeek.Seek)
        {
            cam.cinemachine.m_Lens.OrthographicSize = 6;
        }

    }
    IEnumerator LookAtSeek()
    {
        yield return new WaitForSeconds(5f);        
        gameManager.hideRoleUI.SetActive(true);
        gameManager.hideRoleUI.GetComponent<Animator>().SetBool("Hider", true);
        gameManager.randomRoleUI.SetActive(false);
        foreach (PlayerSetup p in gameManager.players)
        {
            if(p.playerTemp == GameManager.HideOrSeek.Seek)
            {
                cam.owner = p;
                gameManager.waypointCanavas.SetActive(false);
                cam.cinemachine.LookAt = cam.owner.transform;
                cam.cinemachine.Follow = cam.owner.transform;
                speedStun = 0;
                break;
            }
        }
        yield return new WaitForSeconds(1.5f);
        cam.owner = this;
        cam.cinemachine.LookAt = cam.owner.transform;
        cam.cinemachine.Follow = cam.owner.transform;
        gameManager.waypointCanavas.SetActive(true);
        speedStun = 1;
    }
    [PunRPC]
    void UISeekAppear()
    {
        gameManager.SetSeekerAppear(gameManager.itemSeekerAppear);
    }
    void SetAnimation(PlayerSetup _setup, GameObject _model)
    {
        PhotonAnimatorView viewSeeker = _model.GetComponent<PhotonAnimatorView>();
        _setup.photonView.ObservedComponents.Add(viewSeeker);
        viewSeeker.SetLayerSynchronized(0, PhotonAnimatorView.SynchronizeType.Discrete);
        viewSeeker.SetParameterSynchronized("isRunning", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Continuous);
        viewSeeker.SetParameterSynchronized("Idle", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Continuous);
        viewSeeker.SetParameterSynchronized("isRunningSkill", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Continuous);
        viewSeeker.SetParameterSynchronized("Death", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Continuous);
        viewSeeker.SetParameterSynchronized("ChangeCharacter", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Continuous);
        viewSeeker.SetParameterSynchronized("Jump", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Continuous);
        viewSeeker.SetParameterSynchronized("Save", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Continuous);
        viewSeeker.SetParameterSynchronized("TossBandage", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Continuous);
        viewSeeker.SetParameterSynchronized("Tired", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Continuous);
        if (!_setup.ObjectInvisible.Contains(_model.transform.Find("RunEffect").gameObject))
        {
            _setup.ObjectInvisible.Add(_model.transform.Find("RunEffect").gameObject);
            _setup.ObjectInvisible.Add(_model.transform.Find("WalkEffect").gameObject);
        }
    }
    [PunRPC]
    void Ready()
    {
        isReady = true;
    }

    [PunRPC]
    void SettingDone(bool _t)
    {
        isSettingDone = _t;
    }

    bool t=false;

    IEnumerator WaitForSync()
    {
        t = true;
        yield return new WaitForSeconds(0f);
        defaultSpeed = hideSpeed;
        foreach (PlayerSetup p in gameManager.players)
        {
            p.name.color = hideColorTeam;
            p.circle.color = hideColorTeam;
            p.outLineName.effectColor = outlineHide;
        }
        name.color = hideColor;
        circle.color = hideColor;
        outLineName.effectColor = outlineHide;
        gameManager.state = GameManager.GameState.InstantiateObject;
    }
    [PunRPC]
    public void Attack()
    {
        foreach (AnimatorControllerParameter item in pAnimator.parameters)
        {
            pAnimator.SetBool(item.name, false);
        }
        pAnimator.SetBool("isAttack", true);
        speedAttack = 0;
        StartCoroutine(WaitForAttack());
    }

    IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(0.6f);
        speedAttack = 1;
        pAnimator.SetBool("isAttack", false);
        attacked = false;
    }
    [PunRPC]
    void DisapearAfterDead()
    {
        deadBool = true;
        foreach (GameObject item in objectDisableAfterDead)
        {
            item.SetActive(false);
        }
        if (!isBot)
        {
            cam.GetComponent<CameraFilterPack_AAA_Blood_Hit>().enabled = false;
        }
        rpc.enabled = false;
        //characterController.enabled = false;
        if(gameManager.mainPlayer.player == GameManager.HideOrSeek.Hide)
        {
            deadArrow.SetActive(true);
        }
        reviveCircle.gameObject.SetActive(true);
        reviveCircle.role = player;
    }
    [PunRPC]
    void ChangeModelAnimation(bool t)
    {
        foreach (AnimatorControllerParameter item in pAnimator.parameters)
        {
            pAnimator.SetBool(item.name, false);
        }
        pAnimator.SetBool("ChangeCharacter", t);
    }
    public IEnumerator IEButton(GameObject buttonWarning, float time)
    {
        yield return new WaitForSeconds(time);
        buttonWarning.SetActive(false);
    }

    public void ApearAfterRevive()
    {
        dead = false;        
        pAnimator.SetBool("Death", false);
        deadBool = false;
        foreach (GameObject item in objectDisableAfterDead)
        {
            item.SetActive(true);
        }
        //characterController.enabled = true;
        gameManager.playersMatch.Add(this);
        deadArrow.SetActive(false);      
    }
    public bool deadTemp;
    public Vector3 playerKill;
    [PunRPC]
    public void Catch(string playerKillName, Vector3 _pos)
    {
        if (!isBot && photonView.IsMine)
        {
            Vibrator.Vibrate(100);
            cam.StartCoroutine(cam.Shake(0.2f, 0.4f));
        }
        if (!isTranform)
        {
            foreach (PlayerSetup p in gameManager.players)
            {
                Physics.IgnoreCollision(p.collider, collider, true);
            }
            dead = true;
            GetComponent<Player>().rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            deadTemp = true;
            if (isBot)
            {
                GetComponent<Player>().enabled = false;
                agent.enabled = false;
            }
            foreach (AnimatorControllerParameter item in pAnimator.parameters)
            {
                pAnimator.SetBool(item.name, false);
            }
            pAnimator.SetBool("Death", true);
            if (character == GameManager.Character.Wizard)
            {
                GameObject redBody = model.GetComponent<PlayerStat>().redBody;
                redBody.transform.position = transform.position;
                redBody.SetActive(true);
                model.GetComponent<PlayerStat>().mainBody.SetActive(false);
            }
            //
            //redBody.GetComponent<Animator>().SetBool("Death", true);

            gameManager.SetSeekKillHide(playerKillName, name.text, gameManager.itemKill);
            gameManager.hideNumberText.text = gameManager.numberHideCurrent.ToString();
            gameManager.hideNumberChangeText.text = "-1";
            DoorGame.door.CheckPlayerDie(name.text.ToString());
            gameManager.playersMatch.Remove(this);
            StartCoroutine(AfterHit(null, playerKillName));
        }
        else
        {
            rpc.TransformBodyBack(rpc.currentId);
        }
    }
    IEnumerator AfterHit(GameObject _redBody, string _playerKillName)
    {
        if(gameManager.mainPlayer.player == GameManager.HideOrSeek.Seek)
        {
            killedTarget.enabled = true;
        }
        yield return new WaitForSeconds(0.2f);
        GetComponent<Player>().rigidbody.velocity = Vector3.zero;
        deadTemp = false;
        speedDie = 0f;
        isMoving = false;
        dead = true;
        yield return new WaitForSeconds(1f);
        //_redBody.SetActive(false);
        //_redBody.GetComponent<Animator>().SetBool("Death", false);
        if (gameManager.mainPlayer.player == GameManager.HideOrSeek.Seek)
        {
            killedTarget.enabled = false;
        }
        StartCoroutine(IEButton(gameManager.hideNumberChangeText.gameObject, 2f));
        Utilities.FxButtonPress(gameManager.hideNumberText.transform, false);
        Utilities.FxButtonPress(gameManager.hideNumberChangeText.transform, false);

    }
    IEnumerator SetPos(PlayerSetup setup, Vector3 pos)
    {
        //setup.characterController.enabled = false;
        setup.transform.position = pos;
        yield return new WaitForSeconds(0.1f);
        //setup.characterController.enabled = true;
    }

    public IEnumerator SlowTime(float time, PlayerSetup p,GameObject slowEffect,float _speedSlow)
    {
        foreach (PlayerSetup item in gameManager.players)
        {
            if (item.player == GameManager.HideOrSeek.Hide && (p.inGrass))
            {
            }
            else if (item.player == GameManager.HideOrSeek.Seek)
            {
                slowEffect.SetActive(true);
                slowEffect.transform.position = p.transform.position;
                slowEffect.transform.SetParent(p.transform);
            }
        }
        isSlow = true;

        p.speedDown += _speedSlow;
        yield return new WaitForSeconds(time);
        isSlow = false;
        p.speedDown -= _speedSlow;
        if (slowEffect.activeSelf)
        {
            slowEffect.SetActive(false);
        }
    }
    public IEnumerator SlowTimeAfterRun(float time, PlayerSetup p, GameObject slowEffect, float _speedSlow)
    {
        p.AddDebuffText("SLOW",1);
        isTired = true;
        foreach (PlayerSetup item in gameManager.players)
        {
            if (item.player == GameManager.HideOrSeek.Hide && (p.inGrass))
            {
            }
            else if (item.player == GameManager.HideOrSeek.Seek)
            {
                slowEffect.SetActive(true);
                slowEffect.transform.position = p.transform.position;
                slowEffect.transform.SetParent(p.transform);
            }
        }
        if (p.photonView.IsMine && !p.dead)
        {
            p.pAnimator.SetBool("Tired", true);
        }
        p.speedDown += _speedSlow;
        yield return new WaitForSeconds(time);
        p.RemoveDebuffText();
        isTired = false;
        p.speedDown -= _speedSlow;
        if (p.photonView.IsMine && !p.dead)
        {
            p.pAnimator.SetBool("Tired", false);
        }
        if (slowEffect.activeSelf)
        {
            slowEffect.SetActive(false);
        }
    }
    public IEnumerator FearTime(float time, PlayerSetup p, GameObject fearEffect)
    {
        p.AddDebuffText("FEAR",1);
        foreach (PlayerSetup item in gameManager.players)
        {
            if (item.player == GameManager.HideOrSeek.Seek && (p.inGrass || p.invisible || p.isTranform))
            {
            }
            else if (item.player == GameManager.HideOrSeek.Hide)
            {
                fearEffect.SetActive(true);               
                fearEffect.transform.position = p.transform.position;
                fearEffect.transform.SetParent(p.transform);
            }
        }
        p.isFear = true;
        //if (p == gameManager.mainPlayer)
        //{
        //    p.cam.GetComponent<CameraFilterPack_FX_Drunk>().enabled = true;
        //}
        yield return new WaitForSeconds(time);
        p.RemoveDebuffText();
        p.isFear = false;
        //if(p == gameManager.mainPlayer)
        //{
        //    p.cam.GetComponent<CameraFilterPack_FX_Drunk>().enabled = false;
        //}
        
        if (!p.isBot)
        {
            p.cam.GetComponent<Animator>().SetBool("isFear", false);
        }      
        if (fearEffect != null && fearEffect.activeSelf)
        {
            fearEffect.SetActive(false);
            if (ObjectInvisible.Contains(fearEffect))
            {
                ObjectInvisible.Remove(fearEffect);
            }
        }
    }
    string TranslateBuff(string buffName)
    {
        string buffTranslate = "";
        switch (buffName)
        {
            case "STUN":
                buffTranslate = "CHOÁNG";
                break;
            case "FEAR":
                buffTranslate = "HOẢNG SỢ";
                break;
            case "SLOW":
                buffTranslate = "BỊ LÀM CHẬM";
                break;
            case "SPEED UP":
                buffTranslate = "TĂNG TỐC";
                break;
            case "SPEED UP ALL TEAM":
                buffTranslate = "TĂNG TỐC ĐỘI";
                break;
            case "SLOW BY HIDER":
                buffTranslate = "BỊ LÀM CHẬM BỞI HIDER";
                break;
            case "SLOW BY SEEKER":
                buffTranslate = "BỊ LÀM CHẬM BỞI SEEKER";
                break;
            case "FEAR BY HIDER":
                buffTranslate = "HOẢNG SỢ BỞI HIDER";
                break;
            case "FEAR BY SEEKER":
                buffTranslate = "HOẢNG SỢ BỞI SEEKER";
                break;
            case "ENEMY DETECTED":
                buffTranslate = "THẤY ĐỊCH";
                break;
            case "MISS":
                buffTranslate = "HỤT";               
                break;
        }
        return buffTranslate;
    }
    public void AddDebuffText(string debuffname, int type)
    {
        debuffText.text = debuffname;
        if (LocalizationManager.CurrentLanguage == "English")
        {    
            debuffText.font = StatusPlayer.Instance.fontList[0];
        }
        else
        {
            debuffname = TranslateBuff(debuffname);
            debuffText.font = StatusPlayer.Instance.fontList[1];
        }
        debuffText.text = debuffname;
        if (type == 0)
        {
            debuffText.text = "<color=#0AF5F5>"+ debuffText.text + "</color>";
        }
        else if(type == 1)
        {
            debuffText.text = "<color=#F53232>"+ debuffText.text + "</color>";
        }
        else
        {
            debuffText.color = Color.white;
        }

        debuff.SetActive(true);
        ObjectInvisible.Add(debuff);
    }
    public void RemoveDebuffText()
    {       
        ObjectInvisible.Remove(debuff);
        debuff.SetActive(false);
    }
    public void MissHit()
    {
        photonView.RPC("MissHitCmd", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void MissHitCmd()
    {
        StartCoroutine(HitMiss());
    }
    IEnumerator HitMiss()
    {
        AddDebuffText("MISS",2);
        yield return new WaitForSeconds(3f);
        RemoveDebuffText();
    }
    [PunRPC]
    public void StunPlayer(float time)
    {
        StartCoroutine(StunTime(time));
    }
    public IEnumerator StunTime(float time)
    {
        AddDebuffText("STUN",1);
        foreach (AnimatorControllerParameter item in pAnimator.parameters)
        {
            if (item.name != "Death")
            {
                pAnimator.SetBool(item.name, false);
            }
        }
        isbeStun = true;
        stunPT.SetActive(true);
        ObjectInvisible.Add(stunPT);
        pAnimator.SetBool("Stun", true);
        speedStun = 0;
        yield return new WaitForSeconds(time);
        RemoveDebuffText();
        pAnimator.SetBool("Stun", false);             
        ObjectInvisible.Remove(stunPT);
        stunPT.SetActive(false);
        if(DoorGame.door.isWatching == false)
        {
            isbeStun = false;
            speedStun = 1;
        }
    }
    [PunRPC]
    public void FearPlayer(float time)
    {
        StartCoroutine(FearTime(time));
    }
    public IEnumerator FearTime(float time)
    {
        AddDebuffText("FEAR",1);
        isFear = true;
        //if (this == gameManager.mainPlayer)
        //{
        //    cam.GetComponent<CameraFilterPack_FX_Drunk>().enabled = true;
        //}
        //cam.GetComponent<Animator>().SetBool("isFear", true);
        yield return new WaitForSeconds(time);
        isFear = false;
        RemoveDebuffText();
        //if (this == gameManager.mainPlayer)
        //{
        //    cam.GetComponent<CameraFilterPack_FX_Drunk>().enabled = false;
        //}
        if (!isBot)
        {
            cam.GetComponent<Animator>().SetBool("isFear", false);
        }        
        photonView.RPC("RemoveFearEffect", RpcTarget.AllBuffered);
    }
    public IEnumerator FastTime(float time, PlayerSetup p, float _speedFast)
    {
        p.AddDebuffText("SPEED UP",0);     
        isSlow = true;
        p.speedUp += _speedFast;
        p.wizardSpeedUp.SetActive(true);
        p.ObjectInvisible.Add(p.wizardSpeedUp);
        yield return new WaitForSeconds(time);
        p.RemoveDebuffText();
        isSlow = false;
        p.speedUp -= _speedFast;
        p.ObjectInvisible.Remove(p.wizardSpeedUp);
        p.wizardSpeedUp.SetActive(false);
    }
    GameObject feardebuff;
    [PunRPC]
    void AddFearEffectToObjectInvisible()
    {
        foreach (GameObject f in gameManager.effectRaven)
        {
            if (f.GetComponent<FearEffect>().owner == this)
            {
                if (!ObjectInvisible.Contains(f))
                {
                    ObjectInvisible.Add(f);
                    feardebuff = f;
                }
            }
        }
    }
    [PunRPC]
    void RemoveFearEffect()
    {
        foreach (GameObject f in gameManager.effectRaven)
        {
            if (f.GetComponent<FearEffect>().owner == this)
            {
                if (ObjectInvisible.Contains(feardebuff))
                {
                    ObjectInvisible.Remove(feardebuff);
                }
                if (feardebuff.activeSelf)
                {
                    feardebuff.SetActive(false);
                }
            }
        }
        
    }
    public void ReciveBoxEffect(int idBuff)
    {
        if (photonView.IsMine)
        {
            string roleName = "";
            if(player == GameManager.HideOrSeek.Hide)
            {
                roleName = "HIDER";
            }
            else
            {
                roleName = "SEEKER";
            }
            photonView.RPC("ReciveBoxEffectCmd", RpcTarget.AllBuffered, idBuff,roleName);
        }
    }

    public void ReciveBoxDebuff(int idBuff)
    {
        if (photonView.IsMine)
        {
            string roleName = "";
            if (player == GameManager.HideOrSeek.Hide)
            {
                roleName = "HIDER";
            }
            else
            {
                roleName = "SEEKER";
            }
            photonView.RPC("ReciveBoxDebuffCmd", RpcTarget.AllBuffered, idBuff, roleName);
        }
    }
    [PunRPC]
    public void ReciveBoxEffectCmd(int idBuff, string roleName)
    {
        switch (idBuff)
        {
            case 0:
                //Speed up 1 player
                StartCoroutine(FastTime(5, this, 1));
                break;
            case 1:
                //Speed up all player
                foreach (PlayerSetup i in gameManager.players)
                {
                    if (i.dead == false && i.player == player)
                    {
                        i.StartCoroutine(FastBuff(5, i, 1));
                    }
                }
                break;
            case 2:
                if(player == GameManager.HideOrSeek.Seek)
                {
                    StartCoroutine(Detected(0, this));
                }
                else
                {
                    StartCoroutine(Detected(1, this));
                }
                break;
            default:
                //Slow all players in team
                StartCoroutine(FastTime(5, this, 1));
                break;
        }

    }
    [PunRPC]
    public void ReciveBoxDebuffCmd(int idBuff, string roleName)
    {
        switch (idBuff)
        {
            case 0:
                //Slow 1 player
                StartCoroutine(SlowTimeAfterRun(2, this, slowEffectAffterRunningObj, currentSpeed * 0.4f));
                break;
            case 1:
                //Stun 1 player
                StartCoroutine(StunTime(1.5f));
                break;
            //Debuff player team
            case 2:
                //Slow player team
                foreach (PlayerSetup i in gameManager.players)
                {
                    if (i.dead == false && i.player == player)
                    {
                        i.StartCoroutine(SlowBuff(5, i, slowEffectAffterRunningObj, 1, roleName));
                    }
                }
                break;
            case 3:
                //Slow player team
                foreach (PlayerSetup i in gameManager.players)
                {
                    if (i.dead == false && i.player != player)
                    {
                        i.StartCoroutine(SlowBuff(5, i, slowEffectAffterRunningObj, 1, roleName));
                    }
                }
                break;
            case 4:
                slowBomb.Spawn();
                break;
            default:
                //Slow all players in team
                foreach (PlayerSetup i in gameManager.players)
                {
                    if (i.dead == false && i.player != player)
                    {
                        i.StartCoroutine(SlowBuff(5, i, slowEffectAffterRunningObj, 1, roleName));
                    }
                }
                break;
        }

    }
    public IEnumerator FastZoneTime(float time, PlayerSetup p, float _speedFast)
    {
        isSlow = true;
        p.speedUpZone += _speedFast;
        p.wizardSpeedUp.SetActive(true);
        yield return new WaitForSeconds(time);
        isSlow = false;
        p.speedUpZone -= _speedFast;
        p.wizardSpeedUp.SetActive(false);
    }
    public IEnumerator SlowBuff(float time, PlayerSetup p, GameObject slowEffect, float _speedSlow, string roleName)
    {
        p.AddDebuffText("SLOW BY "+roleName,1);
        isTired = true;
        foreach (PlayerSetup item in gameManager.players)
        {
            if (item.player == GameManager.HideOrSeek.Hide && (p.inGrass))
            {
            }
            else if (item.player == GameManager.HideOrSeek.Seek)
            {
                slowEffect.SetActive(true);
                slowEffect.transform.position = p.transform.position;
                slowEffect.transform.SetParent(p.transform);
            }
        }
        if (p.photonView.IsMine && !p.isBot)
        {
            p.pAnimator.SetBool("Tired", true);
        }
        p.speedDown += _speedSlow;
        yield return new WaitForSeconds(time);
        p.RemoveDebuffText();
        isTired = false;
        p.speedDown -= _speedSlow;
        if (p.photonView.IsMine && !p.isBot)
        {
            p.pAnimator.SetBool("Tired", false);
        }
        if (slowEffect.activeSelf)
        {
            slowEffect.SetActive(false);
        }
    }
    public IEnumerator FearBuff(float time, string roleName)
    {
        AddDebuffText("FEAR BY "+roleName,1);
        isFear = true;
        //if (this == gameManager.mainPlayer)
        //{
        //    cam.GetComponent<CameraFilterPack_FX_Drunk>().enabled = true;
        //}
        yield return new WaitForSeconds(time);
        isFear = false;
        RemoveDebuffText();
        //if (this == gameManager.mainPlayer)
        //{
        //    cam.GetComponent<CameraFilterPack_FX_Drunk>().enabled = false;
        //}
        if (!isBot)
        {
            cam.GetComponent<Animator>().SetBool("isFear", false);
        }
        //photonView.RPC("RemoveFearEffect", RpcTarget.AllBuffered);
    }
    public IEnumerator FearBuff2(float time)
    {
        AddDebuffText("FEAR",1);
        isFear = true;
        //if (this == gameManager.mainPlayer)
        //{
        //    cam.GetComponent<CameraFilterPack_FX_Drunk>().enabled = true;
        //}
        yield return new WaitForSeconds(time);
        isFear = false;
        RemoveDebuffText();
        //if (this == gameManager.mainPlayer)
        //{
        //    cam.GetComponent<CameraFilterPack_FX_Drunk>().enabled = false;
        //}
        if (!isBot)
        {
            cam.GetComponent<Animator>().SetBool("isFear", false);
        }
        //photonView.RPC("RemoveFearEffect", RpcTarget.AllBuffered);
    }
    public IEnumerator FastBuff(float time, PlayerSetup p, float _speedFast)
    {
        p.AddDebuffText("SPEED UP ALL TEAM",0);
        isSlow = true;
        p.speedUp += _speedFast;
        p.wizardSpeedUp.SetActive(true);
        p.ObjectInvisible.Add(p.wizardSpeedUp);
        yield return new WaitForSeconds(time);
        p.RemoveDebuffText();
        isSlow = false;
        p.speedUp -= _speedFast;
        p.ObjectInvisible.Remove(p.wizardSpeedUp);
        p.wizardSpeedUp.SetActive(false);
    }
    public IEnumerator Detected(int id, PlayerSetup pl)
    {        
        if(gameManager.mainPlayer == pl)
        {
            pl.AddDebuffText("ENEMY DETECTED",0);
            if (id == 0)
            {
                foreach (PlayerSetup p in gameManager.players)
                {
                    if (p.player == GameManager.HideOrSeek.Hide && !p.dead &&
                        gameManager.mainPlayer.player == GameManager.HideOrSeek.Seek)
                    {
                        foreach (Ping ping in gameManager.pings)
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
                foreach (PlayerSetup p in gameManager.players)
                {
                    if (p.player == GameManager.HideOrSeek.Seek && !p.dead &&
                        gameManager.mainPlayer.player == GameManager.HideOrSeek.Hide)
                    {
                        foreach (Ping ping in gameManager.pings)
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
            RemoveDebuffText();
            foreach (Ping p in gameManager.pings)
            {
                if (p.gameObject.activeSelf)
                {
                    p.owner.target.enabled = false;
                    p.gameObject.SetActive(false);
                }
            }
        }         
    }
    IEnumerator reviveAlertTime()
    {
        target.enabled = true;
        yield return new WaitForSeconds(0.5f);
        target.enabled = false;
    }
    public void reviveAlert()
    {
        if(GameManager.instance.mainPlayer.player == GameManager.HideOrSeek.Seek)
        {
            StartCoroutine(reviveAlertTime());
        }        
    }
}
