using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using System.Linq;
using System;

public class GameSetupController : UnityEngine.MonoBehaviour
{
    public static GameSetupController GS;
    public Transform[] point;
    public Text PlayerSeekText;
    public Transform seekPoint;
    public int hideNumber;
    public float timeLeft;
    public bool hideWin;
    public bool seekWin;
    public enum Job
    {
        Hide,
        Seek,
    }
    public int hideChoosed;
    public float timeSeekGo;

    private void OnEnable()
    {
        if(GameSetupController.GS == null)
        {
            GameSetupController.GS = this;
        }
        hideChoosed = 0;
        hideWin = false;
        seekWin = false;
    }
    private void Update()
    {
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

        }
        else
        {
            timeLeft = 0;
            hideWin = true;
        }

        if(hideNumber > 0)
        {
            if(hideNumber == 0)
            seekWin = true;
        }
    }
}
