using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class WizardObject : MonoBehaviourPun
{
    public GameObject bullet;
    public GameObject lockEffect;
    public GameObject bulletEffect;
    public GameObject waypoint;
    public Transform bulletStart, target, targetPosition;
    public PlayerSetup owner;
    public GameObject shootEffect;
    private void Start()
    {
        transform.SetParent(owner.transform);
        transform.position = owner.transform.position;
        owner.GetComponentInChildren<WizardShootSkill>().WizardObject = this;
        owner.GetComponentInChildren<WizardShootSkill>().Setup();
    }
    public void LockEffectActive(Transform position)
    {
        lockEffect.SetActive(true);
        if (lockEffect.transform.parent != null)
        {
            lockEffect.transform.parent = null;
        }

        lockEffect.transform.SetParent(position);
        lockEffect.transform.localPosition = Vector3.zero;
        StartCoroutine(StunTime(5f, lockEffect));
    }
    public IEnumerator StunTime(float time, GameObject p)
    {
        yield return new WaitForSeconds(time);
        p.SetActive(false);
    }
}
