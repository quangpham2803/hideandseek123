using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashObjectTutorial : MonoBehaviour
{
    public GameObject trashBody;
    public GameObject[] trashFall;
    public GameObject[] trashWater;
    public bool isFall;
    public int id;
    public int wayFall;
    private void Start()
    {
        isFall = false;
    }
}
