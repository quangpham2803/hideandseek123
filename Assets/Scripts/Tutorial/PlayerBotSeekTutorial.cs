using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBotSeekTutorial : MonoBehaviour
{
    //Movement

    public float rotationSpeed = 450f;
    public Rigidbody rigidbody;
    public PlayerSetupTutorial pSetup;
    private Vector3 inputVector;
    public float xPos, yPos;
    public LayerMask layerMask;
    public LayerMask layerWall;
    public Transform target;
    private void Awake()
    {
    }
    void Start()
    {
        pSetup = GetComponent<PlayerSetupTutorial>();
        rigidbody = GetComponent<Rigidbody>();
        pSetup.joystick = GameManagerTutorial.instance.joystick;
        pSetup.actionBtn = GameManagerTutorial.instance.actionBtn;
        pSetup.checkSeekNear.SetActive(true);
    }
    float x;
    float y;
    public bool done;
    public float f1;
    public float f2;
    private bool isPhysicWall;
    public GameObject mainPlayer;
    private void Update()
    {
        if (GameManagerTutorial.instance.state != GameManagerTutorial.GameState.Playing)
        {
            return;
        }
        //if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.Tutorial.Step8a)
        //{
        //    pSetup.pAnimator.SetBool("isRunning", false);
        //    pSetup.pAnimator.enabled = false;
        //    pSetup.agent.enabled = false;
        //    return;
        //}
        //if (!done)
        //{
        //    pSetup.agent.SetDestination(target.position);
        //    transform.LookAt(target.position);
        //}
        //if (pSetup.outPause)
        //{
        //    pSetup.outPause = false;
        //    pSetup.pAnimator.enabled = true;
        //    pSetup.agent.enabled = true;
        //    //done = false;
        //}
        //if (GameManagerTutorial.instance.tutorial == GameManagerTutorial.Tutorial.Step8 || GameManagerTutorial.instance.tutorial == GameManagerTutorial.Tutorial.Step10 && pSetup.agent.enabled == true)
        //{
        //    pSetup.agent.SetDestination(GameManagerTutorial.instance.mainPlayer.transform.position);
        //}
        pSetup.pAnimator.SetBool("isRunning", true);
        if (pSetup.player == GameManagerTutorial.HideOrSeek.Seek)
        {
            pSetup.timeToFire += Time.deltaTime;
            if (pSetup.attack && pSetup.timeToFire >= 1.5f)
            {
                pSetup.timeToFire = 0;
                pSetup.attack = false;
                //pSetup.Attack();
            }
        }
    }
}
