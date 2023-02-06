using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Weapon : MonoBehaviourPunCallbacks
{
    public PlayerSetup owner;
    int tm;
    private void OnEnable()
    {
        if (GameManager.instance.state==GameManager.GameState.Playing)
        {
            owner = transform.parent.GetComponentInParent<PlayerSetup>();
            tm = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.state != GameManager.GameState.Playing)
        {
            return;
        }
        if (!other.CompareTag("Player") || owner.isbeStun)
            return;
        PlayerSetup obj = other.GetComponent<PlayerSetup>();
        if (obj != null)
        {
            if (IsFriendlyFire(owner, obj)) return;
        }
        List<PlayerSetup> targets = new List<PlayerSetup>();
        if (obj != null && !obj.dead) targets.Add(obj);
        //if (owner.photonView.IsMine && targets.Count != 0)
        //{
        //    SoundManager.Instance.PlaySound(SoundManager.AUDIOLIST.attack);
        //}
        if (owner.photonView.IsMine && targets.Count != 0 && !owner.isBot)
        {
            Vibrator.Vibrate(100);
            owner.cam.StartCoroutine(owner.cam.Shake(0.2f, 0.4f));
        }
        else if (owner.isBot && PhotonNetwork.IsMasterClient && targets.Count != 0)
        {
            owner.isHitPlayer = true;
            owner.playerBotHit = targets[0];
            owner.inRange.Clear();
        }
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].playerKill = owner.transform.position;
            if (owner.photonView.IsMine && !owner.isBot)
            {
                tm++;
                targets[i].photonView.RPC("Catch", RpcTarget.AllBuffered, owner.name.text, owner.transform.position);
            }
            else if (owner.isBot && PhotonNetwork.IsMasterClient)
            {
                targets[i].photonView.RPC("Catch", RpcTarget.AllBuffered, owner.name.text, owner.transform.position);
                tm++;
            }
        }
    }
    private void OnDisable()
    {
        if (tm == 0 && owner.photonView.IsMine && !owner.isBot)
        {
            owner.MissHit();
        }
        else if (tm == 0 && PhotonNetwork.IsMasterClient && owner.isBot)
        {
            owner.MissHit();
        }
    }
    private bool IsFriendlyFire(PlayerSetup origin, PlayerSetup target)
    {
        if (target == owner || target == null) return true;
        else if (origin.player == target.player) return true;
        return false;
    }
}
