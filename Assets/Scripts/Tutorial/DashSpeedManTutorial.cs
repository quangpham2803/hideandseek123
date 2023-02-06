using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSpeedManTutorial : MonoBehaviour
{
    public Transform target;
    public Collider triggerBox;
    public bool isDash;
    public PlayerSetupTutorial owner;
    public RunSkillData data;
    float currentTime;
    public float timeDash;
    public PlayerRPCTutorial rpc;
    public GameObject smoke;
    public bool isNearWall;

    public GameObject blackHole;
    void Start()
    {
        smoke.SetActive(false);
        isDash = false;
        owner = GetComponentInParent<PlayerSetupTutorial>();
        rpc = owner.rpc;
        isNearWall = false;
    }
    void Update()
    {
        if (isDash == true)
        {
            if (owner.dead == false && !owner.isbeStun)
            {
                //if(owner == GameManager.instance.mainPlayer && GameManager.instance.isHighQuality)
                //{
                //    owner.cam.GetComponent<CameraFilterPack_Drawing_Manga_Flash_Color>().enabled = true;
                //}
                owner.isMoving = true;
                float step = data.improveSpeed * Time.deltaTime;
                owner.transform.position = Vector3.MoveTowards(owner.transform.position, target.position, step);
                //owner.transform.LookAt(target.position);
                currentTime += Time.deltaTime;
                if (currentTime > timeDash)
                {
                    DisableDash();
                    
                }
            }
            else
            {
                DisableDash();
            }
        }
        if (rpc.isDash)
        {
            smoke.SetActive(true);
        }
        else
        {
            smoke.SetActive(false);
        }
    }
    public void Use()
    {
        if (GameManagerTutorial.instance.scene == GameManagerTutorial.isScene.Scene1 && GameManagerTutorial.instance.tutorial == GameManagerTutorial.SceneTutorial1.Step4)
        {
            GameManagerTutorial.instance.tutorial = GameManagerTutorial.SceneTutorial1.Step5;
        }
        isDash = true;
        //owner.collider.enabled = false;
        currentTime = 0;
        owner.speedDash = 0;
        rpc.ActiveDash(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isDash == true)
        {
            //if (other.CompareTag("wall") || other.CompareTag("prop"))
            //{
            //    DisableDash();
            //    rpc.StunSeek(owner);
            //}
            if (other.CompareTag("Player") && owner.dead == false && other.GetComponent<PlayerSetupTutorial>().dead == false)
            {
                DisableDash();
                rpc.StunSeek(owner);
            }
            if (other.CompareTag("table"))
            {
                DisableDash();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("wall") || other.CompareTag("prop") || other.CompareTag("Player"))
        {
            isNearWall = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("wall") || other.CompareTag("prop") || other.CompareTag("Player"))
        {
            isNearWall = false;
        }
    }
    void DisableDash()
    {
        isDash = false;
        //owner.boxCollider.enabled = true;
        currentTime = 0;
        //owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.transform.position, 0);
        rpc.SetDefaultMaterial(owner);
        owner.speedDash = 1;
        owner.isMoving = false;
        rpc.ActiveDash(false);
        
    }

}
