using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowCloneName : MonoBehaviour
{
    public TMPro.TextMeshProUGUI playerName;
    public TransformSkill host;
    void Start()
    {
        playerName.text = host.rpc.player.name.text;
        playerName.color = Color.green;
    }
    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
    }
}
