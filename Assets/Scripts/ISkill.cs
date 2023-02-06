using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ISkill: MonoBehaviourPunCallbacks
{
    public PlayerSetup player;

    public PlayerSetup GetPlayerSetup()
    {
        return player;
    }
}