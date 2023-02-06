using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizardShootSkill : MonoBehaviour
{
    public WizardObject WizardObject;
    GameObject bullet;
    GameObject lockEffect;
    GameObject bulletEffect;
    GameObject waypoint;
    //Bullet fly
    bool isFly;
    float currentTimeFly;
    //Bullet position
    Transform bulletStart, target, targetPosition;
    private bool temp;
    //Data form PlayerRPC
    PlayerRPC rpc;
    PlayerSetup owner;
    public GameObject slowEffect;
    public void Setup()
    {
        owner = GetComponentInParent<PlayerSetup>();
        rpc = GetComponentInParent<PlayerRPC>();
        bullet = WizardObject.bullet;
        lockEffect = WizardObject.lockEffect;
        bulletEffect = WizardObject.bulletEffect;
        waypoint = WizardObject.waypoint;
        bulletStart = WizardObject.bulletStart;
        target = WizardObject.target;
        targetPosition = WizardObject.targetPosition;
        lockEffect.SetActive(false);
        waypoint.SetActive(true);
        bulletEffect.SetActive(true);
        isFly = false;
        bullet.GetComponent<SleepBullet>().lockEffect = lockEffect;
        //Data setup
        rpc.isShoot = false;
        rpc.isDisactive = false;
        rpc.isActive = false;
        bullet.SetActive(false);
        rpc.isSlow = false;
    }
    public void Use()
    {
        rpc.WizardShootAction();
    }
    public void Use2()
    {
        StartCoroutine(WaitForStopSlow());
    }
    // Update is called once per frame
    void Update()
    {
        if (owner != null)
        {
            if (rpc.isSlow == true)
            {
                slowEffect.SetActive(true);
                if (!rpc.player.ObjectInvisible.Contains(slowEffect))
                {
                    rpc.player.ObjectInvisible.Add(slowEffect);
                }
                //foreach (PlayerSetup p in GameManager.instance.players)
                //{
                //    if (p.player != owner.player && Vector3.Distance(p.transform.position, owner.transform.position) <= data.range && !p.isSlow)
                //    {
                //        foreach (GameObject r in GameManager.instance.effectWizard)
                //        {
                //            if (!r.activeSelf)
                //            {                           
                //                owner.StartCoroutine(owner.SlowTime(data.slowTime, p, r, 2));
                //                break;
                //            }
                //        }
                //    }
                //}
                foreach (PlayerSetup p in GameManager.instance.players)
                {
                    if (p.player == owner.player && Vector3.Distance(p.transform.position, owner.transform.position) <= ConfigDataGame.wizardRange && !p.isSlow)
                    {
                        p.StartCoroutine(p.FastTime(ConfigDataGame.wizardTime, p, 1.5f));
                    }
                }
            }
            else
            {
                slowEffect.SetActive(false);
                rpc.player.ObjectInvisible.Remove(slowEffect);
            }

            //BulletControl();
            ActionAfterRPC();
        }       
    }
    //void BulletControl()
    //{
    //    //Bullet Move
    //    if (isFly == true)
    //    {
    //        currentTimeFly += Time.deltaTime;
    //        float step = data.bulletSpeed * Time.deltaTime;                      
    //        target.parent = null;
    //        bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, target.position, step);
    //        bullet.transform.LookAt(target);
    //    }
    //    if (currentTimeFly >= data.bulletFlyTime)
    //    {
    //        rpc.WizardDisActiveAction();
    //    }
    //}
    void ActionAfterRPC()
    {
        //Shoot action
        if (WizardObject != null)
        {
            if (rpc.isShoot == true)
            {
                bullet.SetActive(true);
                //bulletEffect.SetActive(true);
                //waypoint.SetActive(false);
                bullet.transform.position = transform.position;
                bullet.transform.parent = null;
                isFly = true;
                WizardObject.shootEffect.SetActive(true);
                //StartCoroutine(WaitForActive());
                rpc.isShoot = false;
            }
            //Disactive action
            if (rpc.isDisactive == true)
            {
                isFly = false;
                currentTimeFly = 0;
                bullet.SetActive(false);
                lockEffect.SetActive(false);
                WizardObject.shootEffect.SetActive(false);
                //bulletEffect.SetActive(true);
                rpc.isDisactive = false;
            }
            //Active action
            if (rpc.isActive == true)
            {
                //if (rpc.player.photonView.IsMine)
                //{
                //    waypoint.SetActive(true);
                //    bullet.SetActive(true);
                //    bulletEffect.SetActive(true);
                //}
                bullet.transform.position = bulletStart.position;
                target.parent = this.transform;
                target.position = targetPosition.position;
                bullet.transform.parent = this.transform;
                rpc.isActive = false;
            }
        }
    }
    //IEnumerator WaitForActive()
    //{
    //    yield return new WaitForSeconds(data.countdown);
    //    rpc.WizardActiveAction();
    //}
    IEnumerator WaitForStopSlow()
    {
        rpc.photonView.RPC("SlowOn", Photon.Pun.RpcTarget.AllBuffered);
        
        yield return new WaitForSeconds(3f);
        rpc.photonView.RPC("SlowOf", Photon.Pun.RpcTarget.AllBuffered);

    }
}
