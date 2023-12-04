using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : PoolObject, IHitPoints
{
    private SpriteRenderer _spriteRenderer;
    private Color _defaultColour;
    private Color _flashColour;

    [Header("Death Settings")]
    [SerializeField] public int Points = 10;
    [SerializeField] [Range(0.0f, 1.0f)] public float SpecialCharge = 0.02f;

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
    [SerializeField] [Range(0.0f, 20.0f)] public float BaseWeight = 1.0f;
    private float _weight;

    [Header("Optimization")]
    [SerializeField] [Range(0.001f, 1.0f)] public float TargetAcquisitionTime = 0.1f;
    private Vector3 _target;

    private void Awake()
    {
        //get renderer & colour
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultColour = _spriteRenderer.color;
        _flashColour = Color.Lerp(_defaultColour, Color.white, DataManager.Instance.LevelDataObject.FlashWhiteness);

        //init
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
        _cooldown = BaseCooldown / DataManager.Instance.LevelDataObject.NewEnemyCooldownDivisor;
        _lastAttackTime = -60.0f; //attacks are ready immediately

        //init speed
        _speed = BaseSpeed * DataManager.Instance.LevelDataObject.NewEnemySpeedMultiplier;

        //init weight
        _weight = BaseWeight * DataManager.Instance.LevelDataObject.NewEnemyWeightMultiplier;

        //find target
        StartCoroutine(FindTarget());

        //reset colour
        _spriteRenderer.color = _defaultColour;
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

    public void Knockback(float knockback)
    {
        StartCoroutine(ApplyKnockback(knockback));
    }

    private IEnumerator ApplyKnockback(float knockback)
    {
        float remainingKnockback = knockback;

        while (remainingKnockback > 0.0f)
        {
            float deltaKnockback = knockback * Time.deltaTime / DataManager.Instance.LevelDataObject.KnockbackTime;
            remainingKnockback -= deltaKnockback;

            //apply knockback movement
            transform.Translate((_target - transform.position).normalized * -deltaKnockback);

            yield return null;
        }
    }

    private IEnumerator Flash()
    {
        _spriteRenderer.color = _flashColour;

        yield return new WaitForSeconds(DataManager.Instance.LevelDataObject.FlashTime);

        _spriteRenderer.color = _defaultColour;
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

        if (currentHP <= 0.0f)
        {
            _currentHP = 0.0f;
            OnDeath();
        }
        else
        {
            _currentHP = currentHP;
            StartCoroutine(Flash());
        }

        //trigger effects
        EventManager.Instance.EnemyDamaged.TriggerEvent(transform.position);
    }

    public void OnDeath()
    {
        //charge special
        DataManager.Instance.PlayerDataObject.SpecialCharge += SpecialCharge;

        //TEMP
        if (DataManager.Instance.PlayerDataObject.SpecialCharge > 1.0f)
        {
            Debug.Log("Special: 100%");
        }
        else
        {
            Debug.Log("Special: " + Mathf.FloorToInt(DataManager.Instance.PlayerDataObject.SpecialCharge * 100) + "%");
        }
        //END TEMP

        //trigger effects
        EventManager.Instance.EnemyKilled.TriggerEvent(transform.position);

        //despawn
        OnDespawn();
    }

    #endregion
}
