using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveCircleTutorial : MonoBehaviour
{
    public PlayerSetupTutorial owner;
    public GameObject timeUI;
    public Text timeUIText;
    public float reviveTime;
    float currentTime = 0;
    public bool isReviving;
    public PlayerSetupTutorial currnetRevivePlayer;
    public float timeDisappear;
    public float currentTimeDisappear = 0;

    //Button UI
    public float speedShowAndHide;
    public CameraFollowTutorial cam;
    public GameManagerTutorial.HideOrSeek role;
    public bool isShowing;

    public GameObject healEffect;
    public bool isStayIn;
    public GameObject bloodExplo;
    private void OnEnable()
    {
        timeUI.SetActive(true);
        isReviving = false;
        isShowing = false;
        dead = false;
        healEffect.SetActive(false);
        currentTimeDisappear = 0;
        //bloodExplo.SetActive(false);
        owner.transform.Find("Other").gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        currentTime = 0;
        isReviving = false;
        if (currnetRevivePlayer != null)
        {
            if (currnetRevivePlayer.isBot)
            {
                currnetRevivePlayer.isReviving = false;
                //currnetRevivePlayer.GetComponent<Player>().havePlayerNeedRevive = false;
                currnetRevivePlayer.agent.enabled = true;
            }
            currnetRevivePlayer.pAnimator.SetBool("Save", false);
            currnetRevivePlayer.fixObject.SetActive(false);
            currnetRevivePlayer = null;
        }
        isShowing = false;
        owner.fixObject.SetActive(false);
        if (owner == GameManagerTutorial.instance.mainPlayer)
        {
            cam.TurnBackTo(owner);
            if (owner.dead)
            {
                cam.UI.SetActive(true);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerSetupTutorial>() != owner
            && other.GetComponent<PlayerSetupTutorial>().player == role
            && other.GetComponent<PlayerSetupTutorial>().dead == false
            && isReviving == false
            && isShowing == false)
        {
            currnetRevivePlayer = other.GetComponent<PlayerSetupTutorial>();
            if (currnetRevivePlayer != null && !currnetRevivePlayer.isBot)
            {
                UIActive(currnetRevivePlayer);
            }
            else if (currnetRevivePlayer != null && currnetRevivePlayer.isBot && currnetRevivePlayer.GetComponent<PlayerBotHideTutorial>().caseBot == PlayerBotHideTutorial.CaseBot.SaveHider)
            {
                UIActive(currnetRevivePlayer);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerSetupTutorial>() != owner
            && other.GetComponent<PlayerSetupTutorial>().player == role
            && other.GetComponent<PlayerSetupTutorial>().dead == false
            && isReviving == false 
            && isShowing == false)
        {
            currnetRevivePlayer = other.GetComponent<PlayerSetupTutorial>();
            if (currnetRevivePlayer != null && !currnetRevivePlayer.isBot)
            {
                UIActive(currnetRevivePlayer);
            }
            else if (currnetRevivePlayer != null && currnetRevivePlayer.isBot && currnetRevivePlayer.GetComponent<PlayerBotHideTutorial>().caseBot == PlayerBotHideTutorial.CaseBot.SaveHider)
            {
                UIActive(currnetRevivePlayer);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isReviving == false && currnetRevivePlayer != null && other.GetComponent<PlayerSetupTutorial>() == currnetRevivePlayer && !currnetRevivePlayer.isBot)
        {
            UIDisactive();
        }
    }
    public Image fillBar;
    private bool dead;
    private void Update()
    {
        if (owner.dead == false)
        {
            TurnOffReviveCircle();
        }
        if (GameManagerTutorial.instance.scene == GameManagerTutorial.isScene.Scene3)
        {
            if (currentTimeDisappear < timeDisappear && owner.dead)
            {
                currentTimeDisappear += Time.deltaTime;
                string seconds = (timeDisappear - currentTimeDisappear % 60).ToString("00");
                timeUIText.text = seconds;
            }
            else if (owner.dead)
            {
                if (owner == GameManagerTutorial.instance.mainPlayer)
                {
                    GameManagerTutorial.instance.waypointCanavas.SetActive(false);
                    GameManagerTutorial.instance.joystick.gameObject.SetActive(false);
                    GameManagerTutorial.instance.seekNear.SetActive(false);
                }
                DisableRevive();
            }
        }
        if (isShowing == true && isReviving == false)
        {
            Use();
        }
        if (isReviving && currnetRevivePlayer != null)
        {
            if (currnetRevivePlayer.dead == false && currnetRevivePlayer.isMoving == false && !currnetRevivePlayer.isBot)
            {
                if (currentTime < reviveTime)
                {
                    currentTime += Time.deltaTime;
                    fillBar.fillAmount = (reviveTime - currentTime) / reviveTime;
                }
                else
                {
                    ReviveSuccess(currnetRevivePlayer.name.text);
                }
            }
            else if (currnetRevivePlayer.dead || currnetRevivePlayer.isMoving && !currnetRevivePlayer.isBot)
            {
                ReviveFail();
            }
            else if (currnetRevivePlayer.isBot && currnetRevivePlayer.inRange.Count != 0)
            {
                ReviveFail();
            }
            else if (currnetRevivePlayer.isBot && currnetRevivePlayer.inRange.Count == 0)
            {
                currnetRevivePlayer.GetComponent<PlayerBotHideTutorial>().enabled = false;
                currnetRevivePlayer.agent.enabled = false;
                if (currentTime < reviveTime)
                {
                    currentTime += Time.deltaTime;
                    fillBar.fillAmount = (reviveTime - currentTime) / reviveTime;
                }
                else
                {
                    ReviveSuccess(currnetRevivePlayer.name.text);
                }
            }
        }
    }

    void UIActive(PlayerSetupTutorial p)
    {
        currnetRevivePlayer = p;
        isShowing = true;
    }
    void UIDisactive()
    {
        if (currnetRevivePlayer != null)
        {
            //if (currnetRevivePlayer.actionBtn != null)
            //{
            //    currnetRevivePlayer.actionBtn.GetComponent<Animator>().SetBool("isAction", false);              
            //}
            if (currnetRevivePlayer.isBot )
            {
                currnetRevivePlayer.GetComponent<PlayerBotHideTutorial>().enabled = true;
                currnetRevivePlayer.agent.enabled = true;
                currnetRevivePlayer.isReviving = false;
                currnetRevivePlayer.GetComponent<PlayerBotHideTutorial>().caseBot = PlayerBotHideTutorial.CaseBot.Grass;
                currnetRevivePlayer.GetComponent<PlayerBotHideTutorial>().CaseHiderBot();
            }
            currnetRevivePlayer.fixObject.SetActive(false);
            currnetRevivePlayer.pAnimator.SetBool("Save", false);
            currnetRevivePlayer = null;
            isShowing = false;
        }
    }
    void ReviveFail()
    {
        isReviving = false;

        if (currnetRevivePlayer != null)
        {
            if (currnetRevivePlayer.isBot )
            {
                currnetRevivePlayer.GetComponent<PlayerBotHideTutorial>().enabled = true;
                currnetRevivePlayer.agent.enabled = true;
            }
            currnetRevivePlayer.fixObject.SetActive(false);
            currnetRevivePlayer.pAnimator.SetBool("Save", false);
            currnetRevivePlayer.isReviving = false;
            currnetRevivePlayer = null;
            fillBar = null;
        }

        currentTime = 0;
        isShowing = false;
        //if (photonView.IsMine && !owner.isBot)
        //{
        //    cam.CamNearDead();
        //}
        healEffect.SetActive(false);
    }
    public void ReviveClick()
    {
        if (isReviving == false)
        {
            Use();
        }
    }
    void Use()
    {
        currentTime = 0;
        isReviving = true;
        owner.needHelp = false;
        currnetRevivePlayer.fixObject.SetActive(true);
        fillBar = currnetRevivePlayer.fillBar;
        fillBar.fillAmount = 1;
        currnetRevivePlayer.pAnimator.SetBool("Save", true);
        if (currnetRevivePlayer.isBot )
        {
            currnetRevivePlayer.GetComponent<PlayerBotHideTutorial>().enabled = false;
            currnetRevivePlayer.agent.enabled = false;
            currnetRevivePlayer.isReviving = true;
        }
        if (cam != null && !owner.isBot)
        {
            cam.TurnBackTo(owner);
        }
        healEffect.SetActive(true);
    }
    void DisableRevive()
    {
        if ( !owner.isBot)
        {
            cam.CamDead();
        }
        if (currnetRevivePlayer != null && currnetRevivePlayer.isBot)
        {
            currnetRevivePlayer.GetComponent<PlayerBotHideTutorial>().enabled = true;
            currnetRevivePlayer.agent.enabled = true;
            currnetRevivePlayer.isReviving = false;
            currnetRevivePlayer.GetComponent<PlayerBotHideTutorial>().caseBot = PlayerBotHideTutorial.CaseBot.Grass;
            currnetRevivePlayer.GetComponent<PlayerBotHideTutorial>().CaseHiderBot();
        }
        owner.needHelp = false;
        GameManagerTutorial.instance.playerList.Remove(owner);
        //GameManager.instance.deadNumberText.text = GameManager.instance.numberDeadCurrent.ToString();
        //GameManager.instance.deadNumberChangeText.text = "+1";
        //owner.StartCoroutine(owner.IEButton(GameManager.instance.deadNumberChangeText.gameObject, 2f));
        //Utilities.FxButtonPress(GameManager.instance.deadNumberChangeText.transform, false);
        //Utilities.FxButtonPress(GameManager.instance.deadNumberText.transform, false);
        StartCoroutine(waitToExplo(1f));
    }
    public IEnumerator waitToExplo(float time)
    {
        owner.deadArrow.SetActive(false);
        //bloodExplo.SetActive(true);
        owner.model.SetActive(false);
        if (currnetRevivePlayer != null)
        {
            currnetRevivePlayer.fixObject.SetActive(false);
        }
        GameManagerTutorial.instance.SetHideDie(owner.name.text, GameManagerTutorial.instance.itemDead);
        healEffect.SetActive(false);
        owner.deadComplete = true;
        owner.transform.Find("Other").gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
    void ReviveSuccess(string playername)
    {
        if (owner.isBot)
        {
            owner.GetComponent<PlayerBotHideTutorial>().enabled = true;
            owner.agent.enabled = true;
        }
        else
        {
            owner.GetComponent<PlayerTutorial>().rigidbody.constraints = RigidbodyConstraints.None;
            owner.GetComponent<PlayerTutorial>().rigidbody.freezeRotation = true;
        }
        resetRevive = true;
        isReviving = false;
        owner.ApearAfterRevive();
        owner.speedDie = 1f;
        GameManagerTutorial.instance.itemHelp.SetActive(true);
        GameManagerTutorial.instance.SetHideReviveHide(owner.name.text, playername, GameManagerTutorial.instance.itemHelp);
        GameManagerTutorial.instance.hideNumberText.text = GameManagerTutorial.instance.numberHideCurrent.ToString();
        GameManagerTutorial.instance.hideNumberChangeText.text = "+1";
        owner.StartCoroutine(owner.IEButton(GameManagerTutorial.instance.hideNumberChangeText.gameObject, 2f));
        Utilities.FxButtonPress(GameManagerTutorial.instance.hideNumberChangeText.transform, false);
        Utilities.FxButtonPress(GameManagerTutorial.instance.hideNumberText.transform, false);
        GameManagerTutorial.instance.isPlayerRevive = true;
        if (fillBar != null)
        {
            currnetRevivePlayer.fixObject.SetActive(false);
            currnetRevivePlayer.pAnimator.SetBool("Save", false);
            if (currnetRevivePlayer.isBot)
            {
                currnetRevivePlayer.GetComponent<PlayerBotHideTutorial>().enabled = true;
                currnetRevivePlayer.agent.enabled = true;
                currnetRevivePlayer.isReviving = false;
                currnetRevivePlayer.GetComponent<PlayerBotHideTutorial>().caseBot = PlayerBotHideTutorial.CaseBot.Grass;
                currnetRevivePlayer.GetComponent<PlayerBotHideTutorial>().CaseHiderBot();
            }
            fillBar = null;
        }
        if (GameManagerTutorial.instance.scene == GameManagerTutorial.isScene.Scene2 && GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step5)
        {
            GameManagerTutorial.instance.tutorial = GameManagerTutorial.SceneTutorial1.Step6;
        }
        healEffect.SetActive(false);
        owner.transform.Find("Other").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public bool resetRevive;
    IEnumerator IEButton(GameObject buttonWarning, float time)
    {
        yield return new WaitForSeconds(time);
        buttonWarning.GetComponent<Text>().text = "";
    }


    void TurnOffReviveCircle()
    {
        owner.ApearAfterRevive();
        if (currnetRevivePlayer != null)
        {
            currnetRevivePlayer.fixObject.SetActive(false);
            currnetRevivePlayer.pAnimator.SetBool("Save", false);
            fillBar = null;
        }
        //GameManager.instance.itemKill.SetActive(false);
        isShowing = false;
        gameObject.SetActive(false);
    }
}
