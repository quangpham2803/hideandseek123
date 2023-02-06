using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneMove : MonoBehaviour
{
    public Transform target;
    public float time;
    public bool isMove;
    public Animator anim;
    public float speed;
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.instance.cloneTranform.Add(transform);
    }
    private void OnEnable()
    {
        StartCoroutine(moveInTime());
    }
    private void FixedUpdate()
    {
        if(isMove == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
            anim.SetBool("isRunning", true);
        }
    }
    IEnumerator moveInTime()
    {
        isMove = true;
        yield return new WaitForSeconds(time);
        isMove = false;
        anim.SetBool("isRunning", false);
    }
}
