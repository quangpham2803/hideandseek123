using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOwner : MonoBehaviour
{
    public PlayerSetup owner;

    private void Start()
    {
        if (owner.isBot && PhotonNetwork.IsMasterClient)
        {
            transform.localPosition = new Vector3(0, 5000f, 0);
            Image[] childrenImage;
            childrenImage = transform.GetComponentsInChildren<Image>();
            foreach (Image rend in childrenImage)
            {
                rend.color = new Color(0,0,0,0);
            }
            Text[] childrenText;
            childrenText = transform.GetComponentsInChildren<Text>();
            foreach (Text rend in childrenText)
            {
                rend.color = new Color(0, 0, 0, 0);
            }
            TextMeshProUGUI[] childrenTextMesh;
            childrenTextMesh = transform.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI rend in childrenTextMesh)
            {
                rend.color = new Color(0, 0, 0, 0);
            }
        }
    }
}
