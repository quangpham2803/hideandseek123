using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ButtonGame : MonoBehaviourPun
{
    public GameObject buttonMid;
    public bool isActive, isAlert;
    public string nameButton;
    public GameObject playerCheck;

    private bool firstCheck=true;

    public PlayerSetup curFixingPlayer;
    public float fixTime;
    public float curFixTime;
    public bool isFixing;
    public bool isShowing;
    //Button UI
    public bool deActive;
    public float timeCantFix;
    public float curTimeCantFix;
    public bool isBroken;
    public Text timeBrokenText;
    public bool isTurnOff;
    public GameObject shockWave;
    public GameObject needActiveWave;
    public int id;
    public GameObject buttonBody;
    private void Awake()
    {
        isFixing = false;
        curFixTime = 0;
        isActive = false;
        isAlert = false;
        isShowing = false;
        isBroken = false;
        curTimeCantFix = 0;
        isTurnOff = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Hide && firstCheck && other.GetComponent<PlayerSetup>().dead == false && isBroken == false)
            {
                playerCheck = other.gameObject;
                StartCoroutine(EnableScript());
                firstCheck = false;
            }
            if (other.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Hide && isActive == false && !DoorGame.door.escape.GetComponent<Escape>().opening && other.GetComponent<PlayerSetup>().dead == false && isBroken == false)
            {
                photonView.RPC("Active", RpcTarget.AllBuffered);
                StartCoroutine(EnableWave());
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);
        if (other.tag=="Player")
        {
            if (other.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Hide && firstCheck && other.GetComponent<PlayerSetup>().dead == false && isBroken == false)
            {
                playerCheck = other.gameObject;
                StartCoroutine(EnableScript());
                firstCheck = false;
            }
            GameManager.instance.button.Remove(gameObject);
            if (other.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Hide && isActive == false && !DoorGame.door.escape.GetComponent<Escape>().opening && other.GetComponent<PlayerSetup>().dead == false && isBroken == false)
            {
                photonView.RPC("Active",RpcTarget.All);
                foreach (PlayerSetup p in GameManager.instance.players)
                {
                    if (p.isBot && p.player == GameManager.HideOrSeek.Seek && p.inRange.Count == 0)
                    {
                        p.buttonMove = transform;
                        p.isbuttonActive = true;
                    }
                    else if (p.isBot)
                    {
                        p.GetComponent<Player>().changeTarget = false;
                    }
                }
                StartCoroutine(EnableWave());
            }

        }
    }
    IEnumerator EnableWave()
    {
        shockWave.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        shockWave.SetActive(false);
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player")  && isActive == true && isFixing == false && curFixingPlayer != null && other.GetComponent<PlayerSetup>() == curFixingPlayer && !other.GetComponent<PlayerSetup>().isbeStun)
    //    {
    //        photonView.RPC("UIDisactive", RpcTarget.AllBuffered);
    //    }
    //}
    [PunRPC]
    void UIActive(int id)
    {
        foreach(PlayerSetup p in GameManager.instance.players)
        {
            if(p.photonView.ViewID == id)
            {
                curFixingPlayer = p;
                break;
            }
        }
        if (curFixingPlayer.photonView.IsMine)
        {
            curFixingPlayer.actionBtn.GetComponent<Animator>().SetBool("isAction", true);
            curFixingPlayer.actionBtn.onClick.AddListener(FixClick);
        }
        isShowing = true;
    }
    [PunRPC]
    void UIDisactive()
    {
        if(curFixingPlayer != null)
        {
            if (curFixingPlayer.actionBtn != null)
            {
                curFixingPlayer.actionBtn.GetComponent<Animator>().SetBool("isAction", false);
                curFixingPlayer.actionBtn.onClick.RemoveAllListeners();
            }
            curFixingPlayer = null;
            isShowing = false;
        }
    }
    IEnumerator IEWarningButton(GameObject buttonWarning)
    {
        yield return new WaitForSeconds(3f);
        buttonWarning.SetActive(false);
    }
    [PunRPC]
    void StunPlayerSeekWhileFixing()
    {
        isActive = true;
        isAlert = true; ;
        isFixing = false;
        curFixTime = 0;
        fillBar = null;
        curFixingPlayer.fixObject.SetActive(false);
        StartCoroutine(stunPlayerSeek());
    }
    [PunRPC]
    void MissFix()
    {
        curFixingPlayer.isbeStun = false;
        isActive = true;
        isAlert = true; ;
        isFixing = false;
        curFixTime = 0;
        fillBar = null;
        curFixingPlayer.fixObject.SetActive(false);
        isShowing = false;
    }
    IEnumerator stunPlayerSeek()
    {
        curFixingPlayer.isbeStun = true;
        curFixingPlayer.stunEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        curFixingPlayer.isbeStun = false;
        curFixingPlayer.stunEffect.SetActive(false);
        isShowing = false;
    }

    public void FixClick()
    {
        if(isFixing == false && isActive == true)
        {
            photonView.RPC("Use", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    void Use()
    {
        curFixTime = 0;
        curFixingPlayer.isbeStun = true;
        curFixingPlayer.fixObject.SetActive(true);
        fillBar = curFixingPlayer.fillBar;
        fillBar.fillAmount = 0;
        isFixing = true;
        if (curFixingPlayer.photonView.IsMine)
        {
            curFixingPlayer.actionBtn.GetComponent<Animator>().SetBool("isAction", false);
            curFixingPlayer.actionBtn.onClick.RemoveAllListeners();
        }
    }
    public Image fillBar;
    private void Update()
    {
        if(isFixing == true)
        {
            if (DoorGame.door.escape.opening == false)
            {
                if (curFixTime < fixTime)
                {
                    curFixTime += Time.deltaTime;
                    fillBar.fillAmount = (fixTime - curFixTime) / fixTime;
                }
                else
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        photonView.RPC("DisActive", RpcTarget.AllBuffered);
                    }
                }
            }
            else
            {
                photonView.RPC("MissFix", RpcTarget.AllBuffered);
            }

        }
        if(curFixingPlayer != null && DoorGame.door.escape.GetComponent<Escape>().opening == true)
        {
            photonView.RPC("UIDisactive", RpcTarget.AllBuffered);
            //cantfixAlert.SetActive(true);
        }
        if(isBroken == true)
        {
            curTimeCantFix += Time.deltaTime;
            int timeTemp = (int)(timeCantFix - curTimeCantFix + 1);
            timeBrokenText.text = timeTemp.ToString();
            if(curTimeCantFix >= timeCantFix)
            {              
                //cantfixAlert.SetActive(false);
                isBroken = false;
                curTimeCantFix = 0;
            }
        }
        if(DoorGame.door.escape.opening == true || DoorGame.door.escape.opened == true)
        {
            GetComponent<Target>().enabled = false;
            needActiveWave.SetActive(false);
        }
        if(isTurnOff == false && GameManager.instance.isSeeker)
        {
            GetComponent<Target>().enabled = false;
            isTurnOff = true;
        }
    }
    [PunRPC]
    void DisActive()
    {
        curFixingPlayer.isbeStun = false;
        fillBar = null;
        curFixingPlayer.fixObject.SetActive(false);
        isActive = false;
        isAlert = false;
        isFixing = false;
        GameManager.instance.button.Add(gameObject);
        buttonMid.SetActive(true);
        GetComponent<Target>().enabled = true;
        curFixingPlayer = null;
        isShowing = false;
        curTimeCantFix = 0;
        isBroken = true;        
        //cantfixAlert.SetActive(true);
    }
    [PunRPC]
    void Active()
    {
        isActive = true;
        DoorGame.door.Check();
        buttonMid.SetActive(false);
        GameManager.instance.button.Remove(gameObject);
        StartCoroutine(ChangeColorArrow());
        SpawnButtonRandom.Spawn.SpawnRandom();
    }
    IEnumerator ChangeColorArrow()
    {
        yield return new WaitForSeconds(1.5f);
        GetComponent<Target>().enabled = false;
    }
    IEnumerator EnableScript()
    {
        if (playerCheck == null)
            yield break;
        if(OffScreenIndicator.offScreen.mainCamera.GetComponent<CameraFollow>().owner.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Seek)
        {
            playerCheck.GetComponent<PlayerSetup>().target.enabled = true;
            yield return new WaitForSeconds(6f);
            playerCheck.GetComponent<PlayerSetup>().target.enabled = false;
        }
        else
        {
            yield return new WaitForSeconds(6f);
        }
    }
}
