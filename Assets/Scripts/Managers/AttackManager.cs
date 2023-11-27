using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : Singleton<AttackManager>
{
    [SerializeField] public SpecialAttack SpecialAttackPrefab;

    public void OnSpecialAttack(MonoBehaviour attacker, MonoBehaviour attackTarget)
    {
        PoolManager.Instance.Spawn(SpecialAttackPrefab.name, attacker.transform.position, Quaternion.identity);
    }
}
