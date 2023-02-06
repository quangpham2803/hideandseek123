using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerProfile : MonoBehaviour
{
    public TMPro.TextMeshProUGUI playerNameText;
    public Image fillLevel;
    public TMPro.TextMeshProUGUI levelText;
    public TMPro.TextMeshProUGUI cupText;
    //Profile
    public TMPro.TextMeshProUGUI playerNameTextGet;
    public Image fillLevelGet;
    public TMPro.TextMeshProUGUI levelTextGet;
    public TMPro.TextMeshProUGUI cupTextGet;
    private void OnEnable()
    {
        playerNameTextGet.text = playerNameText.text;
        levelTextGet.text = levelText.text;
        cupTextGet.text = cupText.text;
        fillLevelGet.fillAmount = fillLevel.fillAmount;
    }
}
