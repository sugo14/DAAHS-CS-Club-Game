using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character List", menuName = "Platform Fighter/Character List")]
public class CharacterList : ScriptableObject
{
    public Character[] characters;
}
