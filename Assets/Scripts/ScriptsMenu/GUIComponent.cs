using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GUIComponent : MonoBehaviourPunCallbacks
{
    public GUISkin guiSkin; // choose a guiStyle (Important!)

    public string text = "Player Name"; // choose your name

    public Color color = Color.white;   // choose font color/size
    public float fontSize = 10;
    public float offsetX = 0;
    public float offsetY = 0.5f;

    float boxW = 150f;
    float boxH = 20f;

    public bool messagePermanent = true;
    public Camera myCamera;
    public float messageDuration { get; set; }

    Vector2 boxPosition;

    PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        if (messagePermanent)
        {
            messageDuration = 9999999;
        }
    }

    void OnGUI()
    {
        if (PV.IsMine)
        {
            if (messageDuration > 0)
            {
                if (!messagePermanent) // if you set this to false, you can simply use this script as a popup messenger, just set messageDuration to a value above 0
                {
                    messageDuration -= Time.deltaTime;
                }

                GUI.skin = guiSkin;
                boxPosition = myCamera.WorldToScreenPoint(transform.position);
                boxPosition.y = Screen.height - boxPosition.y;
                boxPosition.x -= boxW * 0.1f;
                boxPosition.y -= boxH * 5f;
                guiSkin.box.fontSize = 10;

                GUI.contentColor = color;

                Vector2 content = guiSkin.box.CalcSize(new GUIContent(text));

                GUI.Box(new Rect(boxPosition.x - content.x / 2 * offsetX, boxPosition.y + offsetY, content.x, content.y), text);
            }
        }
    }
    [PunRPC]
    void GUICmd()
    {

    }
}