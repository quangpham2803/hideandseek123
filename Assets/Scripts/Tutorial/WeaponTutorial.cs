using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTutorial : MonoBehaviour
{
    public PlayerSetupTutorial owner;
    private void OnEnable()
    {
        if (GameManagerTutorial.instance.state == GameManagerTutorial.GameState.Playing)
        {
            owner = transform.parent.GetComponentInParent<PlayerSetupTutorial>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (GameManagerTutorial.instance.state != GameManagerTutorial.GameState.Playing)
        {
            return;
        }
        if (!other.CompareTag("Player"))
            return;
        PlayerSetupTutorial obj = other.GetComponent<PlayerSetupTutorial>();
        if (obj != null)
        {
            if (IsFriendlyFire(owner, obj)) return;
        }
        List<PlayerSetupTutorial> targets = new List<PlayerSetupTutorial>();
        if (obj != null && !obj.dead) targets.Add(obj);
        if (GameManagerTutorial.instance.mainPlayer.player == GameManagerTutorial.HideOrSeek.Seek)
        {
            Vibrator.Vibrate(100);
            owner.cam.StartCoroutine(owner.cam.Shake(0.2f, 0.4f));
        }

        for (int i = 0; i < targets.Count; i++)
        {           
            targets[i].playerKill = owner.transform.position;
            targets[i].Catch(owner.name.text, owner.transform.position);
        }
    }
    private bool IsFriendlyFire(PlayerSetupTutorial origin, PlayerSetupTutorial target)
    {
        if (target == owner || target == null) return true;
        else if (origin.player == target.player) return true;
        return false;
    }
}
