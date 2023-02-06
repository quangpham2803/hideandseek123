using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleNavTutorial : MonoBehaviour
{
    public Transform navigateCircle;
    public PlayerSetupTutorial owner;
    private void Start()
    {
        if (GameManagerTutorial.instance.mainPlayer == owner)
        {
            navigateCircle.gameObject.SetActive(true);
        }
        else
        {
            navigateCircle.gameObject.SetActive(false);
            this.enabled = false;
        }
    }
    private void Update()
    {
        if (GameManagerTutorial.instance.mainPlayer == owner)
        {
            Vector2 direction = owner.joystick.Direction;
            navigateCircle.position = (this.transform.position + new Vector3(-direction.x, 0, -direction.y).normalized);
        }
    }
}
