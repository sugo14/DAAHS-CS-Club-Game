using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Platform Fighter/Level")]
public class Level : ScriptableObject
{
    public string levelName;
    public GameObject levelPrefab;
    public Sprite sprite;
}
