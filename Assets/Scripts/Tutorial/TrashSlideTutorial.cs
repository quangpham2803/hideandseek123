using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSlideTutorial : MonoBehaviour
{
    public PlayerSetupTutorial owner;
    public float time;
    public bool isSliding;
    public float speed;
    public PlayerRPCTutorial rpc;
    private void Start()
    {
        isSliding = false;
    }
    public IEnumerator SlideTime()
    {
        owner.isbeStun = true;
        foreach (AnimatorControllerParameter item in owner.pAnimator.parameters)
        {
            if (item.name != "Death")
            {
                owner.pAnimator.SetBool(item.name, false);
            }
        }
        owner.pAnimator.SetBool("Death", true);
        owner.speedSlide = 0;
        isSliding = true;
        yield return new WaitForSeconds(time);
        if (isSliding == true)
        {
            if (owner.dead == false)
            {
                owner.pAnimator.SetBool("Death", false);
            }
            owner.isbeStun = false;
            owner.speedSlide = 1;
            isSliding = false;
        }
    }
    private void FixedUpdate()
    {
        if (isSliding == true)
        {
            owner.transform.position = Vector3.MoveTowards(owner.transform.position, this.transform.position, speed);
            owner.transform.LookAt(transform.position);
            if (owner.speedStun == 0)
            {
                isSliding = false;
                owner.speedSlide = 1;
                if (owner.dead == false)
                {
                    owner.pAnimator.SetBool("Death", false);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isSliding == true && (other.CompareTag("wall") || other.CompareTag("prop") || other.CompareTag("table")))
        {
            isSliding = false;
            owner.speedSlide = 1;
            if (owner.dead == false)
            {
                owner.pAnimator.SetBool("Death", false);
            }
            rpc.StunSeek(owner);
        }
    }
}
