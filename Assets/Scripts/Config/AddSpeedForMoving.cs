using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpeedForMoving : MonoBehaviour
{
    public bool isBot;
    private void Start()
    {
        if (!isBot)
        {
            GetComponent<PlayerSetup>().hideSpeed = ConfigDataGame.speedHide;
            GetComponent<PlayerSetup>().seekSpeed = ConfigDataGame.speedSeek;
        }
        else
        {
            GetComponent<PlayerSetup>().hideSpeed = ConfigDataGame.speedHideBot;
            GetComponent<PlayerSetup>().seekSpeed = ConfigDataGame.speedSeekBot;
        }
        
    }
}
