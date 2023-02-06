using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Invisible : MonoBehaviour
{
    public PlayerSetup owner;
    public Material materialTeam;
    public Material materialOther;
    public float timeInvisible;

    public GameObject smoke;    
    void Update()
    {
        timeInvisible += Time.deltaTime;
        if (timeInvisible >= 3)
        {
            foreach (PlayerSetup p in GameManager.instance.players)
            {
                if (!owner.inGrass)
                {
                    p.rpc.SetDefaultMaterial(owner);
                }
                else 
                {
                    p.changePlayerInGrass = true;
                }
            }
            GameManager.instance.isInvisible = false;
            owner.invisible = false;
            gameObject.SetActive(false);
        }
    }
}
