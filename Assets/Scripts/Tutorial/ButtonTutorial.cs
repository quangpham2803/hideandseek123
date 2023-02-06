using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTutorial : MonoBehaviour
{
    public GameObject buttonMid;
    public bool isActive, isAlert;
    public string nameButton;
    public PlayerSetupTutorial playerCheck;

    private bool firstCheck = true;

    public PlayerSetup curFixingPlayer;
    public float fixTime;
    public float curFixTime;
    public bool isFixing;
    public bool isShowing;
    //Button UI
    public bool deActive;
    public GameObject cantfixAlert;
    public float timeCantFix;
    public float curTimeCantFix;
    public bool isBroken;
    public Text timeBrokenText;
    public bool isTurnOff;
    public GameObject shockWave;
    private void Start()
    {
        isFixing = false;
        curFixTime = 0;
        isActive = false;
        isAlert = false;
        isShowing = false;
        isBroken = false;
        cantfixAlert.SetActive(false);
        curTimeCantFix = 0;
        isTurnOff = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PlayerSetupTutorial>().player == GameManagerTutorial.HideOrSeek.Hide && firstCheck && other.GetComponent<PlayerSetupTutorial>().dead == false && isBroken == false)
            {
                playerCheck = other.gameObject.GetComponent<PlayerSetupTutorial>();
                StartCoroutine(EnableScript());
                firstCheck = false;
            }
            if (other.GetComponent<PlayerSetupTutorial>().player == GameManagerTutorial.HideOrSeek.Hide && isActive == false && !DoorGameTutorial.door.escape.GetComponent<EscapeTutorial>().opening && other.GetComponent<PlayerSetupTutorial>().dead == false && isBroken == false)
            {
                Active();
                if (other.GetComponent<PlayerSetupTutorial>() == GameManagerTutorial.instance.mainPlayer)
                {
                    //Animator anim = other.GetComponent<PlayerSetupTutorial>().cam.GetComponent<Animator>();
                    StartCoroutine(EnableWave());
                }
            }
        }
    }
    IEnumerator EnableWave()
    {
        shockWave.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        shockWave.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);
        if (other.tag == "Player")
        {
            if (other.GetComponent<PlayerSetupTutorial>().player == GameManagerTutorial.HideOrSeek.Hide && firstCheck && other.GetComponent<PlayerSetupTutorial>().dead == false && isBroken == false)
            {
                playerCheck = other.gameObject.GetComponent<PlayerSetupTutorial>();
                StartCoroutine(EnableScript());
                firstCheck = false;
            }
            if (other.GetComponent<PlayerSetupTutorial>().player == GameManagerTutorial.HideOrSeek.Hide && isActive == false && !DoorGameTutorial.door.escape.GetComponent<EscapeTutorial>().opening && other.GetComponent<PlayerSetupTutorial>().dead == false && isBroken == false)
            {
                Active();
                if (other.GetComponent<PlayerSetupTutorial>() == GameManagerTutorial.instance.mainPlayer)
                {
                   // Animator anim = other.GetComponent<PlayerSetupTutorial>().cam.GetComponent<Animator>();
                    StartCoroutine(EnableWave());
                }
            }
        }
    }
    void UIActive(int id)
    {
        //foreach (PlayerSetupTutorial p in GameManagerTutorial.instance.players)
        //{
        //    if (p.photonView.ViewID == id)
        //    {
        //        curFixingPlayer = p;
        //        break;
        //    }
        //}
        if (curFixingPlayer.photonView.IsMine)
        {
            curFixingPlayer.actionBtn.GetComponent<Animator>().SetBool("isAction", true);
            curFixingPlayer.actionBtn.onClick.AddListener(FixClick);
        }
        isShowing = true;
    }
    void UIDisactive()
    {
        if (curFixingPlayer != null)
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
        if (isFixing == false && isActive == true)
        {
            Use();
        }
    }
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
        if (isFixing == true)
        {
            if (DoorGameTutorial.door.escape.GetComponent<EscapeTutorial>().opening == false)
            {
                if (curFixTime < fixTime)
                {
                    curFixTime += Time.deltaTime;
                    fillBar.fillAmount = (fixTime - curFixTime) / fixTime;
                }
                else
                {
                    DisActive();
                }
            }
            else
            {
                MissFix();
            }

        }
        if (curFixingPlayer != null && DoorGameTutorial.door.escape.GetComponent<EscapeTutorial>().opening == true)
        {
            UIDisactive();
            cantfixAlert.SetActive(true);
        }
        if (isBroken == true)
        {
            curTimeCantFix += Time.deltaTime;
            int timeTemp = (int)(timeCantFix - curTimeCantFix + 1);
            timeBrokenText.text = timeTemp.ToString();
            if (curTimeCantFix >= timeCantFix)
            {
                cantfixAlert.SetActive(false);
                isBroken = false;
                curTimeCantFix = 0;
            }
        }
        if (DoorGameTutorial.door.escape.GetComponent<EscapeTutorial>().opening == true || DoorGameTutorial.door.escape.GetComponent<EscapeTutorial>().opened == true)
        {
            GetComponent<Target>().enabled = false;
        }
        if (isTurnOff == false && GameManagerTutorial.instance.isSeeker)
        {
            GetComponent<Target>().enabled = false;
            isTurnOff = true;
        }
    }
    void DisActive()
    {
        curFixingPlayer.isbeStun = false;
        fillBar = null;
        curFixingPlayer.fixObject.SetActive(false);
        isActive = false;
        isAlert = false;
        isFixing = false;
        GameManagerTutorial.instance.button.Add(gameObject);
        buttonMid.SetActive(true);
        DoorGameTutorial.door.FixedButton();
        GetComponent<Target>().enabled = true;
        curFixingPlayer = null;
        isShowing = false;
        curTimeCantFix = 0;
        isBroken = true;
        cantfixAlert.SetActive(true);
    }
    void Active()
    {
        isActive = true;
        DoorGameTutorial.door.Check();
        buttonMid.SetActive(false);
        GameManagerTutorial.instance.button.Remove(gameObject);
        StartCoroutine(ChangeColorArrow());
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
        if (OffScreenIndicator.offScreen.mainCamera.GetComponent<CameraFollowTutorial>().owner.GetComponent<PlayerSetupTutorial>().player == GameManagerTutorial.HideOrSeek.Seek)
        {
            playerCheck.GetComponent<PlayerSetupTutorial>().target.enabled = true;
            yield return new WaitForSeconds(6f);
            playerCheck.GetComponent<PlayerSetupTutorial>().target.enabled = false;
        }
        else
        {
            yield return new WaitForSeconds(6f);
        }
    }
}
