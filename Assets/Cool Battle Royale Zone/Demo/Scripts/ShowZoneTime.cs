using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Photon.Pun;
using I2.Loc;

namespace CoolBattleRoyaleZone
{
    /// <summary>
    /// Class which controls showing the timer of zone
    /// </summary>
    public class ShowZoneTime : MonoBehaviourPunCallbacks
    {

        public TMPro.TextMeshProUGUI TimeText;
        public Image greenImg, redImg;
        public TMPro.TextMeshProUGUI ringText;
        public TMPro.TextMeshProUGUI alertText;
        public GameObject alertObject;
        public bool isAlert;
        // Method for showing shrinking timer
        public void ShowShrinkingTime ( Zone.Timer timer )
        {
            redImg.gameObject.SetActive(true);
            greenImg.gameObject.SetActive(false);
            ringText.text = (Zone.Instance.CurStep + 1).ToString();
            TimeText.text = "<color=red>" + (timer.EndTime - timer.CurrentTime).ToString("F0") + "</b></color>";
            if (PhotonNetwork.IsMasterClient && isAlert == false)
            {
                isAlert = true;
                photonView.RPC("alertCmd", RpcTarget.AllBuffered);
            }
        }
        
        // Method for clearing text on end of last circle/step
        public void EndOfLastStep ( Zone.Timer timer ) { TimeText.text = ""; }

        // Method for showing waiting timer before shrinking
        public void ShowWaitingTime ( Zone.Timer timer )
        {
            greenImg.gameObject.SetActive(true);
            redImg.gameObject.SetActive(false);
            ringText.text = (Zone.Instance.CurStep + 1).ToString();
            TimeText.text = (timer.EndTime - timer.CurrentTime).ToString("F0");
            if (PhotonNetwork.IsMasterClient && isAlert == true)
            {
                isAlert = false;
                photonView.RPC("alert2Cmd", RpcTarget.AllBuffered, (float)(Zone.Instance.CurStep + 1));
            }
        }
        IEnumerator alertTime()
        {
            isAlert = true;
            alertObject.SetActive(true);
            alertObject.GetComponent<Animator>().Play("zonescaleanim");
            if (LocalizationManager.CurrentLanguage == "English")
            {
                alertText.text = "RING CLOSING !!!";
                alertText.font = StatusPlayer.Instance.fontList[0];
            }
            else
            {
                alertText.text = "BO ĐANG THU LẠI !!!";
                alertText.font = StatusPlayer.Instance.fontList[1];
            }
            
            alertText.color = Color.red;
            yield return new WaitForSeconds(2.5f);
            alertObject.SetActive(false);
        }
        IEnumerator alertTime2(float time)
        {
            
            isAlert = false;
            alertObject.SetActive(true);
            alertObject.GetComponent<Animator>().Play("zonescaleanim");
            if (time < 4)
            {
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    alertText.text = "RING " + time + "/4";
                    alertText.font = StatusPlayer.Instance.fontList[0];
                }
                else
                {
                    alertText.text = "BO " + time + "/4";
                    alertText.font = StatusPlayer.Instance.fontList[1];
                }
            }
            else
            {              
                if (LocalizationManager.CurrentLanguage == "English")
                {
                    alertText.text = "LAST RING !!!";
                    alertText.font = StatusPlayer.Instance.fontList[0];
                }
                else
                {
                    alertText.text = "BO CUỐI !!!";
                    alertText.font = StatusPlayer.Instance.fontList[1];
                }
            }
            alertText.color = Color.white;
            yield return new WaitForSeconds(2.5f);
            alertObject.SetActive(false);
        }
        [PunRPC]
        void alertCmd()
        {
            StartCoroutine(alertTime());
        }
        [PunRPC]
        void alert2Cmd(float time)
        {
            StartCoroutine(alertTime2(time));
        }
    }
}
