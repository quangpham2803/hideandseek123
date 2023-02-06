using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager2 : MonoBehaviourPunCallbacks/*,IPunObservable*/
{
	public static RoomManager2 Instance;
	public int numberPlayer;
	public static bool isAwayTeam = false;
	public int players;
    bool loadScened;
    public SettingMenu setting;
    public int isHight;
    public int isShadow;
    public bool needBot;
    //public int numberPlayerCup;
	void Awake()
	{
		if(Instance == null)
		{
            Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}
	private void Start()
	{
        loadScened = false;
    }

	public override void OnEnable()
	{
		base.OnEnable();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
    GameObject player;

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if( scene.name == "mapgenerate" ) // We're in the game scene
        {
            player = PhotonNetwork.Instantiate(Path.Combine("Player", "Player"), Vector3.zero, Quaternion.identity);
            SettingMap();
            Destroy(gameObject);
        }
        //tutorial hide
        else if(scene.name == "Scene1" || scene.name == "Seek")
        {
            Destroy(gameObject);
        }
    }

	private void Update()
	{
		players = PhotonNetwork.PlayerList.Length;
	}
    void SettingMap()
    {
        isHight = PlayerPrefs.GetInt("isHight");
        isShadow = PlayerPrefs.GetInt("isShadow");
        if(isHight == 1)
        {
            GameManager.instance.crack.SetActive(true);
        }
        else
        {
            GameManager.instance.crack.SetActive(false);
        }
        if (isShadow == 1)
        {
            GameManager.instance.lightMap.shadows = LightShadows.Hard;

        }
        else
        {
            GameManager.instance.lightMap.shadows = LightShadows.None;
        }
        //GameManager.instance.numberPlayerCup = numberPlayerCup;
    }
    
    
}