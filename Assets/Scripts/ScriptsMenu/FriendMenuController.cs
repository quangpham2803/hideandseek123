using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendMenuController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI friendTab, requestTab, blackTab;
    public Color colorDefault;
    public GameObject foucusTab1, foucusTab2, foucusTab3;
    public GameObject containFriend, containRequest, containBlack;
    // Start is called before the first frame update
    void Start()
    {
        OpenTab(1);
    }
    public void OpenTab(int i)
    {
        if(i == 1)
        {
            friendTab.color = Color.white;
            foucusTab1.SetActive(true);
            containFriend.SetActive(true);
            requestTab.color = colorDefault;
            foucusTab2.SetActive(false);
            containRequest.SetActive(false);
            blackTab.color = colorDefault;
            foucusTab3.SetActive(false);
            containBlack.SetActive(false);
        }
        if(i == 2)
        {
            requestTab.color = Color.white;
            foucusTab2.SetActive(true);
            containRequest.SetActive(true);
            friendTab.color = colorDefault;
            foucusTab1.SetActive(false);
            containFriend.SetActive(false);
            blackTab.color = colorDefault;
            foucusTab3.SetActive(false);
            containBlack.SetActive(false);
        }
        if(i == 3)
        {
            blackTab.color = Color.white;
            foucusTab3.SetActive(true);
            containBlack.SetActive(true);
            requestTab.color = colorDefault;
            foucusTab2.SetActive(false);
            containRequest.SetActive(false);
            friendTab.color = colorDefault;
            foucusTab1.SetActive(false);
            containFriend.SetActive(false);
        }
    }
}
