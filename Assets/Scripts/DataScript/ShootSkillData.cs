using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Skills/ShootSkill")]
public class ShootSkillData : ScriptableObject
{
    public string skillName;
    public float countdown;
    public float countdown2;
    public float bulletFlyTime;
    public float bulletSpeed;
    public int consume;
    public float range;
    public float slowTime;
}
