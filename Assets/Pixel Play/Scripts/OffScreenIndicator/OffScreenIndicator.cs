using PixelPlay.OffScreenIndicator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach the script to the off screen indicator panel.
/// </summary>
[DefaultExecutionOrder(-1)]
public class OffScreenIndicator : MonoBehaviour
{
    public static OffScreenIndicator offScreen;
    [Range(0.5f, 0.9f)]
    [Tooltip("Distance offset of the indicators from the centre of the screen")]
    [SerializeField] private float screenBoundOffset = 0.9f;

    public Camera mainCamera;
    private Vector3 screenCentre;
    private Vector3 screenBounds;

    private List<Target> targets = new List<Target>();

    public static Action<Target, bool> TargetStateChanged;
    public Transform wayzonePoint;

    void Awake()
    {
        offScreen = this;
        screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
        screenBounds = screenCentre * screenBoundOffset;
        TargetStateChanged += HandleTargetStateChanged;
    }
    void LateUpdate()
    {
        if (GameManager.instance !=null)
        {
            if (GameManager.instance.state == GameManager.GameState.Playing)
            {
                DrawIndicators();
            }
        }
        else
        {
            if (GameManagerTutorial.instance.state == GameManagerTutorial.GameState.Playing)
            {
                DrawIndicators();
            }
        }
    }
    bool add = true;

    /// <summary>
    /// Draw the indicators on the screen and set thier position and rotation and other properties.
    /// </summary>
    void DrawIndicators()
    {
        if (mainCamera==null)
        {
            if (GameManager.instance == null)
            {
                mainCamera = GameManagerTutorial.instance.mainPlayer.cam.GetComponent<Camera>();
            }
            else
            {
                mainCamera = GameManager.instance.mainPlayer.cam.GetComponent<Camera>();
            }
            return;
        }
        foreach(Target target in targets)
        {
            Vector3 screenPosition = Vector3.zero;
            if (mainCamera.GetComponent<CameraFollow>() != null)
            {
                screenPosition = OffScreenIndicatorCore.GetScreenPosition(mainCamera.GetComponent<CameraFollow>().uiCam, target.transform.position);
            }
            else
            {
                screenPosition = OffScreenIndicatorCore.GetScreenPosition(mainCamera.GetComponent<CameraFollowTutorial>().uiCam, target.transform.position);
            }
            bool isTargetVisible = OffScreenIndicatorCore.IsTargetVisible(screenPosition);
            float distanceFromCamera;
            try
            {
                if (mainCamera.GetComponent<CameraFollow>() != null)
                {
                    distanceFromCamera = target.NeedDistanceText ? target.GetDistanceFromCamera(mainCamera.GetComponent<CameraFollow>().owner.transform.position) : float.MinValue;// Gets the target distance from the camera.
                }
                else
                {
                    distanceFromCamera = target.NeedDistanceText ? target.GetDistanceFromCamera(mainCamera.GetComponent<CameraFollowTutorial>().owner.transform.position) : float.MinValue;// Gets the target distance from the camera.
                }
                
            }
            catch
            {
                distanceFromCamera = 0;
            }
            Indicator indicator = null;

            if(target.NeedBoxIndicator && isTargetVisible)
            {
                screenPosition.z = 0;
                indicator = GetIndicator(ref target.indicator, IndicatorType.BOX, target); // Gets the box indicator from the pool.
            }
            else if (target.NeedArrowIndicator && !isTargetVisible)
            {
                float angle = float.MinValue;
                OffScreenIndicatorCore.GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, screenCentre, screenBounds, mainCamera);
                indicator = GetIndicator(ref target.indicator, IndicatorType.ARROW, target); // Gets the arrow indicator from the pool.
                indicator.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg); // Sets the rotation for the arrow indicator.
            }
            else if (target.NeedArrowButtonIndicator && !isTargetVisible)
            {
                float angle = float.MinValue;
                OffScreenIndicatorCore.GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, screenCentre, screenBounds, mainCamera);
                indicator = GetIndicator(ref target.indicator, IndicatorType.ARROWBUTTON, target); 
                indicator.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
                indicator.btnGame = target.btnGame;
            }
            else if (target.NeedArrowDoorIndicator && !isTargetVisible)
            {
                float angle = float.MinValue;
                OffScreenIndicatorCore.GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, screenCentre, screenBounds, mainCamera);
                indicator = GetIndicator(ref target.indicator, IndicatorType.ARROWDOOR, target); 
                indicator.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            }
            else if (target.NeedArrowPlayerIndicator && !isTargetVisible)
            {
                float angle = float.MinValue;
                OffScreenIndicatorCore.GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, screenCentre, screenBounds, mainCamera);
                indicator = GetIndicator(ref target.indicator, IndicatorType.ARROWPLAYER, target); 
                indicator.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            }
            else if (target.NeedArrowDeadIndicator && !isTargetVisible)
            {
                float angle = float.MinValue;
                OffScreenIndicatorCore.GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, screenCentre, screenBounds, mainCamera);
                indicator = GetIndicator(ref target.indicator, IndicatorType.ARROWDEAD, target);
                indicator.revive = target.revive;
                indicator.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            }
            else if (target.NeedArrowHelpIndicator && !isTargetVisible)
            {
                float angle = float.MinValue;
                OffScreenIndicatorCore.GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, screenCentre, screenBounds, mainCamera);
                indicator = GetIndicator(ref target.indicator, IndicatorType.ARROWHELP, target);
                indicator.revive = target.revive;
                indicator.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            }
            if (mainCamera.GetComponent<CameraFollow>() != null)
            {
                //if (Vector3.Distance(target.transform.position, mainCamera.GetComponent<CameraFollow>().owner.transform.position) < 8f)
                //{
                //    add = false;

                //}
                //else
                //{
                //    add = true;
                //}
            }
            else
            {
                if (Vector3.Distance(target.transform.position, mainCamera.GetComponent<CameraFollowTutorial>().owner.transform.position) < 0f)
                {
                    add = false;
                }
                else
                {
                    add = true;
                }
            }
            

            if (indicator && add)
            {
                indicator.SetImageColor(target.TargetColor());// Sets the image color of the indicator.
                indicator.SetDistanceText(distanceFromCamera); //Set the distance text for the indicator.
                if (!indicator.isWayZone)
                {
                    indicator.transform.position = screenPosition; //Sets the position of the indicator on the screen.
                }
                else
                {
                    indicator.transform.position = wayzonePoint.position;
                }
                indicator.SetTextRotation(Quaternion.identity); // Sets the rotation of the distance text of the indicator.
            }
            else if (indicator && !add)
            {
                if(indicator.indicatorType == IndicatorType.ARROW || indicator.indicatorType == IndicatorType.ARROWBUTTON || indicator.indicatorType == IndicatorType.ARROWDOOR || indicator.indicatorType == IndicatorType.ARROWPLAYER || indicator.indicatorType == IndicatorType.ARROWDEAD)
                {
                    indicator.SetImageColor(new Color32(0, 0, 0, 0));
                }
                else
                {
                    indicator.transform.position = screenPosition;
                }
                indicator.distanceText.GetComponent<Text>().text="";
            }
        }
    }

    /// <summary>
    /// 1. Add the target to targets list if <paramref name="active"/> is true.
    /// 2. If <paramref name="active"/> is false deactivate the targets indicator, 
    ///     set its reference null and remove it from the targets list.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="active"></param>
    private void HandleTargetStateChanged(Target target, bool active)
    {
        if(active)
        {
            targets.Add(target);
        }
        else
        {
            target.indicator?.Activate(false);
            target.indicator = null;
            targets.Remove(target);
        }
    }

    /// <summary>
    /// Get the indicator for the target.
    /// 1. If its not null and of the same required <paramref name="type"/> 
    ///     then return the same indicator;
    /// 2. If its not null but is of different type from <paramref name="type"/> 
    ///     then deactivate the old reference so that it returns to the pool 
    ///     and request one of another type from pool.
    /// 3. If its null then request one from the pool of <paramref name="type"/>.
    /// </summary>
    /// <param name="indicator"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private Indicator GetIndicator(ref Indicator indicator, IndicatorType type, Target target)
    {
        if(indicator != null)
        {
            if(indicator.Type != type)
            {
                indicator.Activate(false);
                if (target.NeedArrowIndicator)
                {
                    indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowObjectPool.current.GetPooledObject();                    
                }
                else if (target.NeedArrowButtonIndicator)
                {
                    indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowButtonObjectPool.current.GetPooledObject();
                }
                else if (target.NeedArrowDoorIndicator)
                {
                    indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowDoorObjectPool.current.GetPooledObject();
                }
                else if (target.NeedArrowPlayerIndicator)
                {
                    indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowPlayerObjectPool.current.GetPooledObject();
                }
                else if (target.NeedArrowDeadIndicator)
                {
                    indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowDeadObjectPool.current.GetPooledObject();
                }
                else if (target.needArrowHelpIndicator)
                {
                    indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowHelpObjectPool.current.GetPooledObject();
                }
                indicator.Activate(true); // Sets the indicator as active.
            }

        }
        else
        {
            if (target.NeedArrowIndicator)
            {
                indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowObjectPool.current.GetPooledObject();
                indicator.Activate(true); // Sets the indicator as active.
            }
            else if (target.NeedArrowButtonIndicator)
            {
                indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowButtonObjectPool.current.GetPooledObject();
                indicator.Activate(true); // Sets the indicator as active.
            }
            else if (target.NeedArrowDoorIndicator)
            {
                indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowDoorObjectPool.current.GetPooledObject();
                indicator.Activate(true); // Sets the indicator as active.
            }
            else if (target.NeedArrowPlayerIndicator)
            {
                indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowPlayerObjectPool.current.GetPooledObject();
                indicator.Activate(true); // Sets the indicator as active.
            }
            else if (target.NeedArrowDeadIndicator)
            {
                indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowDeadObjectPool.current.GetPooledObject();
                indicator.Activate(true); // Sets the indicator as active.
            }
            else if (target.NeedArrowHelpIndicator)
            {
                indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowHelpObjectPool.current.GetPooledObject();
                indicator.Activate(true); // Sets the indicator as active.
            }
        }
        return indicator;
    }

    private void OnDestroy()
    {
        TargetStateChanged -= HandleTargetStateChanged;
    }
}
