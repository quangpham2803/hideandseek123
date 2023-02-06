using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonStat : MonoBehaviour
{
    public static SummonStat instance;
    private void Awake()
    {
        instance = this;
    }
    [Header("Joker")]
    public string jokerSummon;
    public GameObject jokerBomb;
    [Header("DarkRaven")]
    public GameObject slowEffect;
    public string darkravenSummon;
    public GameObject shotPrefab;

    [Header("Wizard")]
    public GameObject invisibleObject;
    public GameObject wizardObj;

    [Header("TranformMan")]
    public GameObject tranformObj;

    [Header("SpeedMan")]
    public GameObject speedManInvisibleObject;
    public GameObject speedManShield;
    public GameObject speedManDash;
    [Header("Davros")]
    public GameObject davrosScanObject;
    public GameObject banDageObject;
    public GameObject lineRenderObject;
}
