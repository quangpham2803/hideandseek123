using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImproveLegShield : MonoBehaviour
{
    PlayerSetup owner;
    public GameObject shieldEffect;
    // Start is called before the first frame update
    void Start()
    {
        owner = GetComponentInParent<PlayerSetup>();
        shieldEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(owner.isImmute == true)
        {
            shieldEffect.SetActive(true);
        }
        else
        {
            shieldEffect.SetActive(false);
        }
    }
}
