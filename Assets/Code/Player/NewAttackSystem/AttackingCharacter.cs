using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Types of attacks that can be performed.
/// </summary>
public enum AttackType
{
    UpNormal,
    DownNormal,
    SideNormal,
    UpSpecial,
    DownSpecial,
    SideSpecial
}

/// <summary>
/// Directions that the character can face.
/// </summary>
public enum Facing
{
    Left,
    Right
}

/// <summary>
/// Performs attacks for the character.
/// </summary>
public class AttackingCharacter : MonoBehaviour
{
    [SerializeField] GameObject[] attacks = new GameObject[6];

    void Attack(GameObject attackPrefab, Facing facing)
    {
        GameObject attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        attack.GetComponent<Attack>().Initialize(gameObject, facing);
    }

    public void Attack(AttackType attackType, Facing facing)
    {
        AudioManager.PlaySound("Gun1");
        Attack(attacks[(int)attackType], facing);
    }
}
