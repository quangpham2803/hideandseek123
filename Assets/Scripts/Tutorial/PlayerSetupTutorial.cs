using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerSetupTutorial : MonoBehaviour
{
    //Only player
    public GameManagerTutorial gameManager;
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

    public GameManagerTutorial.HideOrSeek player;
    public GameManagerTutorial.HideOrSeek playerTemp;
    public int roleNumber;
    public int actor;
    public float currentSpeed;

    public float activationTime;
    public bool invisible;
    public float runTime;
    public float scanTime;
    public bool run;

    public Target target;
    public bool inGrass = false;
    public Transform cameraPrefab;
    public CameraFollowTutorial cam;
    public TMPro.TextMeshProUGUI name;
    public bool changePlayerInGrass;
    public GameObject grass;
    public bool defaultMater;
    public Animator pAnimator;

    public GameObject invisibleObject;
    //Bot
    public List<PlayerSetupTutorial> inRange = new List<PlayerSetupTutorial>();
    public List<PlayerSetupTutorial> listPlayerRevive = new List<PlayerSetupTutorial>();
    public float range;
    public List<PlayerSetupTutorial> inRangeOfBot = new List<PlayerSetupTutorial>();
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

    public List<GameObject> skill = new List<GameObject>();
    public List<GameObject> skillTemp = new List<GameObject>();
    public float defaultSpeed;
    public List<Material> defaultMaterial;
    public List<Material> defaultMaterialSeek;


    public JoystickComponentCtrl joystick;
    public NavMeshAgent agentPlayer;
    public CapsuleCollider collider;

    public PlayerRPCTutorial rpc;

    public bool coverGrass;
    public bool outGrass;
    public List<GameObject> grassList = new List<GameObject>();

    public float speedDown;
    public float speedStun = 1;
    public float speedHook = 1;
    public float speedDie = 1;
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

    public List<PlayerSetupTutorial> playerInRange = new List<PlayerSetupTutorial>();
    public List<PlayerSetupTutorial> playerFriend = new List<PlayerSetupTutorial>();
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
    public ReviveCircleTutorial reviveCircle;
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
    public TrashSlideTutorial slide;
    public FootPrintSpawnerTutotial footPrinter;
    public float speedSlide;
    public float speedDash = 1;
    public GameObject stunPT;
    public Target killedTarget;
    public bool isZone;
    public IEnumerator IEButton(GameObject buttonWarning, float time)
    {
        yield return new WaitForSeconds(time);
        buttonWarning.SetActive(false);
    }
    bool t;
    bool k;
    bool deadTemp;
    public Vector3 playerKill;
    IEnumerator activeArrowMain()
    {
        arrowMain.SetActive(true);
        yield return new WaitForSeconds(4f);
        arrowMain.SetActive(false);
    }
    void DisapearAfterDead()
    {
        deadBool = true;
        foreach (GameObject item in objectDisableAfterDead)
        {
            item.SetActive(false);
        }
        rpc.enabled = false;
        //characterController.enabled = false;
        if (gameManager.mainPlayer.player == GameManagerTutorial.HideOrSeek.Hide)
        {
            deadArrow.SetActive(true);
        }
        reviveCircle.gameObject.SetActive(true);
        reviveCircle.role = player;
    }
    private void Update()
    {
        if (gameManager.scene != GameManagerTutorial.isScene.Scene3 || gameManager.state != GameManagerTutorial.GameState.Playing)
        {
            return;
        }
        if (dead && !deadBool)
        {
            if (pAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death") && pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.9f)
            {
                if (!isBot)
                {
                    reviveCircle.cam = cam;
                }
                DisapearAfterDead();
            }
        }
        if (!isBot)
        {
            //if (gameManager.state == GameManagerTutorial.GameState.Starting && k)
            //{
            //    Debug.Log("asdasdadadad2");
            //    k = false;
            //    for (int i = 0; i < gameManager.players.Count; i++)
            //    {
            //        GameObject pingObj = Instantiate(gameManager.pingObject, Vector3.zero, gameManager.pingObject.transform.rotation);
            //        pingObj.GetComponent<PingTutorial>().owner = gameManager.players[i];
            //        gameManager.pings.Add(pingObj.GetComponent<PingTutorial>());
            //        pingObj.transform.position = pingObj.GetComponent<PingTutorial>().owner.transform.position;
            //        pingObj.transform.SetParent(pingObj.GetComponent<PingTutorial>().owner.transform);
            //        pingObj.SetActive(false);
            //    }
            //}

            if (gameManager.state == GameManagerTutorial.GameState.Playing && !k)
            {
                for (int i = 0; i < gameManager.players.Count; i++)
                {
                    Debug.Log("co");
                    GameObject pingObj = Instantiate(gameManager.pingObject, Vector3.zero, gameManager.pingObject.transform.rotation);
                    pingObj.GetComponent<PingTutorial>().owner = gameManager.players[i];
                    gameManager.pings.Add(pingObj.GetComponent<PingTutorial>());
                    pingObj.transform.position = pingObj.GetComponent<PingTutorial>().owner.transform.position;
                    pingObj.transform.SetParent(pingObj.GetComponent<PingTutorial>().owner.transform);
                    pingObj.SetActive(false);
                }
                k = true;
                if (!isBot)
                {
                    StartCoroutine(activeArrowMain());
                }
                gameManager.textSeekerAppear.gameObject.SetActive(true);
                gameManager.timeSeekerAppear.gameObject.SetActive(true);
            }
            if (gameManager.state == GameManagerTutorial.GameState.Playing && gameManager.changeModelSeeker)
            {
                if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step3)
                {
                    GameManagerTutorial.instance.tutorial = GameManagerTutorial.SceneTutorial1.Step4;
                }
                Debug.Log(gameManager.numberSeekCurrent + "&&" + gameManager.numberHideCurrent);
                gameManager.seekNumberText.text = gameManager.numberSeekCurrent.ToString();
                gameManager.hideNumberText.text = gameManager.numberHideCurrent.ToString();
                gameManager.changeModelSeeker = false;
                if (player == GameManagerTutorial.HideOrSeek.Seek)
                {
                    cam.cinemachine.m_Lens.OrthographicSize = 6;
                }
                else
                {
                    //model.GetComponent<PlayerStat>().redBody.transform.parent = null;
                    //model.GetComponent<PlayerStat>().redBody.transform.parent = this.transform;
                }

                //gameManager.itemDoorAndButton.GetComponent<Animator>().SetTrigger("Misson");
                foreach (PlayerSetupTutorial p in gameManager.players)
                {
                    if (p.playerTemp == GameManagerTutorial.HideOrSeek.Seek)
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
                        //p.seekerName.SetActive(true);
                    }
                    if (p.playerTemp == player && player == GameManagerTutorial.HideOrSeek.Seek)
                    {
                        
                        p.name.color = seekColorTeam;
                        p.circle.color = seekColorTeam;
                        //p.outLineName.effectColor = outlineSeek;
                    }
                    else if (p.playerTemp != player && player == GameManagerTutorial.HideOrSeek.Hide)
                    {
                        p.name.color = seekColor;
                        p.circle.color = seekColor;
                        p.name.text = "SEEKER";
                        //p.outLineName.effectColor = outlineSeek;
                    }
                    else if (p.playerTemp != player && player == GameManagerTutorial.HideOrSeek.Seek)
                    {
                        p.name.color = hideColor;
                        p.circle.color = hideColor;
                        //p.outLineName.effectColor = outlineHide;
                    }
                    if (p.playerTemp != playerTemp)
                    {
                        foreach (Image item in p.messageItem)
                        {
                            item.gameObject.SetActive(false);
                        }
                    }
                    if (player == GameManagerTutorial.HideOrSeek.Seek)
                    {
                        if (p.playerTemp == GameManagerTutorial.HideOrSeek.Seek)
                        {
                            p.seekerName.SetActive(false);
                        }
                    }
                    p.pAnimator = p.model.GetComponent<Animator>();
                }
                if (this == gameManager.mainPlayer && playerTemp == GameManagerTutorial.HideOrSeek.Hide)
                {
                    checkSeekNear.SetActive(true);
                }
                else
                {
                    checkSeekNear.SetActive(false);
                }
                if (player == GameManagerTutorial.HideOrSeek.Seek)
                {
                    name.color = seekColor;
                    circle.color = seekColor;
                    //outLineName.effectColor = outlineSeek;
                }
                else
                {
                    name.color = hideColor;
                    circle.color = hideColor;
                    //outLineName.effectColor = outlineHide;
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
            if (gameManager.state == GameManagerTutorial.GameState.Playing && !k)
            {
                k = true;
                UnParentRedBody();
                if (playerTemp == GameManagerTutorial.HideOrSeek.Seek)
                {
                    ChangeCharacter(GameManagerTutorial.HideOrSeek.Hide);
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
    void UnParentRedBody()
    {
        model.GetComponent<PlayerStat>().redBody.transform.parent = null;
        model.GetComponent<PlayerStat>().redBody.transform.parent = this.transform;
    }
    IEnumerator SeekerBotAppear()
    {
        yield return new WaitForSeconds(5f);
        gameManager.mainPlayer.StartCoroutine(gameManager.mainPlayer.LookAtSeek());
        if (isBot)
        {
            agent.enabled = false;
        }
        if (playerTemp == GameManagerTutorial.HideOrSeek.Seek)
        {
            ChangeModelAnimation( true);
        }

        yield return new WaitForSeconds(1.5f);
        if (playerTemp == GameManagerTutorial.HideOrSeek.Seek)
        {
            ChangeModelAnimation(false);
            ChangeCharacter(GameManagerTutorial.HideOrSeek.Seek);
        }
        yield return new WaitForSeconds(0.5f);
        agent.enabled = true;
        GetComponent<PlayerBotHideTutorial>().isChangeCharacter = true;
        if (playerTemp == GameManagerTutorial.HideOrSeek.Seek)
        {
            pAnimator.SetBool("isRunning", true);
            defaultSpeed = seekSpeed;
            foreach (GameObject item in skill)
            {
                item.transform.localPosition = new Vector3(0, 5000f, 0);
                item.SetActive(true);
            }
        }
        StartCoroutine(GetComponent<PlayerBotHideTutorial>().DetectPlayers());
    }
    void ChangeModelAnimation(bool t)
    {
        foreach (AnimatorControllerParameter item in pAnimator.parameters)
        {
            pAnimator.SetBool(item.name, false);
        }
        pAnimator.SetBool("ChangeCharacter", t);
    }
    void ChangeCharacter(GameManagerTutorial.HideOrSeek _role)
    {
        player = _role;
        if (_role == GameManagerTutorial.HideOrSeek.Seek)
        {
            gameManager.changeModelSeeker = true;
        }
        foreach (PlayerSetupTutorial p in gameManager.players)
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
    public void Catch(string playerKillName, Vector3 _pos)
    {
        if (!isBot)
        {
            Vibrator.Vibrate(100);
            cam.StartCoroutine(cam.Shake(0.2f, 0.4f));
        }
        if (!isTranform)
        {
            foreach (PlayerSetupTutorial p in gameManager.players)
            {
                Physics.IgnoreCollision(p.collider, collider, true);
            }
            dead = true;
            //GetComponent<Player>().rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            deadTemp = true;
            if (isBot)
            {
                GetComponent<PlayerBotHideTutorial>().enabled = false;
                agent.enabled = false;
            }
            //GameObject redBody = model.GetComponent<PlayerStat>().redBody;
            //redBody.SetActive(true);
            //redBody.GetComponent<Animator>().SetBool("Death", true);
            foreach (AnimatorControllerParameter item in pAnimator.parameters)
            {
                pAnimator.SetBool(item.name, false);
            }
            pAnimator.SetBool("Death", true);
            gameManager.SetSeekKillHide(playerKillName, name.text, gameManager.itemKill);
            gameManager.hideNumberText.text = gameManager.numberHideCurrent.ToString();
            gameManager.hideNumberChangeText.text = "-1";
            DoorGameTutorial.door.CheckPlayerDie(name.text.ToString());
            gameManager.playersMatch.Remove(this);
            StartCoroutine(AfterHit(null, playerKillName));
        }
    }
    IEnumerator AfterHit(GameObject _redBody, string _playerKillName)
    {
        if (gameManager.mainPlayer.player == GameManagerTutorial.HideOrSeek.Seek)
        {
            killedTarget.enabled = true;
        }
        yield return new WaitForSeconds(0.2f);
        //GetComponent<Player>().rigidbody.velocity = Vector3.zero;
        deadTemp = false;
        speedDie = 0f;
        isMoving = false;
        dead = true;
        yield return new WaitForSeconds(1f);
        //_redBody.SetActive(false);
        //_redBody.GetComponent<Animator>().SetBool("Death", false);
        if (gameManager.mainPlayer.player == GameManagerTutorial.HideOrSeek.Seek)
        {
            killedTarget.enabled = false;
        }
        StartCoroutine(IEButton(gameManager.hideNumberChangeText.gameObject, 2f));
        Utilities.FxButtonPress(gameManager.hideNumberText.transform, false);
        Utilities.FxButtonPress(gameManager.hideNumberChangeText.transform, false);

    }
    public IEnumerator LookAtSeek()
    {
        foreach (PlayerSetupTutorial p in gameManager.players)
        {
            if (p.playerTemp == GameManagerTutorial.HideOrSeek.Seek)
            {
                gameManager.mainPlayer.cam.owner = p;
                gameManager.waypointCanavas.SetActive(false);
                gameManager.mainPlayer.cam.cinemachine.LookAt = gameManager.mainPlayer.cam.owner.transform;
                gameManager.mainPlayer.cam.cinemachine.Follow = gameManager.mainPlayer.cam.owner.transform;
                speedStun = 0;
                break;
            }
        }
        yield return new WaitForSeconds(1.5f);
        gameManager.mainPlayer.cam.owner = gameManager.mainPlayer;
        gameManager.mainPlayer.cam.cinemachine.LookAt = gameManager.mainPlayer.cam.owner.transform;
        gameManager.mainPlayer.cam.cinemachine.Follow = gameManager.mainPlayer.cam.owner.transform;
        gameManager.waypointCanavas.SetActive(true);
        speedStun = 1;
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
        //gameManager.playersMatch.Add(this);
        deadArrow.SetActive(false);
    }
    private void Start()
    {
        speedDash = 1;
        rpc.GetMaterial(this);
        rpc.GetInvisibleMaterial(this);
        rpc.GetInvisibleOtherTeamMaterial(this);
        if (playerTemp == GameManagerTutorial.HideOrSeek.Seek)
        {
            rpc.GetMaterialSeek(this);
        }
    }
    public IEnumerator StunTime(float time)
    {
        AddDebuffText("STUN", 1);
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
        isbeStun = false;
        ObjectInvisible.Remove(stunPT);
        stunPT.SetActive(false);
        speedStun = 1;
    }
    public void AddDebuffText(string debuffname, int type)
    {
        debuffText.text = debuffname;
        if (type == 0)
        {
            debuffText.text = "<color=#0AF5F5>" + debuffText.text + "</color>";
        }
        else if (type == 1)
        {
            debuffText.text = "<color=#F53232>" + debuffText.text + "</color>";
        }
        else
        {
            debuffText.color = Color.white;
        }
        debuff.SetActive(true);
        ObjectInvisible.Add(debuff);
    }
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
    public void RemoveDebuffText()
    {
        ObjectInvisible.Remove(debuff);
        debuff.SetActive(false);
    }
}
