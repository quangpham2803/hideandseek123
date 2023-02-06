using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pocket : MonoBehaviour
{
    public static Pocket pocket;
    GameObject player;
    int curMoney;
    [SerializeField] private Text coinText;
    bool winOrLose;
    [SerializeField] GameObject winPanel, losePanel;
    [SerializeField] TMPro.TextMeshProUGUI winMoney, loseMoney;
    private void Awake()
    {
        if(pocket == null)
        {
            pocket = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        curMoney = 0;
        winOrLose = false;
    }
    public void AddMoney()
    {
        curMoney++;
        coinText.text = curMoney.ToString();
    }
    private void Update()
    {
        if (GameManager.instance.state == GameManager.GameState.Ending && winOrLose == false)
        {
            StartCoroutine(WaitPanel());
            winOrLose = true;
        }
    }
    IEnumerator WaitPanel()
    {
        yield return new WaitForSeconds(3f);
        if (player.GetComponent<PlayerSetup>().player == GameManager.HideOrSeek.Hide)
        {
            //if (!Escape.hideWin)
            //{
            //    losePanel.SetActive(true);
            //    loseMoney.text = curMoney.ToString();
            //}
            //else
            //{
            //    winPanel.SetActive(true);
            //    winMoney.text = curMoney.ToString();
            //}
        }
        else
        {
            //if (!Escape.hideWin)
            //{
            //    winPanel.SetActive(true);
            //    winMoney.text = curMoney.ToString();
            //}
            //else
            //{
            //    losePanel.SetActive(true);
            //    loseMoney.text = curMoney.ToString();
            //}
        }
    }
}
