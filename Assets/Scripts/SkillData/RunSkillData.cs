using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Skill/RunSkillData")]
public class RunSkillData : ScriptableObject
{
    public string nameSkill;
    public float countdown;
    public float countdown2;
    public float timeDuration;
    public float timeDuration2;
    public float runningSpeed;
    public float improveSpeed;
    public int consume;
}
