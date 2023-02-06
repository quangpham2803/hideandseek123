using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraFollow : MonoBehaviour
{
    public PlayerSetup owner;
    public Vector3 cameraOffset;
    public float Smoothfactor = 0.125f;
    protected Plane Plane;
    Touch touch;
    public GameObject UI;
    public List<PlayerSetup> playerList = new List<PlayerSetup>();
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
    public GameObject winImg, loseImg, winReward, loseReward;
    void Start()
    {
        camera = GetComponent<Camera>();
        objectMap = GameManager.instance.wallObject;
        UI.SetActive(false);
        StartCoroutine(waitForAdd());
        cinemachine.LookAt = owner.transform;
        cinemachine.Follow = owner.transform;
        view =hideView;
        btnDead.SetActive(false);
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        EndSceneManager.manager.camMain = this;
    }
    //Add players same role
    IEnumerator waitForAdd()
    {
        yield return new WaitUntil(isStarted);
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            if(p.player == owner.player && p != owner)
            {
                playerList.Add(p);               
            }
        }
    }
    bool isStarted()
    {
        return GameManager.instance.state == GameManager.GameState.Starting;
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
    public void TurnBackTo(PlayerSetup p)
    {
        UI.SetActive(false);
        owner = p;
        cinemachine.LookAt = owner.transform;
        cinemachine.Follow = owner.transform;
        GameManager.instance.waypointCanavas.SetActive(true);
    }
    public void WinCam()
    {
        UI.SetActive(true);
        btnDead.SetActive(false);
        losePanel.SetActive(false);
        winPanel.SetActive(true);
        btnBack1.onClick.AddListener(EndSceneManager.manager.leaveRoom.Home);
    } 
    public void LoseCam()
    {
        UI.SetActive(true);
        btnDead.SetActive(false);
        losePanel.SetActive(true);
        winPanel.SetActive(false);
        btnBack2.onClick.AddListener(EndSceneManager.manager.leaveRoom.Home);
    }
    public void OnClick()
    {
        isSwitch = true;
        if(playerList != null)
        {
            owner = playerList[i];
        }        
        GameManager.instance.waypointCanavas.SetActive(false);
        cinemachine.LookAt = owner.transform;
        cinemachine.Follow = owner.transform;
        if(i < playerList.Count -1)
        {
            i++;
        }
        else
        {
            i = 0;
        }
    }
    public void MoveToWinReward()
    {
        winImg.SetActive(false);
        winReward.SetActive(true);
    }
    public void MoveToLoseReward()
    {
        loseImg.SetActive(false);
        loseReward.SetActive(true);
    }
    IEnumerator AutoSwitch()
    {
        yield return new WaitForSeconds(3f);
        if(isSwitch == false)
        {
            OnClick();
        }
    }
    bool temp = false;
    public float t;
    // Update is called once per frame
    void LateUpdate()
    {
        if (owner==null )
        {
            return;
        }
        if (GameManager.instance.state == GameManager.GameState.Playing)
        {
            //Debug.DrawRay(transform.position, (-transform.position + owner.transform.position) * 100f, Color.yellow);
            RaycastHit[] hit = null;
            hit = Physics.RaycastAll(transform.position, -transform.position + owner.transform.position, 100f, layerMask);
            if (hit.Length != 0)
            {
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].transform.GetComponent<MeshRenderer>() != null && hit[i].transform.GetComponent<MeshRenderer>().sharedMaterial != material)
                    {
                        hit[i].transform.GetComponent<MeshRenderer>().sharedMaterial = material;
                    }
                }
                temp = false;
            }
            else if (!temp)
            {
                temp = true;
                foreach (GameObject item in objectMap)
                {
                    if (item.GetComponent<MeshRenderer>().sharedMaterial != defaultMaterial)
                    {
                        item.GetComponent<MeshRenderer>().sharedMaterial = defaultMaterial;
                    }
                }
            }
        }
    }
    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
        {
            return Vector3.zero;
        }

        //delta
        var rayBefore = camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = camera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
        {
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);
        }

        //not on plane
        return Vector3.zero;
    }

    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
        {
            return rayNow.GetPoint(enterNow);
        }

        return Vector3.zero;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(originalPos.x+ x,originalPos.y+ y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPos;
    }
}