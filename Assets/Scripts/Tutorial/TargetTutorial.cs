using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTutorial : MonoBehaviour
{
    //[Tooltip("Change this color to change the indicators color for this target")]
    //[SerializeField] public Color targetColor = Color.red;

    //[Tooltip("Select if box indicator is required for this target")]
    //[SerializeField] public bool needBoxIndicator = true;

    //[Tooltip("Select if arrow indicator is required for this target")]
    //[SerializeField] public bool needArrowIndicator = true;

    //[Tooltip("Select if arrow indicator is required for this target")]
    //[SerializeField] public bool needArrowButtonIndicator = true;

    //[Tooltip("Select if arrow indicator is required for this target")]
    //[SerializeField] public bool needArrowDoorIndicator = true;

    //[Tooltip("Select if arrow indicator is required for this target")]
    //[SerializeField] public bool needArrowPlayerIndicator = true;

    //[Tooltip("Select if arrow indicator is required for this target")]
    //[SerializeField] public bool needArrowDeadIndicator = true;

    //[Tooltip("Select if arrow indicator is required for this target")]
    //[SerializeField] public bool needArrowHelpIndicator = true;

    //[Tooltip("Select if distance text is required for this target")]
    //[SerializeField] public bool needDistanceText = true;

    //public GameObject owner;
    //public ReviveCircle revive;
    //public bool isArrowDead;
    //public ButtonTutorial btnGame;
    //public bool isArrowBtn;
    ///// <summary>
    ///// Please do not assign its value yourself without understanding its use.
    ///// A reference to the target's indicator, 
    ///// its value is assigned at runtime by the offscreen indicator script.
    ///// </summary>
    //[HideInInspector] public InditorTutorial indicator;

    ///// <summary>
    ///// Gets the color for the target indicator.
    ///// </summary>
    //public Color TargetColor()
    //{
    //    return targetColor;
    //}

    ///// <summary>
    ///// Gets if box indicator is required for the target.
    ///// </summary>
    //public bool NeedBoxIndicator
    //{
    //    get
    //    {
    //        return needBoxIndicator;
    //    }
    //}

    ///// <summary>
    ///// Gets if arrow indicator is required for the target.
    ///// </summary>
    //public bool NeedArrowIndicator
    //{
    //    get
    //    {
    //        return needArrowIndicator;
    //    }
    //}
    //public bool NeedArrowButtonIndicator
    //{
    //    get
    //    {
    //        return needArrowButtonIndicator;
    //    }
    //}
    //public bool NeedArrowDoorIndicator
    //{
    //    get
    //    {
    //        return needArrowDoorIndicator;
    //    }
    //}
    //public bool NeedArrowPlayerIndicator
    //{
    //    get
    //    {
    //        return needArrowPlayerIndicator;
    //    }
    //}
    //public bool NeedArrowDeadIndicator
    //{
    //    get
    //    {
    //        return needArrowDeadIndicator;
    //    }
    //}
    //public bool NeedArrowHelpIndicator
    //{
    //    get
    //    {
    //        return needArrowHelpIndicator;
    //    }
    //}

    ///// <summary>
    ///// Gets if the distance text is required for the target.
    ///// </summary>
    //public bool NeedDistanceText
    //{
    //    get
    //    {
    //        return needDistanceText;
    //    }
    //}

    ///// <summary>
    ///// On enable add this target object to the targets list.
    ///// </summary>
    //private void OnEnable()
    //{
    //    if (OffScreenIndicatorTutorial.TargetStateChanged != null)
    //    {
    //        OffScreenIndicatorTutorial.TargetStateChanged.Invoke(this, true);
    //    }
    //    if (isArrowDead)
    //    {
    //        revive = owner.GetComponent<ReviveCircle>();
    //    }
    //    if (isArrowBtn)
    //    {
    //        btnGame = owner.GetComponent<ButtonTutorial>();
    //    }
    //}

    ///// <summary>
    ///// On disable remove this target object from the targets list.
    ///// </summary>
    //private void OnDisable()
    //{
    //    if (OffScreenIndicatorTutorial.TargetStateChanged != null)
    //    {
    //        OffScreenIndicatorTutorial.TargetStateChanged.Invoke(this, false);
    //    }
    //}

    ///// <summary>
    ///// Gets the distance between the camera and the target.
    ///// </summary>
    ///// <param name="cameraPosition">Camera position</param>
    ///// <returns></returns>
    //public float GetDistanceFromCamera(Vector3 cameraPosition)
    //{
    //    float distanceFromCamera = Vector3.Distance(cameraPosition, transform.position);
    //    return distanceFromCamera;
    //}
}
