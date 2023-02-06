using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            if (p.player == GameManager.HideOrSeek.Seek && Vector3.Distance(this.transform.position, p.transform.position) <= 8)
            {
                if (p.isBot)
                {
                    p.speedDown = 1f;
                    p.isBlackHole = true;
                }
                else
                {
                    p.agentPlayer.enabled = true;
                    p.agentPlayer.speed = 1.5f;
                }
                p.speedDown = 1;
                Vector3 ravenPos = this.transform.position;
                Vector3 pos = new Vector3(ravenPos.x, transform.position.y, ravenPos.z);
                if (p.isBot)
                {
                    p.agent.SetDestination(pos);
                }
                else
                {
                    p.agentPlayer.SetDestination(pos);
                    p.cam.GetComponent<CameraFilterPack_TV_Artefact>().enabled = true;
                }
            }
            else
            {
                if (!p.isBot)
                {
                    p.agentPlayer.speed = 2.5f;                   
                    if (!p.isSummonDarkRaven)
                        p.agentPlayer.enabled = false;
                    p.cam.GetComponent<CameraFilterPack_TV_Artefact>().enabled = false;
                }
                p.speedDown = 0;
                if (p.isBot)
                {
                    p.isBlackHole = false;
                }

            }
        }
    }
    private void OnDisable()
    {
        foreach (PlayerSetup p in GameManager.instance.players)
        {
            if (p.player == GameManager.HideOrSeek.Seek && Vector3.Distance(this.transform.position, p.transform.position) <= 10)
            {
                p.speedDown = 0;
                if (!p.isBot)
                {
                    p.agentPlayer.speed = 2.5f;
                    if (!p.isSummonDarkRaven)
                        p.agentPlayer.enabled = false;
                    p.cam.GetComponent<CameraFilterPack_TV_Artefact>().enabled = false;
                }
                else
                {
                    if (p.isBot)
                    {
                        p.isBlackHole = false;
                    }
                }
            }
        }
    }
}
