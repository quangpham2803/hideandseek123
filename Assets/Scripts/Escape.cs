using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Escape : MonoBehaviourPunCallbacks
{
    //MiniMapComponent doorIcon;
    public bool opening, opened;
    public GameObject effect;
    Target target;
    public bool playAnim;
    private void Start()
    {
        //doorIcon = GetComponent<MiniMapComponent>();
        //doorIcon.mme.icon = closeDoor;
        //doorIcon.UpdateMapObject();
        opening = false;
        opened = false;
        effect.SetActive(false);
        target = GetComponent<Target>();
        target.enabled = false;
        playAnim = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && PhotonNetwork.IsMasterClient)
        {
            GameManager.instance.state = GameManager.GameState.Ending;
            GameManager.instance.seekWin = false;
            GameManager.instance.waypointCanavas.SetActive(false);
            GameManager.instance.mainPlayer.cam.cinemachine.LookAt = GameManager.instance.mainPlayer.transform;
            GameManager.instance.mainPlayer.cam.cinemachine.Follow = GameManager.instance.mainPlayer.transform;
            GameManager.instance.mainPlayer.GetComponent<Player>().rigidbody.velocity = Vector3.zero;
            playAnim = true;
            StartCoroutine(waitToExplo(GameManager.instance.mainPlayer, 0.5f));
            EndSceneManager.manager.OpenEndScene(GameManager.instance.mainPlayer.name.text, 1, GameManager.instance.mainPlayer.photonView.ViewID);
        }
        //if(opening == true)
        //{
        //    doorIcon.mme.icon= openingDoor;
        //    doorIcon.UpdateMapObject();
        //    opening = false;
        //}
        if (opened == true)
        {
            effect.SetActive(true);
            target.enabled = true;
        }
        if(playAnim == true && GameManager.instance.mainPlayer.cam.cinemachine.m_Lens.OrthographicSize > 2)
        {
            GameManager.instance.mainPlayer.cam.cinemachine.m_Lens.OrthographicSize -= Time.deltaTime;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") &&
            GameManager.instance.state == GameManager.GameState.Playing &&
            other.GetComponent<PlayerSetup>().dead == false &&
            other.GetComponent<PlayerSetup>().photonView.IsMine &&
            other.GetComponent<PlayerSetup>().player==GameManager.HideOrSeek.Hide &&             
            opened == true)
        {
            PlayerSetup p = other.GetComponent<PlayerSetup>();
            int id = 0;
            if (p.character == GameManager.Character.TransformMan) id = 0;
            if (p.character == GameManager.Character.Wizard) id = 1;
            if (p.character == GameManager.Character.SpeedMan) id = 2;
            EndSceneManager.manager.OpenEndScene(p.name.text, 
                id, p.photonView.ViewID);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!opened)
        {
            return;
        }
        if (other.gameObject.CompareTag("Player") &&
            GameManager.instance.state == GameManager.GameState.Playing &&
            other.GetComponent<PlayerSetup>().dead == false &&
            other.GetComponent<PlayerSetup>().photonView.IsMine &&
            other.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Hide)
        {
            PlayerSetup p = other.GetComponent<PlayerSetup>();
            int id = 0;
            if (p.character == GameManager.Character.TransformMan) id = 0;
            else if (p.character == GameManager.Character.Wizard) id = 1;
            else if (p.character == GameManager.Character.SpeedMan) id = 2;
            EndSceneManager.manager.OpenEndScene(p.name.text,
                id, p.photonView.ViewID);
        }
        if (other.gameObject.CompareTag("wall") || other.gameObject.CompareTag("prop") || other.gameObject.CompareTag("Grass"))
        {
            Destroy(other.gameObject);
        }
    }
    public IEnumerator waitToExplo(PlayerSetup owner, float time)
    {
        GameManager.instance.waypointCanavas.SetActive(false);
        owner.teleEffect.SetActive(true);
        yield return new WaitForSeconds(time);
        owner.deadArrow.SetActive(false);
        owner.model.SetActive(false);
        owner.deadComplete = true;
        owner.transform.Find("Other").gameObject.SetActive(false);       
    }
}
