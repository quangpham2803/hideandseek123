using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ReviveCircle : MonoBehaviourPun
{
    public PlayerSetup owner;
    public GameObject timeUI;
    public Text timeUIText;
    public float reviveTime;
    float currentTime = 0;
    public bool isReviving;
    public PlayerSetup currnetRevivePlayer;
    public float timeDisappear;
    public float currentTimeDisappear = 0;

    //Button UI
    public float speedShowAndHide;
    public CameraFollow cam;
    public GameManager.HideOrSeek role;
    public bool isShowing;

    public GameObject healEffect;
    public bool isStayIn;
    public GameObject bloodExplo;
    private void OnEnable()
    {       
        if (photonView.IsMine && !owner.isBot)
        {            
            cam.CamNearDead();
        }
        timeUI.SetActive(true);
        isReviving = false;
        isShowing = false;
        dead = false;
        healEffect.SetActive(false);
        currentTimeDisappear = 0;
        bloodExplo.SetActive(false);
        owner.transform.Find("Other").gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        currentTime = 0;
        isReviving = false;
        if(currnetRevivePlayer != null)
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
        if(owner == GameManager.instance.mainPlayer)
        {
            cam.TurnBackTo(owner);
            if(owner.dead)
            {
                cam.UI.SetActive(true);
            }          
        }
        owner.reviveAlert();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") 
            && other.GetComponent<PlayerSetup>().player == role 
            && other.GetComponent<PlayerSetup>().dead == false 
            && isReviving == false 
            && isShowing == false)
        {
            currnetRevivePlayer = other.GetComponent<PlayerSetup>();
            if (currnetRevivePlayer != null && !currnetRevivePlayer.isBot)
            {
                photonView.RPC("UIActive", RpcTarget.AllBuffered, currnetRevivePlayer.GetComponent<PhotonView>().ViewID);
            }          
            else if (currnetRevivePlayer != null && currnetRevivePlayer.isBot && currnetRevivePlayer.GetComponent<Player>().caseBot == Player.CaseBot.SaveHider)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("UIActive", RpcTarget.AllBuffered, currnetRevivePlayer.GetComponent<PhotonView>().ViewID);
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")
            && other.GetComponent<PlayerSetup>().player == role
            && other.GetComponent<PlayerSetup>().dead == false
            && isReviving == false 
            && isShowing == false)
        {

            currnetRevivePlayer = other.GetComponent<PlayerSetup>();
            if (currnetRevivePlayer != null && !currnetRevivePlayer.isBot)
            {
                photonView.RPC("UIActive", RpcTarget.AllBuffered, currnetRevivePlayer.GetComponent<PhotonView>().ViewID);
            }
            else if (currnetRevivePlayer != null && currnetRevivePlayer.isBot && currnetRevivePlayer.GetComponent<Player>().caseBot == Player.CaseBot.SaveHider)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("UIActive", RpcTarget.AllBuffered, currnetRevivePlayer.GetComponent<PhotonView>().ViewID);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isReviving == false && currnetRevivePlayer != null && other.GetComponent<PlayerSetup>() == currnetRevivePlayer && !currnetRevivePlayer.isBot)
        {
            photonView.RPC("UIDisactive", RpcTarget.AllBuffered);
        }
        else if (other.CompareTag("Player") && isReviving == false && currnetRevivePlayer != null && other.GetComponent<PlayerSetup>() == currnetRevivePlayer && currnetRevivePlayer.isBot)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("UIDisactive", RpcTarget.AllBuffered);
            }
        }
    }
    public Image fillBar;
    private bool dead;
    private void Update()
    {
        if (owner.dead == false)
        {
            photonView.RPC("TurnOffReviveCircle", RpcTarget.AllBuffered);
        }
        if (currentTimeDisappear < timeDisappear && owner.dead)
        {
            currentTimeDisappear += Time.deltaTime;
            string seconds = (timeDisappear - currentTimeDisappear % 60).ToString("00");
            timeUIText.text = seconds;
        }
        else if (owner.dead)
        {
            if (owner.photonView.IsMine)
            {
                if (photonView.IsMine && !owner.isBot)
                {
                    GameManager.instance.waypointCanavas.SetActive(false);
                    GameManager.instance.joystick.gameObject.SetActive(false);
                    GameManager.instance.seekNear.SetActive(false);
                }
                photonView.RPC("DisableRevive", RpcTarget.AllBuffered);
            }
        }
        if (isShowing == true && isReviving == false)
        {
            photonView.RPC("Use", RpcTarget.AllBuffered);
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
                    if (owner.photonView.IsMine)
                    {
                        if (!owner.isBot)
                        {
                            cam.UI.SetActive(false);
                        }
                        photonView.RPC("ReviveSuccess", RpcTarget.AllBuffered, currnetRevivePlayer.name.text);
                    }
                }
            }
            else if (currnetRevivePlayer.dead || currnetRevivePlayer.isMoving && !currnetRevivePlayer.isBot)
            {
                photonView.RPC("ReviveFail", RpcTarget.AllBuffered);
            }
            else if (currnetRevivePlayer.isBot && currnetRevivePlayer.inRange.Count != 0)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("ReviveFail", RpcTarget.AllBuffered);
                }
            }
            else if (currnetRevivePlayer.isBot && currnetRevivePlayer.inRange.Count == 0)
            {
                currnetRevivePlayer.GetComponent<Player>().enabled = false;
                currnetRevivePlayer.agent.enabled = false;
                if (currentTime < reviveTime)
                {
                    currentTime += Time.deltaTime;
                    fillBar.fillAmount = (reviveTime - currentTime) / reviveTime;
                }
                else
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        photonView.RPC("ReviveSuccess", RpcTarget.AllBuffered, currnetRevivePlayer.name.text);
                    }
                }
            }
        }
    }

    [PunRPC]
    void UIActive(int id)
    {
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            if (p.photonView.ViewID == id)
            {
                currnetRevivePlayer = p;
                break;
            }
        }
        isShowing = true;
    }
    [PunRPC]
    void UIDisactive()
    {
        if(currnetRevivePlayer != null)
        {
            //if (currnetRevivePlayer.actionBtn != null)
            //{
            //    currnetRevivePlayer.actionBtn.GetComponent<Animator>().SetBool("isAction", false);              
            //}
            if (currnetRevivePlayer.isBot && PhotonNetwork.IsMasterClient)
            {
                currnetRevivePlayer.GetComponent<Player>().enabled = true;
                currnetRevivePlayer.agent.enabled = true;
                currnetRevivePlayer.isReviving = false;
                currnetRevivePlayer.GetComponent<Player>().caseBot = Player.CaseBot.GrassNearst;
                currnetRevivePlayer.GetComponent<Player>().CaseHiderBot();
            }
            currnetRevivePlayer.fixObject.SetActive(false);
            currnetRevivePlayer.pAnimator.SetBool("Save", false);
            currnetRevivePlayer = null;
            isShowing = false;
        }
    }
    [PunRPC]
    void ReviveFail()
    {
        isReviving = false;

        if (currnetRevivePlayer != null)
        {
            if (currnetRevivePlayer.isBot && PhotonNetwork.IsMasterClient)
            {
                currnetRevivePlayer.GetComponent<Player>().enabled = true;
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
        if (photonView.IsMine && !owner.isBot)
        {
            cam.CamNearDead();
        }
        healEffect.SetActive(false);
    }
    public void ReviveClick()
    {
        if (isReviving == false)
        {          
            photonView.RPC("Use", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    void Use()
    {
        currentTime = 0;
        isReviving = true;
        owner.needHelp = false;
        currnetRevivePlayer.fixObject.SetActive(true);
        fillBar = currnetRevivePlayer.fillBar;
        fillBar.fillAmount = 1;
        currnetRevivePlayer.pAnimator.SetBool("Save", true);
        if (currnetRevivePlayer.isBot && PhotonNetwork.IsMasterClient)
        {
            currnetRevivePlayer.GetComponent<Player>().enabled = false;
            currnetRevivePlayer.agent.enabled = false;
            currnetRevivePlayer.isReviving = true;
        }
        if (cam != null && !owner.isBot)
        {
            cam.TurnBackTo(owner);
        }
        healEffect.SetActive(true);
    }
    [PunRPC]
    void DisableRevive()
    {
        if (photonView.IsMine && !owner.isBot)
        {
            cam.CamDead();
        }
        if (currnetRevivePlayer!=null && currnetRevivePlayer.isBot && PhotonNetwork.IsMasterClient)
        {
            currnetRevivePlayer.GetComponent<Player>().enabled = true;
            currnetRevivePlayer.agent.enabled = true;
            currnetRevivePlayer.isReviving = false;
            currnetRevivePlayer.GetComponent<Player>().caseBot = Player.CaseBot.GrassNearst;
            currnetRevivePlayer.GetComponent<Player>().CaseHiderBot();
        }
        owner.needHelp = false;
        GameManager.instance.playerList.Remove(owner);
        GameManager.instance.SetHideDie(owner.name.text, GameManager.instance.itemDead);
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
        bloodExplo.SetActive(true);
        owner.model.SetActive(false);
        if (currnetRevivePlayer != null)
        {
            currnetRevivePlayer.fixObject.SetActive(false);       
        }
        healEffect.SetActive(false);
        owner.deadComplete = true;
        owner.transform.Find("Other").gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
    [PunRPC]
    void ReviveSuccess(string playername)
    {
        if (owner.isBot)
        {
            owner.GetComponent<Player>().enabled = true;
            owner.agent.enabled = true;
            owner.GetComponent<Player>().caseBot = Player.CaseBot.GrassNearst;
            owner.GetComponent<Player>().CaseHiderBot();
            foreach (PlayerSetup p in GameManager.instance.players)
            {
                if (p.player == GameManager.HideOrSeek.Hide && !p.dead)
                {
                    Physics.IgnoreCollision(p.collider, owner.collider, false);
                }
            }
        }
        else
        {
            owner.GetComponent<Player>().rigidbody.constraints = RigidbodyConstraints.None;
            owner.GetComponent<Player>().rigidbody.freezeRotation = true;
        }
        resetRevive = true;
        isReviving = false;
        owner.ApearAfterRevive();
        owner.speedDie = 1f;
        if (owner.character == GameManager.Character.Wizard)
        {
            owner.model.GetComponent<PlayerStat>().mainBody.SetActive(true);
            owner.model.GetComponent<PlayerStat>().redBody.SetActive(false);
        }
        GameManager.instance.itemHelp.SetActive(true);
        GameManager.instance.SetHideReviveHide(owner.name.text, playername, GameManager.instance.itemHelp);
        GameManager.instance.hideNumberText.text = GameManager.instance.numberHideCurrent.ToString();
        GameManager.instance.hideNumberChangeText.text = "+1";
        owner.StartCoroutine(owner.IEButton(GameManager.instance.hideNumberChangeText.gameObject, 2f));
        Utilities.FxButtonPress(GameManager.instance.hideNumberChangeText.transform, false);
        Utilities.FxButtonPress(GameManager.instance.hideNumberText.transform, false);
        GameManager.instance.isPlayerRevive = true;
        if (fillBar != null)
        {
            currnetRevivePlayer.fixObject.SetActive(false);
            currnetRevivePlayer.pAnimator.SetBool("Save", false);
            if (currnetRevivePlayer.isBot && PhotonNetwork.IsMasterClient)
            {
                currnetRevivePlayer.GetComponent<Player>().enabled = true;
                currnetRevivePlayer.agent.enabled = true;
                currnetRevivePlayer.isReviving = false;
                currnetRevivePlayer.GetComponent<Player>().caseBot = Player.CaseBot.GrassNearst;
                currnetRevivePlayer.GetComponent<Player>().CaseHiderBot();
            }
            fillBar = null;            
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


    [PunRPC]
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
