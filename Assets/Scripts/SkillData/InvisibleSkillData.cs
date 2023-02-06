using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName="Skill/InvisibleSkillData")]
public class InvisibleSkillData : ScriptableObject
{
    public string nameSkill;
    public float countdown;
    public float timeDuration;
    public Material invisibleTeam;
    public Material invisibleOtherTeam;
    public int consume;
}
