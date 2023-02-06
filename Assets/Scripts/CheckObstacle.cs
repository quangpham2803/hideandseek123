//using Photon.Pun;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using MonoBehaviour = UnityEngine.MonoBehaviour;

//public class CheckObstacle : MonoBehaviour
//{
//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.transform != transform.parent)
//        {
//            if (other.gameObject.layer==LayerMask.NameToLayer("Obstacle"))
//            { 
//                GetComponentInParent<PlayerBot>().isObstacle = true;
//            }
//            else
//            {
//                GetComponentInParent<PlayerBot>().isObstacle = false;
//            }
//        }
//    }
//}
