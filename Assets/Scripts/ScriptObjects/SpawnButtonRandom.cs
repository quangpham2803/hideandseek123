using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnButtonRandom : MonoBehaviourPunCallbacks
{
    public static SpawnButtonRandom Spawn;
    public List<Transform> pointList;
    public ButtonGame[] triggerButtonList;
    public int haveAdd;
    void Awake()
    {
        Spawn = this;
        haveAdd = 0;
    }
    // Start is called before the first frame update
    public void SpawnRandom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var button in triggerButtonList)
            {
                if (button.isActive == false)
                {
                    foreach (var point in pointList)
                    {
                        int tm = Random.Range(0, 2);
                        if (tm == 1)
                        {
                            pointList.Remove(point);
                            photonView.RPC("SentButtonPosition", RpcTarget.AllBuffered, button.id, point.position);                         
                            break;
                        }
                    }
                }
            }
        }
    }
    [PunRPC]
    public void SentButtonPosition(int id, Vector3 position)
    {
        Debug.Log("add button : "+position +" " + id);
        foreach (var button in triggerButtonList)
        {
            if(button.id == id)
            {
                button.transform.position = position;
                StartCoroutine(ButtonAnim(button));
                break;
            }
            else
            {

            }
        }
    }
    IEnumerator ButtonAnim(ButtonGame button)
    {
        button.buttonBody.SetActive(false);
        button.GetComponent<Animator>().SetBool("appear", true);
        button.buttonBody.SetActive(true);
        yield return new WaitForSeconds(3f);
        button.GetComponent<Animator>().SetBool("appear", false);
    }

}
