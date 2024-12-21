using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Platform Fighter/Character")]
public class Character : ScriptableObject
{
    public string characterName;
    public float weight;
    public float speed;
    public float height, width;
    public Sprite sprite;
}
