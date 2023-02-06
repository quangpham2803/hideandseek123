using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager2 : MonoBehaviour
{
	public static MenuManager2 Instance;
    public GameObject playerName;
	[SerializeField] Menu2[] menus;

	void Awake()
	{
		Instance = this;
	}

	public void OpenMenu(string menuName)
	{
		for(int i = 0; i < menus.Length; i++)
		{
			if(menus[i].menuName == menuName)
			{
				menus[i].Open();
			}
			else if(menus[i].open)
			{
				CloseMenu(menus[i]);
			}
		}
        playerName.SetActive(true);
	}

	public void OpenMenu(Menu2 menu)
	{
		for(int i = 0; i < menus.Length; i++)
		{
			if(menus[i].open)
			{
				CloseMenu(menus[i]);
			}
		}
		menu.Open();
        playerName.SetActive(false);
    }

	public void CloseMenu(Menu2 menu)
	{
		menu.Close();
	}
}