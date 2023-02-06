using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Skills/TransformSkillData")]
public class TransformSkillData : ScriptableObject
{
    public string skillName;
    public float countdown;
    public float countdown2;
    public float timeTransform;
    public int consume;
}
