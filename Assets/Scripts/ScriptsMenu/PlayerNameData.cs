using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PlayerData/Name")]
public class PlayerNameData : ScriptableObject
{
    public bool isNamed;
    public string playerName;
}
