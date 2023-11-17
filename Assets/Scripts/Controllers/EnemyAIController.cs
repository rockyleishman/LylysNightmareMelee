using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : PoolObject, IHitPoints
{
    [Header("Death Settings")]
    [SerializeField] public int Points = 10;

    [Header("Stats")]
    [SerializeField] [Range(1.0f, 1000.0f)] public float BaseHP = 10.0f;
    private float _maxHP;
    private float _currentHP;
    [SerializeField] [Range(0.0f, 1000.0f)] public float BaseDamage = 10.0f;
    private float _damage;
    [SerializeField] [Range(0.0f, 5.0f)] public float BaseCooldown = 1.0f;
    private float _cooldown;
    private double _lastAttackTime;
    [SerializeField] [Range(0.0f, 20.0f)] public float BaseSpeed = 3.0f;
    private float _speed;

    [Header("Optimization")]
    [SerializeField] [Range(0.001f, 1.0f)] public float TargetAcquisitionTime = 0.1f;
    private Vector3 _target;

    private void Start()
    {
        //init everything else
        Init();        
    }

    public void Init()
    {
        //init hp
        _maxHP = BaseHP * DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier;
        InitHP();

        //init damage
        _damage = BaseDamage * DataManager.Instance.LevelDataObject.NewEnemyDamageMultiplier;

        //init cooldown
        _cooldown = BaseCooldown * DataManager.Instance.LevelDataObject.NewEnemyCooldownMultiplier;
        _lastAttackTime = -60.0f; //attacks are ready immediately

        //init speed
        _speed = BaseSpeed * DataManager.Instance.LevelDataObject.NewEnemySpeedMultiplier;

        //find target
        StartCoroutine(FindTarget());
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        //apply movement
        transform.Translate((_target - transform.position).normalized * _speed * Time.deltaTime);
    }

    private void Animate()
    {
        //TODO: animation
    }

    private IEnumerator FindTarget()
    {
        while (true)
        {
            _target = DataManager.Instance.PlayerDataObject.Player.transform.position;

            yield return new WaitForSeconds(TargetAcquisitionTime);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.timeAsDouble - _cooldown >= _lastAttackTime)
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.DamageHP(_damage);

                _lastAttackTime = Time.timeAsDouble;
            }
        }
    }

    #region HitPointMethods

    public void InitHP()
    {
        _currentHP = _maxHP;
    }

    public void HealHP(float hp)
    {
        float currentHP = _currentHP + hp;

        if (currentHP > _maxHP)
        {
            _currentHP = _maxHP;
        }
        else
        {
            _currentHP = currentHP;
        }

        //TODO: trigger effect
    }

    public void DamageHP(float hp)
    {
        float currentHP = _currentHP - hp;

        if (currentHP < 0.0f)
        {
            _currentHP = 0.0f;
            OnDeath();
        }
        else
        {
            _currentHP = currentHP;
        }

        //TODO: trigger effect
    }

    public void OnDeath()
    {
        //despawn
        OnDespawn();
    }

    #endregion
}
