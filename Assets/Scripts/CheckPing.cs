using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPing : MonoBehaviour
{
	public Text ping;
	public float frequency = 0.5F;
	private Color color = Color.white;
	public Image pingBackground;
	private void OnEnable()
	{
		StartCoroutine(Ping());
	}
	IEnumerator Ping()
	{
		while (true)
		{
			int pingTemp = PhotonNetwork.GetPing();
			ping.text = pingTemp.ToString() +" Ping";
			color = (pingTemp >= 100) ? Color.red : ((pingTemp > 60) ? Color.yellow : Color.green);
			color.a = 0.7f;
			pingBackground.color = color;
			yield return new WaitForSeconds(frequency);
		}
	}
}
