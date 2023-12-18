using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : LimitedTimeObject
{
    private Animator _animator;
    private List<EnemyAIController> _enemiesHit;

    private void Awake()
    {
        //get animator
        _animator = GetComponent<Animator>();

        //init enemies hit list
        _enemiesHit = new List<EnemyAIController>();
    }

    private void OnEnable()
    {
        //trigger animation
        _animator.SetTrigger("OnAttack");

        //reset enemies hit list
        _enemiesHit.Clear();

        //limit duration
        StartCoroutine(LifeTimer(DataManager.Instance.PlayerDataObject.SpecialAttackDuration));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        EnemyAIController enemy = other.GetComponent<EnemyAIController>();
        MirrorAIController mirror = other.GetComponent<MirrorAIController>();

        if (enemy != null && !_enemiesHit.Contains(enemy))
        {
            enemy.OnDeathNoCharge();
            SoundManager.Instance.PlayHit();
            /*enemy.DamageHP(DataManager.Instance.PlayerDataObject.SpecialAttackDamage * DataManager.Instance.PlayerDataObject.DamageMultiplier);

            if (enemy.isActiveAndEnabled)
            {
                enemy.Knockback(DataManager.Instance.PlayerDataObject.SpecialAttackKnockback * DataManager.Instance.PlayerDataObject.KnockbackMultiplier);
                _enemiesHit.Add(enemy);
            }*/
        }
        else if (mirror != null)
        {
            mirror.Death();
        }
    }
}
