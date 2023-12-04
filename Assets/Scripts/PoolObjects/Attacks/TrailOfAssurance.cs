using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailOfAssurance : LimitedTimeObject
{
    private List<EnemyAIController> _enemiesHit;
    private int _piercePoints;

    private SpriteRenderer _spriteRenderer;
    [SerializeField] public Sprite[] Sprites;

    private float _damage;

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
    }

    public void InitProjectile(float damage, float cooldown, int pierce)
    {
        //limit duration
        StartCoroutine(LifeTimer(1.0f / cooldown));

        //reset pierce points
        _piercePoints = pierce + DataManager.Instance.PlayerDataObject.PierceIncreaser;

        //set damage
        _damage = damage * DataManager.Instance.PlayerDataObject.DamageMultiplier;

        //restart sprite changing coroutine        
        StartCoroutine(Sparkle());
    }

    private IEnumerator Sparkle()
    {
        while (true)
        {
            //break if this object is inactive
            if (!isActiveAndEnabled)
            {
                break;
            }

            //set sprite
            _spriteRenderer.sprite = Sprites[Random.Range(0, Sprites.Length)];

            yield return new WaitForSeconds(DataManager.Instance.PlayerDataObject.SparkleTime);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        EnemyAIController enemy = other.GetComponent<EnemyAIController>();

        if (enemy != null && !_enemiesHit.Contains(enemy))
        {
            enemy.DamageHP(_damage);

            if (enemy.isActiveAndEnabled)
            {
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
