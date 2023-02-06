using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cage : MonoBehaviour
{
    public float waitTime;
    private float currentTime;
    public Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state == GameManager.GameState.Playing)
        {
            if (currentTime > 0)
            {
                timeText.transform.parent.gameObject.SetActive(true);
                currentTime -= Time.deltaTime;
                string seconds = ((int)currentTime).ToString("0");
                timeText.text = seconds;
            }
            else
            {
                timeText.text = "0";
                
                timeText.transform.parent.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}
