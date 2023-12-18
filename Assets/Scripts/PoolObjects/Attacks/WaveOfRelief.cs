using System.Collections.Generic;
using UnityEngine;

public class WaveOfRelief : LimitedTimeObject
{
    private List<EnemyAIController> _enemiesHit;
    private int _piercePoints;

    private SpriteRenderer _spriteRenderer;
    [SerializeField] public Sprite Sprite000;
    [SerializeField] public Sprite Sprite225;
    [SerializeField] public Sprite Sprite450;
    [SerializeField] public Sprite Sprite675;

    private Vector3 _velocity;

    private void Awake()
    {
        //init enemies hit list
        _enemiesHit = new List<EnemyAIController>();

        //get sprite renderer
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        //limit duration
        StartCoroutine(LifeTimer(DataManager.Instance.PlayerDataObject.WORAttackRange[DataManager.Instance.PlayerDataObject.WaveOfReliefLevel] / DataManager.Instance.PlayerDataObject.WORProjectileSpeed));

        //reset enemies hit list
        _enemiesHit.Clear();

        //reset pierce points
        _piercePoints = DataManager.Instance.PlayerDataObject.WORAttackPierce[DataManager.Instance.PlayerDataObject.WaveOfReliefLevel] + DataManager.Instance.PlayerDataObject.PierceIncreaser;

        //reset position
        transform.position = DataManager.Instance.PlayerDataObject.Player.transform.position;

        //set velocity
        _velocity = -DataManager.Instance.PlayerDataObject.Player.PlayerDirectionObject.transform.up * DataManager.Instance.PlayerDataObject.WORProjectileSpeed;

        //set sprite
        int rotationValue = Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.Player.PlayerDirectionObject.transform.eulerAngles.z / 22.5f);
        switch (rotationValue % 4)
        {
            case 1:
                _spriteRenderer.sprite = Sprite225;
                break;

            case 2:
                _spriteRenderer.sprite = Sprite450;
                break;

            case 3:
                _spriteRenderer.sprite = Sprite675;
                break;

            case 0:
            default:
                _spriteRenderer.sprite = Sprite000;
                break;
        }

        //rotate sprite
        _spriteRenderer.gameObject.transform.eulerAngles = new Vector3(0, 0, rotationValue / 4 * 90);
        
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
            enemy.DamageHP(DataManager.Instance.PlayerDataObject.WORAttackDamage[DataManager.Instance.PlayerDataObject.WaveOfReliefLevel] * DataManager.Instance.PlayerDataObject.DamageMultiplier);

            if (enemy.isActiveAndEnabled)
            {
                enemy.Knockback(DataManager.Instance.PlayerDataObject.WORAttackKnockback[DataManager.Instance.PlayerDataObject.WaveOfReliefLevel] * DataManager.Instance.PlayerDataObject.KnockbackMultiplier);
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
