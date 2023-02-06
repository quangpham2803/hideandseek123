using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransformSkilUI : MonoBehaviour
{
    public Image countdownUI;
    //[SerializeField] Image iconUIFake;
    //[SerializeField] Image iconUITranfrom;
    //[SerializeField] Sprite[] listIcon;
    //[SerializeField] TMPro.TextMeshProUGUI currentTimeTransformText;
    public TransformSkill skill;
    private float time;
    //public TransformSkillData data;
    //public Text consume;
    PlayerSetup player;
    public Image imagePressFake;
    public Image imagePressTranform;
    public TextMeshProUGUI countDownText;
    public GameObject backgroundCountDown;
    bool isFirstTime;
    //int consumeNumber;
    // Start is called before the first frame update
    void Start()
    {
        //consumeNumber = data.consume;
        player = GetComponent<UIOwner>().owner;
        if (!player.photonView.IsMine || player.isBot)
        {
            gameObject.SetActive(false);
        }
        //consume.text = consumeNumber.ToString();
        //currentTimeTransformText.text = "";
        skill = player.GetComponentInChildren<TransformSkill>();
        isFirstTime = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state != GameManager.GameState.Playing || !player.photonView.IsMine)
        {
            return;
        }
        if (skill != null  || player.playerTemp == GameManager.HideOrSeek.Seek)
        {
            UICountDown();
            if(player.playerTemp != GameManager.HideOrSeek.Seek)
            {
                //iconUIFake.sprite = listIcon[skill.rpc.id];
                //iconUITranfrom.sprite = listIcon[skill.rpc.id];
                if (skill.currentTimeTransform > 0 && player.isTranform)
                {
                    string seconds = (skill.currentTimeTransform % 60).ToString("00");
                    //currentTimeTransformText.text = seconds;
                }
                else
                {
                   // currentTimeTransformText.text = "";
                }
            }
            
        }
        else
        {
            skill = player.GetComponentInChildren<TransformSkill>();
        }
    }
    public void UICountDown()
    {
        if (countdownUI.fillAmount >= 1)
        {
            countdownUI.gameObject.SetActive(false);
            countDownText.gameObject.SetActive(false);
            backgroundCountDown.gameObject.SetActive(false);
            isFirstTime = true;
            if (player.useSkill1 && player.isBot && PhotonNetwork.IsMasterClient)
            {
                player.useSkill1 = false;
                SkillClick2();
            }
            if(ConfigDataGame.fakeTransformMan < 10)
            {
                ConfigDataGame.fakeTransformMan = 99;
            }
        }
        else
        {
            time += Time.deltaTime;
            if (time >= 0.3)
            {
                countdownUI.gameObject.SetActive(true);
                countDownText.gameObject.SetActive(true);
                if (isFirstTime == true)
                {
                    int timeTemp = (int)(ConfigDataGame.fakeTransformMan - time + 1);
                    backgroundCountDown.gameObject.SetActive(true);
                    countDownText.text = timeTemp.ToString();
                    countdownUI.fillAmount = time / ConfigDataGame.fakeTransformMan;
                }
                else
                {
                    int timeTemp = (int)(GameManager.instance.firstTimeColdown - time + 1);
                    backgroundCountDown.gameObject.SetActive(true);
                    countDownText.text = timeTemp.ToString();
                    countdownUI.fillAmount = time / GameManager.instance.firstTimeColdown;
                }
            }
        }
    }
    public void SkillClick()
    {
        if (countdownUI.fillAmount == 1 /*&& skill.rpc.id != 0*/ && player.dead == false && player.isJumping == false && !player.isSilent && !player.isbeStun)
        {
            Utilities.FxButtonPress(imagePressFake.transform, true);
            Use();
            time = 0;
            countdownUI.fillAmount = 0;
        }
    }
    public void SkillClick2()
    {
        if (countdownUI.fillAmount == 1 /*&& skill.rpc.id != 0*/ && player.dead == false && !player.isbeStun)
        {
            Utilities.FxButtonPress(imagePressTranform.transform, true);
            Use2();
            time = 0;
            countdownUI.fillAmount = 0;
        }
    }
    public void Use()
    {
        time = 0;
        skill.Use3();
    }
    public void Use2()
    {
        time = 0;
        skill.Use();
    }
}
