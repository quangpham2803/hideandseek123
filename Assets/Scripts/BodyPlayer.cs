using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPlayer : MonoBehaviour
{
    public static BodyPlayer bodyInstitance;
    public GameObject body, circle, name;
    // Start is called before the first frame update
    private void Awake()
    {
        bodyInstitance = this;
        body = this.gameObject;
    }
}
