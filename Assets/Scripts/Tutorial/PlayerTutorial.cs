using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTutorial : MonoBehaviour
{
    //Movement

    public float rotationSpeed = 450f;
    public Rigidbody rigidbody;
    public PlayerSetupTutorial pSetup;
    public Vector3 inputVector;
    public float xPos, yPos;
    public LayerMask layerMask;
    public LayerMask layerWall;
    public GameObject joyStickimage;
    //private void Awake()
    //{
    //}
    void Start()
    {
        pSetup = GetComponent<PlayerSetupTutorial>();
        rigidbody = GetComponent<Rigidbody>();
        pSetup.joystick = GameManagerTutorial.instance.joystick;
        //pSetup.actionBtn = GameManagerTutorial.instance.actionBtn;
        //pSetup.checkSeekNear.SetActive(true);
    }
    public float x;
    public float y;
    public float f;
    public float f1;
    public float f2;

    //Bot Seeker
    public Transform targetPoint;
    public bool isChangeCharacter;
    private bool beforeChangeCharacter = true;
    public bool changeTarget = false;
    public bool grassHide;
    public bool add = true;
    public bool addTarget = true;
    public bool clear = true;
    public bool doorOpened;
    [Header("BotHider")]
    public Transform playerNeed;
    public PlayerSetup goRevivePlayer;
    public Transform tableMove;
    public CaseBot caseBot;
    public IsInRange isInRange;
    public enum IsInRange
    {
        NoInRange,
        InRange
    }
    public enum CaseBot
    {
        Grass,
        GrassNearst,
        GrassTable,
        Gate,
        InRange,
        SaveHider,
        Table,
        Scan,
        Item,
        HitPlayer,
        OldPos
    }
    //private bool isPhysicWall;
    private void FixedUpdate()
    {
        if (GameManagerTutorial.instance.state != GameManagerTutorial.GameState.Playing || pSetup.dead)
        {
            if (!pSetup.isMoving)
            {
                rigidbody.velocity = Vector3.zero;
            }
            return;
        }
        if (pSetup.isbeStun || pSetup.speedDash == 0)
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }
        if (!pSetup.isFear && !pSetup.isFollow && !pSetup.isSummonDarkRaven)
        {
            pSetup.horizontal = pSetup.joystick.Horizontal;
            pSetup.vertical = pSetup.joystick.Vertical;
        }
        else if (pSetup.isFear)
        {
            pSetup.horizontal = -pSetup.joystick.Horizontal;
            pSetup.vertical = -pSetup.joystick.Vertical;
        }
        else if (pSetup.isFollow || pSetup.isSummonDarkRaven || pSetup.isJumping)
        {
            pSetup.horizontal = 0;
            pSetup.vertical = 0;
        }
        float alpha = Vector2.Angle(new Vector2(pSetup.horizontal, pSetup.vertical), Vector2.right);
        if (alpha >= 90)
        {
            alpha = Mathf.Abs(90 - alpha);
            x = Mathf.Sin(Mathf.Deg2Rad * alpha);
            y = Mathf.Cos(Mathf.Deg2Rad * alpha);
        }
        else
        {
            x = Mathf.Cos(Mathf.Deg2Rad * alpha);
            y = Mathf.Sin(Mathf.Deg2Rad * alpha);
        }
        if (pSetup.horizontal == 0 && pSetup.vertical == 0 && pSetup.player == GameManagerTutorial.HideOrSeek.Hide)
        {
            x = 0;
            y = 0;
        }
        if (pSetup.horizontal == 0 && pSetup.vertical == 0)
        {
            x = 0;
            y = 0;
        }
        if (pSetup.horizontal < 0)
        {
            x = -x;
        }
        if (pSetup.vertical < 0)
        {
            y = -y;
        }
        if (pSetup.isZone)
        {
            rigidbody.velocity = Vector3.zero;
            transform.position = Vector3.MoveTowards(transform.position, GameManagerTutorial.instance.currentSafeZone.transform.position, pSetup.currentSpeed * Time.deltaTime);
            transform.LookAt(GameManagerTutorial.instance.currentSafeZone.transform.position);
            return;
        }
        if (pSetup.isbeStun)
        {
            return;
        }

        if (pSetup.isWall && (pSetup.joystick.Horizontal != 0 || pSetup.joystick.Vertical != 0))
        {
            if (wallCollision == WallCollision.down)
            {
                CheckWallDown(pSetup.currentSpeed);
            }
            else if (wallCollision == WallCollision.up)
            {
                CheckWallUp(pSetup.currentSpeed);
            }
            else if (wallCollision == WallCollision.right)
            {
                CheckWallRight(pSetup.currentSpeed);
            }
            else
            {
                CheckWallLeft(pSetup.currentSpeed);
            }
            pSetup.targetRotation = Quaternion.LookRotation(new Vector3(-pSetup.joystick.Horizontal, 0, -pSetup.joystick.Vertical));
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, pSetup.targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }
        if (pSetup.isJumping)
        {
            transform.Translate((Vector3.forward * f + Vector3.up * f1) * Time.deltaTime * f2);
        }
        if (transform.position.y != 2.5f && !pSetup.isJumping && !pSetup.isWall)
        {
            pSetup.targetRotation = Quaternion.LookRotation(inputVector);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 2.5f, transform.position.z), Time.deltaTime * 5f);
        }
        if (!pSetup.isFollow && !pSetup.isSummonDarkRaven && !pSetup.isWall)
        {
            inputVector = new Vector3(-x * pSetup.currentSpeed, 0, -y * pSetup.currentSpeed);
            rigidbody.velocity = inputVector;
        }

        if (!pSetup.isSummonDarkRaven && !pSetup.isFollow && !pSetup.isWall)
        {
            if (rigidbody.velocity.magnitude > 0.2f)
            {
                pSetup.targetRotation = Quaternion.LookRotation(inputVector);
                transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, pSetup.targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
                if (!pSetup.isTired)
                {
                    pSetup.pAnimator.SetBool("isRunning", true);
                    pSetup.isMoving = true;
                }
            }
            else
            {
                pSetup.pAnimator.SetBool("isRunning", false);
                pSetup.isMoving = false;
            }
            pSetup.currentSpeed = (pSetup.defaultSpeed + pSetup.speedUp + pSetup.speedDoorOpen - pSetup.speedDown + pSetup.speedUpZone + pSetup.speedDownZone) * pSetup.speedStun * pSetup.speedDie * pSetup.speedAttack * pSetup.speedHook * pSetup.speedSlide * pSetup.speedDash;
            if (pSetup.currentSpeed < 0 && pSetup.speedStun != 0)
            {
                pSetup.currentSpeed = 0.5f;
            }
        }
    }
    void CheckWallUp(float speed)
    {
        if (pSetup.joystick.Horizontal > 0)
        {
            rigidbody.velocity = Vector3.left * speed;
        }
        else
        {
            rigidbody.velocity = Vector3.right * speed;
        }
    }
    void CheckWallDown(float speed)
    {
        if (pSetup.joystick.Horizontal > 0)
        {
            rigidbody.velocity = Vector3.left * speed;
        }
        else
        {
            rigidbody.velocity = Vector3.right * speed;
        }
    }
    void CheckWallLeft(float speed)
    {
        if (pSetup.joystick.Vertical > 0)
        {
            rigidbody.velocity = Vector3.back * speed;
        }
        else
        {
            rigidbody.velocity = Vector3.forward * speed;
        }
    }

    void CheckWallRight(float speed)
    {
        if (pSetup.joystick.Vertical > 0)
        {
            rigidbody.velocity = Vector3.back * speed;
        }
        else
        {
            rigidbody.velocity = Vector3.forward * speed;
        }
    }
    private enum WallCollision
    {
        up,
        down,
        left,
        right
    }
    private WallCollision wallCollision;
    bool step1a;
    private void OnCollisionStay(Collision collision)
    {
        if (GameManagerTutorial.instance.state != GameManagerTutorial.GameState.Playing)
        {
            return;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("ObjectMap"))
        {
            Vector3 collisionVector = transform.position - collision.transform.position;
            if (Mathf.Abs(collisionVector.z) > Mathf.Abs(collisionVector.x))
            {
                if (collisionVector.z > 0)
                {
                    //Debug.Log("down");
                    wallCollision = WallCollision.down;
                    if (pSetup.joystick.Vertical > 0)
                    {
                        pSetup.isWall = true;
                    }
                    else
                    {
                        pSetup.isWall = false;
                    }
                }
                else
                {
                    //Debug.Log("up");
                    wallCollision = WallCollision.up;
                    if (pSetup.joystick.Vertical < 0)
                    {
                        pSetup.isWall = true;
                    }
                    else
                    {
                        pSetup.isWall = false;
                    }
                }
            }
            else
            {
                if (collisionVector.x > 0)
                {
                    //Debug.Log("left");
                    wallCollision = WallCollision.left;
                    if (pSetup.joystick.Horizontal > 0)
                    {
                        pSetup.isWall = true;
                    }
                    else
                    {
                        pSetup.isWall = false;
                    }
                }
                else
                {
                    //Debug.Log("right");
                    wallCollision = WallCollision.right;
                    if (pSetup.joystick.Horizontal < 0)
                    {
                        pSetup.isWall = true;
                    }
                    else
                    {
                        pSetup.isWall = false;
                    }
                }
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (GameManagerTutorial.instance.state != GameManagerTutorial.GameState.Playing)
        {
            return;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("ObjectMap"))
        {
            pSetup.isWall = false;
        }
    }
    private void Update()
    {
        if (pSetup.changePlayerInGrass /*&& !GameManager.instance.isInvisible*/)
        {
            pSetup.changePlayerInGrass = false;
            foreach (PlayerSetupTutorial p in GameManagerTutorial.instance.players)
            {
                if (p.player != pSetup.player && p.inGrass)
                {
                    pSetup.rpc.InvisibleOtherTeam(p.invisibleOtherTeamMaterial, p);
                }
                else if (p.player != pSetup.player && p.inGrass)
                {
                    pSetup.rpc.InvisibleOtherTeam(p.invisibleOtherTeamMaterial, p);
                }
                else if (p.player == pSetup.player && p.inGrass)
                {
                    pSetup.rpc.InvisibleTeam(p.invisibleMaterial, p);
                }
                else if (!p.invisible && !p.isTranform)
                {
                    pSetup.rpc.SetDefaultMaterial(p);
                }

            }
        }
        RaycastHit raycast;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward),Color.red);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out raycast, 1f, layerMask))
        {
            if (!pSetup.isTranform && !pSetup.rpc.isDash && !pSetup.isJumping)
            {
                Debug.Log("10");
                TableObjectTutorial table = raycast.transform.GetComponent<TableObjectTutorial>();
                float angle = 0;
                if (!table.tableHorizontal)
                {
                    angle = Vector3.Angle(transform.TransformDirection(Vector3.forward), Vector3.right);
                }
                else
                {
                    angle = Vector3.Angle(transform.TransformDirection(Vector3.forward), Vector3.forward);
                }
                //Debug.Log(Mathf.Abs(angle -90));
                if (!pSetup.isBot && Mathf.Abs(angle - 90) > 25)
                {
                    Debug.Log("nhay");
                    table.PlayerJump(pSetup);
                }
            }
        }
        //Skill 
        //Run
        //if (pSetup.player == GameManagerTutorial.HideOrSeek.Seek)
        //{
        //    if (Vector3.Distance(transform.position, target2.transform.position) < 2f && !q && GameManagerTutorial.instance.tutorial == GameManagerTutorial.Tutorial.Step4)
        //    {
        //        q = true;
        //        //GameManagerTutorial.instance.step5.SetActive(false);
        //        //GameManagerTutorial.instance.canavasTutorial.SetActive(true);
        //        GameManagerTutorial.instance.tutorialUI.GetComponent<Animator>().SetBool("Step5", false);
        //        GameManagerTutorial.instance.tutorial = GameManagerTutorial.Tutorial.Step5;
        //    }
        //    pSetup.timeToFire += Time.deltaTime;
        //    if (pSetup.attack && pSetup.timeToFire >= 1.5f)
        //    {
        //        pSetup.timeToFire = 0;
        //        pSetup.attack = false;
        //        pSetup.Attack();
        //    }
        //}
        //else
        {
            //if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.Tutorial.Step7)
            //{
            //    if (GameManagerTutorial.instance.grassTutorial.activeSelf && Vector3.Distance(transform.position, GameManagerTutorial.instance.grassTutorial.transform.position) < 12f)
            //    {
            //        GameManagerTutorial.instance.grassTutorial.SetActive(false);
            //        GameManagerTutorial.instance.tutorial = GameManagerTutorial.Tutorial.Step8;
            //    }
            //    else if (GameManagerTutorial.instance.grassTutorial2.activeSelf && Vector3.Distance(transform.position, GameManagerTutorial.instance.grassTutorial2.transform.position) < 12f)
            //    {
            //        GameManagerTutorial.instance.grassTutorial2.SetActive(false);
            //        GameManagerTutorial.instance.tutorial = GameManagerTutorial.Tutorial.Step8;
            //    }
            //}

        }

    }
    public float timeChangeGrass;
    public float timeChangeTable;
    public float timeScan;
    int step = 0;
    bool addNeed;
    bool haveClone;
    bool haveCloneTemp;
    public Vector3 targetMove;
    public GameManagerTutorial manager;
    public IEnumerator DetectPlayers()
    {
        yield return new WaitForSeconds(0.25f);
        clear = true;
        addNeed = true;
        haveClone = false;
        pSetup.inRange.Clear();
        pSetup.playerFriend.Clear();
        pSetup.listPlayerRevive.Clear();
        if (!pSetup.dead)
        {
            foreach (PlayerSetupTutorial p in manager.players)
            {
                addTarget = true;
                grassHide = false;
                if (pSetup.player == GameManagerTutorial.HideOrSeek.Hide)
                {
                    if (p != pSetup && p.player == pSetup.player && Vector3.Distance(p.transform.position, transform.position) < pSetup.range + 7f && !p.dead)
                    {
                        pSetup.playerFriend.Add(p);
                    }
                    if (p != pSetup && Vector3.Distance(p.transform.position, transform.position) < (pSetup.range + 7f) && p.player == pSetup.player && p.dead && !p.deadComplete)
                    {
                        var zonePos = ZoneTutorial.Instance.CurrentSafeZone.Position;
                        var zoneRadius = ZoneTutorial.Instance.CurrentSafeZone.Radius;
                        var dstToZone = Vector3.Distance(new Vector3(p.transform.position.x, zonePos.y, p.transform.position.z),
                                                           zonePos);
                        if (dstToZone < zoneRadius)
                        {
                            pSetup.listPlayerRevive.Add(p);
                        }
                    }
                }
                else
                {
                    if (p != pSetup && p.player == pSetup.player && Vector3.Distance(p.transform.position, transform.position) < pSetup.range)
                    {
                        pSetup.playerFriend.Add(p);
                    }
                }
                if (p != pSetup && p.player != pSetup.player && !p.invisible 
                    && Vector3.Distance(p.transform.position, transform.position) < pSetup.range)
                {
                    if (p.inGrass && !p.dead)
                    {
                        //if (pSetup.player == GameManager.HideOrSeek.Hide && isInRange == IsInRange.NoInRange)
                        //{
                        //    addTarget = false;
                        //}
                        //else
                        if (pSetup.player == GameManagerTutorial.HideOrSeek.Seek)
                        {
                            addTarget = false;
                        }
                    }
                    if (Vector3.Distance(p.transform.position, transform.position) < /*pSetup.rangeDetected + pSetup.rangeDetectedIncrease*/ 3f && !p.dead)
                    {
                        addTarget = true;
                    }
                    if (addTarget)
                    {
                        if (pSetup.player == GameManagerTutorial.HideOrSeek.Seek && !p.dead)
                        {
                            pSetup.inRange.Add(p);
                        }
                        else if (pSetup.player == GameManagerTutorial.HideOrSeek.Hide)
                        {
                            pSetup.inRange.Add(p);
                        }
                    }
                }

            }

            if (pSetup.player == GameManagerTutorial.HideOrSeek.Seek)
            {
                if (pSetup.inRange.Count != 0 && pSetup.playerFriend.Count != 0)
                {
                    foreach (PlayerSetupTutorial pF in pSetup.playerFriend)
                    {
                        foreach (PlayerSetupTutorial pRange in pSetup.inRange)
                        {
                            if (pF.inRange.Contains(pRange))
                            {
                                pSetup.inRange.Remove(pRange);
                                break;
                            }
                        }
                    }
                }
                if (pSetup.inRange.Count != 0 && isInRange != IsInRange.InRange) //TH2: mat target
                {
                    isInRange = IsInRange.InRange;
                    if (pSetup.inRange.Count != 0 && pSetup.inRange[0] != pSetup.playerBotHit)
                    {
                        targetOld = pSetup.inRange[0].transform;
                    }
                }
                else if (pSetup.inRange.Count == 0 && isInRange != IsInRange.NoInRange)
                {
                    isInRange = IsInRange.NoInRange;
                    caseBot = CaseBot.OldPos;
                    CaseSeekerBot();
                }
            }
            else if (pSetup.player == GameManagerTutorial.HideOrSeek.Hide)
            {
                if (pSetup.playerFriend.Count != 0)
                {
                    foreach (PlayerSetupTutorial pF in pSetup.playerFriend)
                    {
                        foreach (PlayerSetupTutorial pRevive in pSetup.listPlayerRevive)
                        {
                            if (pF.listPlayerRevive.Contains(pRevive))
                            {
                                pSetup.listPlayerRevive.Remove(pRevive);
                                break;
                            }
                        }
                    }
                }
                if (pSetup.inRange.Count == 0 && isInRange != IsInRange.NoInRange)
                {
                    isInRange = IsInRange.NoInRange;
                    if (caseBot != CaseBot.Grass)
                    {
                        caseBot = CaseBot.Grass;
                    }
                    CaseHiderBot();
                }
            }
        }
        StartCoroutine(DetectPlayers());
    }
    public void CaseSeekerBot()
    {
        switch (caseBot)
        {
            case CaseBot.InRange:
                {
                    CheckTargetNearest();
                    AgentMove(targetPoint);
                    break;
                }
            case CaseBot.Scan:
                {
                    CheckScanNearest();
                    AgentMove(targetPoint);
                    break;
                }
            case CaseBot.Grass:
                {
                    CheckGrassRandom();
                    AgentMove(targetPoint);
                    break;
                }
            case CaseBot.HitPlayer:
                {
                    CheckGrassNearest();
                    AgentMove(targetPoint);
                    break;
                }
            case CaseBot.Gate:
                {
                    break;
                }
            case CaseBot.Item:
                {
                    break;
                }
            case CaseBot.OldPos:
                {
                    step++;
                    //Debug.Log(pSetup.name.text + "-> Bot muc tieu cu ->Step:" + step);
                    targetPoint = targetOld;
                    AgentMove(targetPoint);
                    break;
                }
        }
    }
    public Transform targetOld;
    void SeekerBot()
    {
        if (pSetup.isBlackHole)
        {
            return;
        }
        if (isChangeCharacter)
        {
            beforeChangeCharacter = false;
            isChangeCharacter = false;
            pSetup.inRange.Clear();
            caseBot = CaseBot.Grass;
            CaseHiderBot();
        }
        if (beforeChangeCharacter)
        {
            if (!changeTarget)
            {
                changeTarget = true;
                caseBot = CaseBot.Grass;
                CaseSeekerBot();
            }
        }
        else
        {
            if (pSetup.inRange.Count != 0)
            {
                caseBot = CaseBot.InRange;
                CaseSeekerBot();
            }
            else if (!haveClone)
            {
                pSetup.useSkill2 = false;
                if (DoorGame.door.escape.opened)
                {
                    caseBot = CaseBot.Gate;
                    CaseSeekerBot();
                    return;
                }
                if (pSetup.isHitPlayer)
                {
                    step++;
                    pSetup.isHitPlayer = false;
                    caseBot = CaseBot.HitPlayer;
                    CaseSeekerBot();
                }
                if (Vector3.Distance(transform.position, targetMove) < pSetup.agent.stoppingDistance)
                {
                    step++;
                    //Debug.Log(pSetup.name.text + "-> Bot binh thuong ->Step:" + step);
                    caseBot = CaseBot.Grass;
                    CaseSeekerBot();
                }
            }
        }

    }
    void HiderBot()
    {
        if (pSetup.dead)
        {
            return;
        }
        if (isChangeCharacter)
        {
            beforeChangeCharacter = false;
            isChangeCharacter = false;
            pSetup.inRange.Clear();
            caseBot = CaseBot.GrassNearst;
            CaseHiderBot();
        }
        if (beforeChangeCharacter)
        {
            if (!changeTarget)
            {
                changeTarget = true;
                caseBot = CaseBot.Grass;
                CaseHiderBot();
            }
        }
        else
        {
            if (DoorGame.door.escape.opened && !pSetup.isReviving)
            {
                caseBot = CaseBot.Gate;
                CaseHiderBot();
                return;
            }
            if (pSetup.inRange.Count != 0 && !pSetup.isReviving)
            {
                if (!pSetup.agent.enabled)
                {
                    pSetup.agent.enabled = true;
                }
                if (isInRange == IsInRange.NoInRange)
                {
                    //Debug.Log(pSetup.name.text + "-> Bot trong tam ->Step:" + step + "CaseBot: " + caseBot);
                    caseBot = CaseBot.InRange;
                    isInRange = IsInRange.InRange;
                    CaseHiderBot();
                }
                if (Vector3.Distance(transform.position, pSetup.inRange[0].transform.position) < 5f && !pSetup.isSilent)
                {
                    pSetup.useSkill1 = true;
                }
                if (Vector3.Distance(transform.position, targetMove) < pSetup.agent.stoppingDistance)
                {
                    step++;
                    //Debug.Log(pSetup.name.text + "-> Bot co muc tieu ->Step:" + step +"CaseBot: "+ caseBot);
                    if (caseBot == CaseBot.GrassTable)
                    {
                        //Debug.Log("Vo day");
                        caseBot = CaseBot.Table;
                        CaseHiderBot();
                    }
                    else if (caseBot == CaseBot.Grass)
                    {
                        caseBot = CaseBot.InRange;
                        CaseHiderBot();
                    }
                }
            }
            else if (pSetup.inRange.Count == 0)
            {
                pSetup.useSkill1 = false;
                if (pSetup.listPlayerRevive.Count != 0)
                {
                    step++;
                    //Debug.Log(pSetup.name.text + "-> Bot cuu nguoi ->Step:" + step);
                    if (caseBot != CaseBot.SaveHider)
                    {
                        caseBot = CaseBot.SaveHider;
                        CaseHiderBot();
                    }
                }
                else if (caseBot == CaseBot.Scan)
                {
                    step++;
                    //Debug.Log(pSetup.name.text + "-> Bot scan ->Step:" + step);
                    timeScan += Time.deltaTime;
                    if (timeScan > 10f)
                    {
                        timeScan = 0f;
                        caseBot = CaseBot.Grass;
                        CaseHiderBot();
                    }
                }
                if (Vector3.Distance(transform.position, targetMove) < pSetup.agent.stoppingDistance)
                {
                    step++;
                    //Debug.Log(pSetup.name.text + "-> Bot binh thuong ->Step:" + step +"CaseBot: " + caseBot);
                    if (caseBot != CaseBot.GrassNearst)
                    {
                        caseBot = CaseBot.GrassNearst;
                        CaseHiderBot();
                    }
                }
            }

        }
    }
    public void CaseHiderBot()
    {
        switch (caseBot)
        {
            case CaseBot.Grass:
                {
                    CheckGrassRandom();
                    AgentMove(targetPoint);
                    break;
                }
            case CaseBot.InRange:
                {
                    caseBot = CaseBot.Table;
                    CaseHiderBot();
                    break;
                }
            case CaseBot.GrassNearst:
                {
                    CheckGrassNearestHide();
                    AgentMove(targetPoint);
                    break;
                }
            case CaseBot.Table:
                {
                    CheckTable();
                    AgentMove(targetPoint);
                    break;
                }
            case CaseBot.Scan:
                {
                    CheckGrassRandom();
                    AgentMove(targetPoint);
                    break;
                }

            case CaseBot.GrassTable:
                {
                    //CheckGrassNearest();
                    CheckGrassFoward();
                    AgentMove(targetPoint);
                    break;
                }
            case CaseBot.SaveHider:
                {
                    CheckPlayerNeedNearest2();
                    AgentMove(targetPoint);
                    break;
                }
            case CaseBot.Gate:
                {
                    AgentMove(DoorGame.door.escape.transform);
                    break;
                }
            case CaseBot.Item:
                {

                    break;
                }
        }
    }
    public bool buttonTarget;
    public void CheckTargetNearest()
    {
        targetPoint = pSetup.inRange[0].transform;
        float minRange = Vector3.Distance(targetPoint.position, transform.position);
        foreach (PlayerSetupTutorial p in pSetup.inRange)
        {
            if (Vector3.Distance(transform.position, p.transform.position) < minRange)
            {
                minRange = Vector3.Distance(transform.position, p.transform.position);
                targetPoint = p.transform;
            }
        }
    }
    public void AgentMove(Transform _targetPoint)
    {
        targetMove = Vector3.zero;
        targetMove = new Vector3(_targetPoint.position.x, 2.5f, _targetPoint.position.z);
        if (!pSetup.isJumping && pSetup.isbeStun && pSetup.speedStun == 0 && !pSetup.slide.isSliding && pSetup.speedHook == 0)
        {
            transform.LookAt(targetMove);
        }
        pSetup.pAnimator.SetBool("isRunning", true);
        //Debug.Log("AgentMove + Name: ->" + pSetup.name.text +"CaseBot:"+ caseBot);
        pSetup.agent.SetDestination(targetMove);
    }
    public void CheckTable()
    {
        have = false;
        targetPoint = manager.tableAI[0].transform;
        float minRange = Mathf.Infinity;
        foreach (Transform t in manager.tableAI)
        {
            var zonePos = ZoneTutorial.Instance.CurrentSafeZone.Position;
            var zoneRadius = ZoneTutorial.Instance.CurrentSafeZone.Radius;
            var dstToZone = Vector3.Distance(new Vector3(t.position.x, zonePos.y, t.position.z),
                                               zonePos);
            if (dstToZone > zoneRadius)
            {
                continue;
            }
            foreach (PlayerSetupTutorial p in pSetup.inRange)
            {
                if (Vector3.Distance(transform.position, t.transform.position) < minRange && Vector3.Angle(transform.position - p.transform.position, transform.position - t.transform.position) > 100)
                {
                    minRange = Vector3.Distance(transform.position, t.transform.position);
                    targetPoint = t.transform;
                    have = true;
                }
            }
        }
        if (!have)
        {
            caseBot = CaseBot.GrassTable;
            CaseHiderBot();
        }
    }
    bool have;
    public bool haveRevive;
    public List<PlayerSetupTutorial> listPlayerNeed = new List<PlayerSetupTutorial>();
    public void CheckPlayerNeedNearest2()
    {
        haveRevive = false;
        targetPoint = pSetup.listPlayerRevive[0].transform;
        float minRange = Mathf.Infinity;
        foreach (PlayerSetupTutorial p in pSetup.listPlayerRevive)
        {
            if (Vector3.Distance(transform.position, p.transform.position) < minRange)
            {
                minRange = Vector3.Distance(transform.position, p.transform.position);
                targetPoint = p.transform;
                haveRevive = true;
            }
        }
        if (!haveRevive)
        {
            caseBot = CaseBot.GrassTable;
            CaseHiderBot();
        }
    }
    public void CheckGrassRandom()
    {
        foreach (GameObject g in manager.grass)
        {
            int ran = Random.Range(0, manager.grass.Count);
            var zonePos = Vector3.zero;
            var zoneRadius = 0f;
            if (ZoneTutorial.Instance.CurStep == 3)
            {
                zonePos = ZoneTutorial.Instance.CurrentSafeZone.Position;
                zoneRadius = ZoneTutorial.Instance.CurrentSafeZone.Radius;
            }
            else
            {
                zonePos = ZoneTutorial.Instance.NextSafeZone.Position;
                zoneRadius = ZoneTutorial.Instance.NextSafeZone.Radius;
            }
            var dstToZone = Vector3.Distance(new Vector3(manager.grass[ran].transform.position.x, zonePos.y, manager.grass[ran].transform.position.z), zonePos);
            if (dstToZone < zoneRadius)
            {
                targetPoint = manager.grass[ran].transform;
                break;
            }
        }
    }
    public void CheckGrassFoward()
    {
        haveGrass = false;
        targetPoint = manager.grass[0].transform;
        float minRange = Mathf.Infinity;
        foreach (GameObject g in manager.grass)
        {
            var zonePos = ZoneTutorial.Instance.CurrentSafeZone.Position;
            var zoneRadius = ZoneTutorial.Instance.CurrentSafeZone.Radius;
            var dstToZone = Vector3.Distance(new Vector3(g.transform.position.x, zonePos.y, g.transform.position.z),
                                               zonePos);
            if (dstToZone > zoneRadius)
            {
                continue;
            }
            foreach (PlayerSetupTutorial p in pSetup.inRange)
            {
                if (Vector3.Distance(transform.position, g.transform.position) < minRange && Vector3.Angle(transform.position - p.transform.position, transform.position - g.transform.position) > 100)
                {
                    minRange = Vector3.Distance(transform.position, g.transform.position);
                    haveGrass = true;
                    targetPoint = g.transform;
                }
            }
        }
        if (!haveGrass)
        {
            caseBot = CaseBot.Grass;
            CaseHiderBot();
        }
    }
    bool haveGrass;
    public void CheckGrassNearest()
    {
        //int ran = Random.Range(0, manager.grassPointAI.Count);
        targetPoint = manager.grass[0].transform;
        float minRange = Mathf.Infinity;
        foreach (GameObject g in manager.grass)
        {
            var zonePos = ZoneTutorial.Instance.CurrentSafeZone.Position;
            var zoneRadius = ZoneTutorial.Instance.CurrentSafeZone.Radius;
            var dstToZone = Vector3.Distance(new Vector3(g.transform.position.x, zonePos.y, g.transform.position.z),
                                               zonePos);
            if (dstToZone > zoneRadius)
            {
                continue;
            }
            foreach (PlayerSetupTutorial p in pSetup.inRange)
            {
                if (Vector3.Distance(transform.position, g.transform.position) < minRange && Vector3.Distance(p.transform.position, g.transform.position) > (Vector3.Distance(g.transform.position, transform.position)))
                {
                    minRange = Vector3.Distance(transform.position, g.transform.position);
                    targetPoint = g.transform;
                }
            }
        }
    }
    public void CheckGrassNearestHide()
    {
        //int ran = Random.Range(0, manager.grassPointAI.Count);
        targetPoint = manager.grass[0].transform;
        float minRange = Mathf.Infinity;
        foreach (GameObject g in manager.grass)
        {
            var zonePos = ZoneTutorial.Instance.NextSafeZone.Position;
            var zoneRadius = ZoneTutorial.Instance.NextSafeZone.Radius;
            var dstToZone = Vector3.Distance(new Vector3(g.transform.position.x, zonePos.y, g.transform.position.z),
                                               zonePos);
            if (dstToZone > zoneRadius)
            {
                continue;
            }
            if (Vector3.Distance(transform.position, g.transform.position) < minRange)
            {
                minRange = Vector3.Distance(transform.position, g.transform.position);
                targetPoint = g.transform;
            }
        }
    }
    public void CheckScanNearest()
    {
        foreach (PlayerSetupTutorial p in manager.players)
        {
            if (p.player == pSetup.player || p == pSetup || p.dead)
            {
                continue;
            }
            targetPoint = p.transform;
        }
        float minRange = 0;
        if (targetPoint != null)
        {
            minRange = Vector3.Distance(targetPoint.position, transform.position);
        }
        foreach (PlayerSetupTutorial p in manager.players)
        {
            if (p.player == pSetup.player || p == pSetup || p.dead)
            {
                continue;
            }
            if (Vector3.Distance(transform.position, p.transform.position) < minRange)
            {
                minRange = Vector3.Distance(transform.position, p.transform.position);
                targetPoint = p.transform;
            }
        }
    }
}
