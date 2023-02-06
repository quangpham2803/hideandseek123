using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunToEscape : MonoBehaviour
{
    public Transform escapePoint;
    public float speed;
    public Animator anim;
    public GameObject[] models;
    public GameObject effect;
    public float timeDisappear;
    public bool isEscape;
    public Camera cam1;
    public Camera cam2;
    public GameObject canavasFade;
    public GameObject canvasOpen;
    public TMPro.TextMeshProUGUI playerName;
    public RunToEscape seeker;
    private void OnEnable()
    {
        isEscape = false;
        cam1.enabled = true;
        cam2.enabled = false;
    }
    private void FixedUpdate()
    {
        if (!isEscape)
        {
            if (playerName != null)
            {
                playerName.text = EndSceneManager.manager.nameHider;
            }
            transform.position = Vector3.MoveTowards(transform.position, escapePoint.position, speed);
            transform.LookAt(escapePoint);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("escape"))
        {
            StartCoroutine(PlayerTele());
        }
    }
    IEnumerator PlayerTele()
    {
        isEscape = true;
        cam1.enabled = false;
        cam2.enabled = true;
        yield return new WaitForSeconds(1.5f);
        cam2.transform.parent = null;
        yield return new WaitForSeconds(0.5f);
        seeker.anim.SetBool("isEscape", true);
        yield return new WaitForSeconds(0.5f);
        effect.SetActive(true);
        anim.enabled = false;
        canvasOpen.SetActive(false);
        foreach (GameObject g in models)
        {
            g.SetActive(false);
        }
        yield return new WaitForSeconds(2.5f);
        canavasFade.SetActive(true);
        StartCoroutine(EndSceneManager.manager.PopUpMenu(GameManager.instance.mainPlayer));
    }
}
