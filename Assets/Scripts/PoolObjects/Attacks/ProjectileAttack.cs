using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : LimitedTimeObject
{
    private List<EnemyAIController> _enemiesHit;
    private int _piercePoints;

    private SpriteRenderer _spriteRenderer;
    [SerializeField] public Sprite[] Sprites;

    private Vector3 _velocity;

    private float _damage;
    private float _knockback;

    private void Awake()
    {
        //init enemies hit list
        _enemiesHit = new List<EnemyAIController>();

        //get sprite renderer
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        //reset enemies hit list
        _enemiesHit.Clear();
        
        //reset position
        transform.position = DataManager.Instance.PlayerDataObject.Player.transform.position;

        //set sprite
        _spriteRenderer.sprite = Sprites[Random.Range(0, Sprites.Length)];
    }

    public void InitProjectile(Vector3 direction, float speed, float damage, float knockback, float range, int pierce)
    {
        //limit duration
        StartCoroutine(LifeTimer(range / speed));

        //reset pierce points
        _piercePoints = pierce + DataManager.Instance.PlayerDataObject.PierceIncreaser;

        //set velocity
        _velocity = direction * speed;

        //set damage
        _damage = damage * DataManager.Instance.PlayerDataObject.DamageMultiplier;

        //set knockback
        _knockback = knockback * DataManager.Instance.PlayerDataObject.KnockbackMultiplier;
    }

    private void Update()
    {
        //move
        transform.Translate(_velocity * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        EnemyAIController enemy = other.GetComponent<EnemyAIController>();

        if (enemy != null && !_enemiesHit.Contains(enemy))
        {
            enemy.DamageHP(_damage);

            if (enemy.isActiveAndEnabled)
            {
                enemy.Knockback(_knockback);
                _enemiesHit.Add(enemy);
            }

            //check if can hit more enemies
            _piercePoints--;
            if (_piercePoints <= 0)
            {
                OnDespawn();
            }
        }
    }
}
