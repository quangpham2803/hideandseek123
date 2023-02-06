using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraFollowTutorial : MonoBehaviour
{
    public PlayerSetupTutorial owner;
    public Vector3 cameraOffset;
    public float Smoothfactor = 0.125f;
    protected Plane Plane;
    Touch touch;
    public GameObject UI;
    public List<PlayerSetupTutorial> playerList = new List<PlayerSetupTutorial>();
    public int i = 0;
    public float view;

    public float seekView;
    public float hideView;
    public float viewSeek;
    public float smoothTime;
    public Material material;
    public Material defaultMaterial;
    Camera camera;
    private List<GameObject> objectMap = new List<GameObject>();
    public LayerMask layerMask;
    public CinemachineVirtualCamera cinemachine;
    public Camera uiCam;
    public GameObject btnDead, winPanel, losePanel;
    public Button btnBack1, btnBack2;
    public bool isSwitch;
    void Start()
    {
        camera = GetComponent<Camera>();
        objectMap = GameManagerTutorial.instance.wallObject;
        //UI.SetActive(false);
        StartCoroutine(waitForAdd());
        cinemachine.LookAt = owner.transform;
        cinemachine.Follow = owner.transform;
        view = hideView;
        //btnDead.SetActive(false);
        //losePanel.SetActive(false);
        //winPanel.SetActive(false);
    }
    //Add players same role
    IEnumerator waitForAdd()
    {
        yield return new WaitUntil(isStarted);
        foreach (PlayerSetupTutorial p in GameManagerTutorial.instance.players)
        {
            if (p.player == owner.player && p != owner)
            {
                playerList.Add(p);
            }
        }
    }
    bool isStarted()
    {
        return GameManagerTutorial.instance.state == GameManagerTutorial.GameState.Starting;
    }
    public void CamDead()
    {
        UI.SetActive(true);
        btnDead.SetActive(true);
        StartCoroutine(AutoSwitch());
        GetComponent<CameraFilterPack_Color_GrayScale>().enabled = true;
    }
    public void CamNearDead()
    {
        UI.SetActive(true);
        btnDead.SetActive(true);
        StartCoroutine(AutoSwitch());
    }
    public void TurnBackTo(PlayerSetupTutorial p)
    {
        UI.SetActive(false);
        owner = p;
        cinemachine.LookAt = owner.transform;
        cinemachine.Follow = owner.transform;
        GameManagerTutorial.instance.waypointCanavas.SetActive(true);
    }
    public GameObject Skip;
    public void SkipCam()
    {
        Skip.SetActive(true);
    }
    public void Yes()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        //PhotonNetwork.LeaveRoom();
    }
    public void Next()
    {
        SceneManager.LoadScene(3);
        //PhotonNetwork.LeaveRoom();
    }
    public void No()
    {
        Skip.SetActive(false);
        Time.timeScale = 1;
        GameManagerTutorial.instance.state = GameManagerTutorial.GameState.Playing;
    }
    public void WinCam()
    {
        UI.SetActive(true);
        btnDead.SetActive(false);
        losePanel.SetActive(false);
        winPanel.SetActive(true);
        btnBack1.onClick.AddListener(Home);
    }
    public void LoseCam()
    {
        UI.SetActive(true);
        btnDead.SetActive(false);
        losePanel.SetActive(true);
        winPanel.SetActive(false);
        btnBack2.onClick.AddListener(Home);
    }
    public void Home()
    {
        PhotonNetwork.LeaveRoom();
        if(GameManagerTutorial.instance.mainPlayer.player == GameManagerTutorial.HideOrSeek.Seek)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(3);
        }

    }
    public void OnClick()
    {
        isSwitch = true;
        owner = playerList[i];
        GameManagerTutorial.instance.waypointCanavas.SetActive(false);
        cinemachine.LookAt = owner.transform;
        cinemachine.Follow = owner.transform;
        if (i < playerList.Count - 1)
        {
            i++;
        }
        else
        {
            i = 0;
        }
    }
    IEnumerator AutoSwitch()
    {
        yield return new WaitForSeconds(3f);
        if (isSwitch == false)
        {
            OnClick();
        }
    }
    bool temp = false;
    public float t;
    // Update is called once per frame
    void LateUpdate()
    {
        if (owner == null)
        {
            return;
        }
        //if (GameManagerTutorial.instance.state == GameManagerTutorial.GameState.Playing)
        //{
        //    //Debug.DrawRay(transform.position, (-transform.position + owner.transform.position) * 100f, Color.yellow);
        //    RaycastHit[] hit = null;
        //    hit = Physics.RaycastAll(transform.position, -transform.position + owner.transform.position, 100f, layerMask);
        //    if (hit.Length != 0)
        //    {
        //        for (int i = 0; i < hit.Length; i++)
        //        {
        //            if (hit[i].transform.GetComponent<MeshRenderer>() != null && hit[i].transform.GetComponent<MeshRenderer>().sharedMaterial != material)
        //            {
        //                hit[i].transform.GetComponent<MeshRenderer>().sharedMaterial = material;
        //            }
        //        }
        //        temp = false;
        //    }
        //    else if (!temp)
        //    {
        //        temp = true;
        //        foreach (GameObject item in objectMap)
        //        {
        //            if (item.GetComponent<MeshRenderer>().sharedMaterial != defaultMaterial)
        //            {
        //                item.GetComponent<MeshRenderer>().sharedMaterial = defaultMaterial;
        //            }
        //        }
        //    }
        //}
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPos;
    }
}
