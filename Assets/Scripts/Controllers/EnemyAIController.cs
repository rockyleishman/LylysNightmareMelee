using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : PoolObject, IHitPoints
{
    [Header("Stats")]
    [SerializeField] [Range(1.0f, 1000.0f)] public float BaseHP = 10.0f;
    private float _maxHP;
    private float _currentHP;
    [SerializeField] [Range(0.0f, 1000.0f)] public float BaseDamage = 10.0f;
    private float _actualDamage;
    [SerializeField] [Range(0.0f, 20.0f)] public float BaseSpeed = 3.0f;
    private float _actualSpeed;

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
        _actualDamage = BaseDamage * DataManager.Instance.LevelDataObject.NewEnemyDamageMultiplier;

        //init speed
        _actualSpeed = BaseSpeed * DataManager.Instance.LevelDataObject.NewEnemySpeedMultiplier;

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
        transform.Translate((_target - transform.position).normalized * _actualSpeed * Time.deltaTime);
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
        PlayerController player = collision.collider.GetComponent<PlayerController>();
        if (player != null)
        {
            //TODO: damage player
            Debug.Log("Ouch!");
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
        OnDespawn();
    }

    #endregion
}
