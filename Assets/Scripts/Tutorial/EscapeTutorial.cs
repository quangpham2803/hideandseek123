using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeTutorial : MonoBehaviour
{
    //MiniMapComponent doorIcon;
    public bool opening, opened;
    public GameObject effect;
    Target target;
    bool playAnim;
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
        //if (playAnim == true && GameManager.instance.mainPlayer.cam.cinemachine.m_Lens.OrthographicSize > 2)
        //{
        //    GameManager.instance.mainPlayer.cam.cinemachine.m_Lens.OrthographicSize -= Time.deltaTime;
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.GetComponent<PlayerSetupTutorial>().player == GameManagerTutorial.HideOrSeek.Hide
            && GameManagerTutorial.instance.state != GameManagerTutorial.GameState.Ending && opened == true)
        {
            playAnim = true;

            StartCoroutine(waitToExplo(other.GetComponent<PlayerSetupTutorial>(), 0.5f));
            //transform.GetChild(0).gameObject.SetActive(false);
            GameManagerTutorial.instance.state = GameManagerTutorial.GameState.Ending;
            GameManagerTutorial.instance.seekWin = false;
            GameManagerTutorial.instance.canavasGame.SetActive(false);
            //GameManagerTutorial.instance.canavasTutorial.SetActive(false);
            GameManagerTutorial.instance.mainPlayer.cam.cinemachine.LookAt = other.transform;
            GameManagerTutorial.instance.mainPlayer.cam.cinemachine.Follow = other.transform;
            GameManagerTutorial.instance.mainPlayer.GetComponent<PlayerTutorial>().rigidbody.velocity = Vector3.zero;
            GameManagerTutorial.instance.lineRenderer.gameObject.SetActive(false);
            //StartCoroutine(PopUpMenu(GameManagerTutorial.instance.mainPlayer));
        }
    }
    IEnumerator PopUpMenu(PlayerSetupTutorial player)
    {
        GameManagerTutorial.instance.state = GameManagerTutorial.GameState.Ending;
        yield return new WaitForSeconds(0.5f);
        if (player.player == GameManagerTutorial.HideOrSeek.Hide)
        {
            player.cam.WinCam();
        }
        else
        {
            player.cam.LoseCam();
        }
    }
    public IEnumerator waitToExplo(PlayerSetupTutorial owner, float time)
    {
        GameManagerTutorial.instance.waypointCanavas.SetActive(false);
        owner.teleEffect.SetActive(true);
        yield return new WaitForSeconds(time);
        //owner.deadArrow.SetActive(false);
        owner.model.SetActive(false);
        //owner.deadComplete = true;
        owner.transform.Find("Other").gameObject.SetActive(false);
        if (GameManagerTutorial.instance.scene == GameManagerTutorial.isScene.Scene1)
        {
            GameManagerTutorial.instance.fadeScene.FadeToLevel("Scene2");
        }
        else if (GameManagerTutorial.instance.scene == GameManagerTutorial.isScene.Scene2)
        {
            GameManagerTutorial.instance.fadeScene.FadeToLevel("Scene3");
        }
        else
        {
            GameManagerTutorial.instance.popupMenu.SetActive(true);
            GameManagerTutorial.instance.backgroundBlack2.SetActive(true);
        }    
    }
    
}
