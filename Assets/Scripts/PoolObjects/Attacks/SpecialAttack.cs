using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : LimitedTimeObject
{
    private Animator _animator;
    private List<EnemyAIController> _enemiesHit;
    private bool _mirrorDestroyed;

    private void Awake()
    {
        //get animator
        _animator = GetComponent<Animator>();

        //init enemies hit list
        _enemiesHit = new List<EnemyAIController>();

        //allow one mirror to be destroyed
        _mirrorDestroyed = false;
    }

    private void OnEnable()
    {
        //trigger animation
        _animator.SetTrigger("OnAttack");

        //reset enemies hit list
        _enemiesHit.Clear();

        //reallow mirror destruction
        _mirrorDestroyed = false;

        //limit duration
        StartCoroutine(LifeTimer(DataManager.Instance.PlayerDataObject.SpecialAttackDuration));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        EnemyAIController enemy = other.GetComponent<EnemyAIController>();
        MirrorAIController mirror = other.GetComponent<MirrorAIController>();

        if (enemy != null && !_enemiesHit.Contains(enemy))
        {
            //destroy enemies
            enemy.OnDeathNoCharge();
            
            //TODO: apply damage and knockback to bosses (if bosses get implemented)
        }
        else if (mirror != null && !_mirrorDestroyed)
        {
            _mirrorDestroyed = true;
            mirror.Death();
        }
    }
}
