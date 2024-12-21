using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level List", menuName = "Platform Fighter/Level List")]
public class LevelList : ScriptableObject
{
    public Level[] levels;
}
