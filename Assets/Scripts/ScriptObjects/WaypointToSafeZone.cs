using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaypointToSafeZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetArrowIndicatorPositionAndAngle(ref Vector3 screenPosition, ref float angle, Vector3 screenCentre, Vector3 screenBounds, Camera mainCamera)
    {
        if (mainCamera.GetComponent<CameraFollow>() != null)
        {
            Vector3 playerPosition = mainCamera.WorldToScreenPoint(mainCamera.GetComponent<CameraFollow>().owner.transform.position);
        }
        else
        {
            Vector3 playerPosition = mainCamera.WorldToScreenPoint(mainCamera.GetComponent<CameraFollowTutorial>().owner.transform.position);
        }

        screenPosition -= screenCentre;
        if (screenPosition.z < 0)
        {
            screenPosition *= -1;
        }
        angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
        float slope = Mathf.Tan(angle);
        if (screenPosition.x > 0)
        {
            screenPosition = new Vector3(screenBounds.x, screenBounds.x * slope, 0);
        }
        else
        {
            screenPosition = new Vector3(-screenBounds.x, -screenBounds.x * slope, 0);
        }
        if (screenPosition.y > screenBounds.y)
        {
            screenPosition = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
        }
        else if (screenPosition.y < -screenBounds.y)
        {
            screenPosition = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);
        }
        screenPosition += screenCentre;
    }
}
