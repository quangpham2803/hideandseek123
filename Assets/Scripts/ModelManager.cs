using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    [Header("Seek")]
    public GameObject jokerModel;
    public GameObject davrosModel;
    public GameObject darkRavenModel;

    [Header("Hide")]
    public GameObject wizardModel;
    public GameObject tranformManModel;
    public GameObject speedManModel;

    public List<GameObject> listModelHider = new List<GameObject>();
}
