using UnityEngine;

public enum AttackType
{
    UpNormal,
    DownNormal,
    SideNormal,
    UpSpecial,
    DownSpecial,
    SideSpecial
}

public class AttackingCharacter : MonoBehaviour
{
    public GameObject[] attacks = new GameObject[6];

    void Attack(GameObject attackPrefab)
    {
        GameObject attack = Instantiate(attackPrefab);
        attack.GetComponent<Attack>().Initialize(gameObject);
    }

    public void Attack(AttackType attackType) { Attack(attacks[(int)attackType]); }
}
