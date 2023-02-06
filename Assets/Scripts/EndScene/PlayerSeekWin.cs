using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSeekWin : MonoBehaviour
{
    public GameObject[] models;
    public TMPro.TextMeshProUGUI nameText;
    public string namePlayer;
    // Start is called before the first frame update
    public void AddPlayerAndName(int idChar, string playerName)
    {
        nameText.text = playerName;
        models[idChar].SetActive(true);
    }
}
