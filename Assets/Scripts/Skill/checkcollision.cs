using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkcollision : MonoBehaviour
{
    public bool isCheck;
    public CheckStayInProp[] listCheck;
    public Transform owner;
    private void OnTriggerStay(Collider other)
    {
        if (isCheck == true)
        {
            if (other.CompareTag("prop") || other.CompareTag("table"))
            {
                foreach (CheckStayInProp i in listCheck)
                {
                    if (i.isStay == false)
                    {
                        owner.position = new Vector3(i.transform.position.x, owner.position.y, i.transform.position.z);
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
    }
}
