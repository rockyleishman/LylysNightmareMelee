using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : Singleton<AttackManager>
{
    //note: "Pillow Attack", "Shield of Light", & "Wishing Well" are attached to the Player object

    [SerializeField] public SpecialAttack SpecialAttackPrefab;
    [SerializeField] public TrailOfAssurance TrailOfAssurancePrefab;
    [SerializeField] public LimitedTimeObject RadiantOrbPrefab; //TODO
    [SerializeField] public ProjectileAttack FlickerOfHopePrefab;
    [SerializeField] public ProjectileAttack SparkOfJoyPrefab;
    [SerializeField] public ProjectileAttack MoonBurstPrefab;
    [SerializeField] public ProjectileAttack FloodOfHopePrefab;
    [SerializeField] public ProjectileAttack SurgeOfJoyPrefab;
    [SerializeField] public ProjectileAttack SunBurstPrefab;
    [SerializeField] public WaveOfRelief WaveOfReliefPrefab;

    private bool _isFloodOfHopeReady;
    private bool _isSurgeOfJoyReady;
    private bool _isSunBurstReady;
    private bool _isWaveOfReliefReady;

    private Vector3 _accumulatedMovement;

    private void Start()
    {
        //ready triggerable secondary attacks
        _isFloodOfHopeReady = true;
        _isSurgeOfJoyReady = true;
        _isSunBurstReady = true;
        _isWaveOfReliefReady = true;

        //start automatic secondary attack coroutines
        StartCoroutine(RadiantOrb());
        StartCoroutine(FlickerOfHope());
        StartCoroutine(SparkOfJoy());
        StartCoroutine(MoonBurst());
        StartCoroutine(PendantOfLife());
    }

    #region public attack events

    public void OnSpecialAttack(Vector3 position)
    {
        PoolManager.Instance.Spawn(SpecialAttackPrefab.name, position, Quaternion.identity);
    }

    public void OnTrailOfAssurance(Vector3 movement)
    {
        if (DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel > 0)
        {
            _accumulatedMovement += movement;

            if (_accumulatedMovement.magnitude >= 1.0f / DataManager.Instance.PlayerDataObject.TOAAttackRange[DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel])
            {
                for (int i = DataManager.Instance.PlayerDataObject.TOAAttackCount[DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel]; i > 0; i--)
                {
                    SpawnTrailOfAssuraceProjectile();
                }

                _accumulatedMovement = Vector3.zero;
            }
        }
    }

    public void OnFloodOfHope(Vector3 position)
    {
        if (_isFloodOfHopeReady && DataManager.Instance.PlayerDataObject.FloodOfHopeLevel > 0)
        {
            for (int i = DataManager.Instance.PlayerDataObject.FloodAttackCount[DataManager.Instance.PlayerDataObject.FloodOfHopeLevel]; i > 0; i--)
            {
                SpawnFloodOfHopeProjectile(i);
            }
            StartCoroutine(FloodOfHopeCooldown());
        }
    }

    public void OnSurgeOfJoy(Vector3 position)
    {
        if (_isSurgeOfJoyReady && DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel > 0)
        {
            for (int i = DataManager.Instance.PlayerDataObject.SurgeAttackCount[DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel]; i > 0; i--)
            {
                SpawnSurgeOfJoyProjectile(i);
            }
            StartCoroutine(SurgeOfJoyCooldown());
        }
    }

    public void OnSunBurst(Vector3 position)
    {
        if (_isSunBurstReady && DataManager.Instance.PlayerDataObject.SunBurstLevel > 0)
        {
            for (int i = DataManager.Instance.PlayerDataObject.SBAttackCount[DataManager.Instance.PlayerDataObject.SunBurstLevel]; i > 0; i--)
            {
                SpawnSunBurstProjectile(Quaternion.Euler(0.0f, 0.0f, 360.0f * i / DataManager.Instance.PlayerDataObject.SBAttackCount[DataManager.Instance.PlayerDataObject.SunBurstLevel]) * -DataManager.Instance.PlayerDataObject.Player.PlayerDirectionObject.transform.up);
            }
            StartCoroutine(SunBurstCooldown());
        }
    }

    public void OnWaveOfRelief(Vector3 position)
    {
        if (_isWaveOfReliefReady && DataManager.Instance.PlayerDataObject.WaveOfReliefLevel > 0)
        {
            PoolManager.Instance.Spawn(WaveOfReliefPrefab.name, position, Quaternion.identity);
            StartCoroutine(WaveOfReliefCooldown());
        }
    }

    #endregion

    #region automatic attack coroutines

    private IEnumerator RadiantOrb()
    {
        while (true)
        {
            if (DataManager.Instance.PlayerDataObject.RadiantOrbLevel > 0)
            {
                SpawnRadiantOrbProjectile();
            }

            yield return new WaitForSeconds(DataManager.Instance.PlayerDataObject.ROAttackCooldown[DataManager.Instance.PlayerDataObject.RadiantOrbLevel]);
        }
    }

    private IEnumerator FlickerOfHope()
    {
        while (true)
        {
            if (DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel > 0)
            {
                SpawnFlickerOfHopeProjectile();
            }

            yield return new WaitForSeconds(DataManager.Instance.PlayerDataObject.FlickAttackCooldown[DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel]);
        }
    }

    private IEnumerator SparkOfJoy()
    {
        while (true)
        {
            if (DataManager.Instance.PlayerDataObject.SparkOfJoyLevel > 0)
            {
                SpawnSparkOfJoyProjectile();
            }

            yield return new WaitForSeconds(DataManager.Instance.PlayerDataObject.SparkAttackCooldown[DataManager.Instance.PlayerDataObject.SparkOfJoyLevel]);
        }
    }

    private IEnumerator MoonBurst()
    {
        while (true)
        {
            if (DataManager.Instance.PlayerDataObject.MoonBurstLevel > 0)
            {
                for (int i = DataManager.Instance.PlayerDataObject.MBAttackCount[DataManager.Instance.PlayerDataObject.MoonBurstLevel]; i > 0; i--)
                {
                    SpawnMoonBurstProjectile(Quaternion.Euler(0.0f, 0.0f, 360.0f * i / DataManager.Instance.PlayerDataObject.MBAttackCount[DataManager.Instance.PlayerDataObject.MoonBurstLevel]) * -DataManager.Instance.PlayerDataObject.Player.PlayerDirectionObject.transform.up);
                }
            }

            yield return new WaitForSeconds(DataManager.Instance.PlayerDataObject.MBAttackCooldown[DataManager.Instance.PlayerDataObject.MoonBurstLevel]);
        }
    }

    private IEnumerator PendantOfLife()
    {
        while (true)
        {
            if (DataManager.Instance.PlayerDataObject.PendantOfLifeLevel > 0)
            {
                DataManager.Instance.PlayerDataObject.Player.HealHP(DataManager.Instance.PlayerDataObject.POLHealingSpeed[DataManager.Instance.PlayerDataObject.PendantOfLifeLevel] * Time.deltaTime);
            }

            yield return null;
        }
    }

    #endregion

    #region attack event cooldowns

    private IEnumerator FloodOfHopeCooldown()
    {
        _isFloodOfHopeReady = false;

        yield return new WaitForSeconds(DataManager.Instance.PlayerDataObject.FloodAttackCooldown[DataManager.Instance.PlayerDataObject.FloodOfHopeLevel] / DataManager.Instance.PlayerDataObject.CooldownDivisor);

        _isFloodOfHopeReady = true;
    }

    private IEnumerator SurgeOfJoyCooldown()
    {
        _isSurgeOfJoyReady = false;

        yield return new WaitForSeconds(DataManager.Instance.PlayerDataObject.SurgeAttackCooldown[DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel] / DataManager.Instance.PlayerDataObject.CooldownDivisor);

        _isSurgeOfJoyReady = true;
    }

    private IEnumerator SunBurstCooldown()
    {
        _isSunBurstReady = false;

        yield return new WaitForSeconds(DataManager.Instance.PlayerDataObject.SBAttackCooldown[DataManager.Instance.PlayerDataObject.SunBurstLevel] / DataManager.Instance.PlayerDataObject.CooldownDivisor);

        _isSunBurstReady = true;
    }

    private IEnumerator WaveOfReliefCooldown()
    {
        _isWaveOfReliefReady = false;

        yield return new WaitForSeconds(DataManager.Instance.PlayerDataObject.WORAttackCooldown[DataManager.Instance.PlayerDataObject.WaveOfReliefLevel] / DataManager.Instance.PlayerDataObject.CooldownDivisor);

        _isWaveOfReliefReady = true;
    }

    #endregion

    #region projectile spawning

    private void SpawnTrailOfAssuraceProjectile()
    {
        //get spawn position deviation
        Vector2 positionDeviation = Random.insideUnitCircle * Mathf.Sqrt(DataManager.Instance.PlayerDataObject.TOAAttackCount[DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel]) * DataManager.Instance.PlayerDataObject.TOASpawnRadiusPerProjectile;

        //fire projectile
        TrailOfAssurance projectile = (TrailOfAssurance)PoolManager.Instance.Spawn(TrailOfAssurancePrefab.name, DataManager.Instance.PlayerDataObject.Player.transform.position + new Vector3(positionDeviation.x, positionDeviation.y, 0.0f), Quaternion.identity);
        projectile.InitProjectile(DataManager.Instance.PlayerDataObject.TOAAttackDamage[DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel],
            DataManager.Instance.PlayerDataObject.TOAAttackCooldown[DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel],
            DataManager.Instance.PlayerDataObject.TOAAttackPierce[DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel]);
    }

    private void SpawnRadiantOrbProjectile()
    {
        
    }

    private void SpawnFlickerOfHopeProjectile()
    {   
        ProjectileAttack projectile = (ProjectileAttack)PoolManager.Instance.Spawn(FlickerOfHopePrefab.name, DataManager.Instance.PlayerDataObject.Player.transform.position, Quaternion.identity);
        projectile.InitProjectile(-DataManager.Instance.PlayerDataObject.Player.PlayerDirectionObject.transform.up,
            DataManager.Instance.PlayerDataObject.FlickProjectileSpeed, 
            DataManager.Instance.PlayerDataObject.FlickAttackDamage[DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel], 
            DataManager.Instance.PlayerDataObject.FlickAttackKnockback[DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel], 
            DataManager.Instance.PlayerDataObject.FlickAttackRange[DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel], 
            DataManager.Instance.PlayerDataObject.FlickAttackPierce[DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel]);
    }

    private void SpawnSparkOfJoyProjectile()
    {
        //get nearest enemy direction
        float nearestEnemyDistance = 1000.0f;
        Vector3 nearestEnemyDirection = -DataManager.Instance.PlayerDataObject.Player.PlayerDirectionObject.transform.up;
        EnemyAIController[] enemies = EnemyManager.Instance.GetComponentsInChildren<EnemyAIController>();
        foreach (EnemyAIController enemy in enemies)
        {
            float enemyDistance = Vector3.Distance(enemy.transform.position, DataManager.Instance.PlayerDataObject.Player.transform.position);
            if (enemyDistance < nearestEnemyDistance)
            {
                nearestEnemyDistance = enemyDistance;
                nearestEnemyDirection = (enemy.transform.position - DataManager.Instance.PlayerDataObject.Player.transform.position).normalized;
            }
        }

        //fire projectile
        ProjectileAttack projectile = (ProjectileAttack)PoolManager.Instance.Spawn(SparkOfJoyPrefab.name, DataManager.Instance.PlayerDataObject.Player.transform.position, Quaternion.identity);
        projectile.InitProjectile(nearestEnemyDirection,
            DataManager.Instance.PlayerDataObject.SparkProjectileSpeed,
            DataManager.Instance.PlayerDataObject.SparkAttackDamage[DataManager.Instance.PlayerDataObject.SparkOfJoyLevel],
            DataManager.Instance.PlayerDataObject.SparkAttackKnockback[DataManager.Instance.PlayerDataObject.SparkOfJoyLevel],
            DataManager.Instance.PlayerDataObject.SparkAttackRange[DataManager.Instance.PlayerDataObject.SparkOfJoyLevel],
            DataManager.Instance.PlayerDataObject.SparkAttackPierce[DataManager.Instance.PlayerDataObject.SparkOfJoyLevel]);
    }

    private void SpawnMoonBurstProjectile(Vector3 direction)
    {
        ProjectileAttack projectile = (ProjectileAttack)PoolManager.Instance.Spawn(MoonBurstPrefab.name, DataManager.Instance.PlayerDataObject.Player.transform.position, Quaternion.identity);
        projectile.InitProjectile(direction,
            DataManager.Instance.PlayerDataObject.MBProjectileSpeed,
            DataManager.Instance.PlayerDataObject.MBAttackDamage[DataManager.Instance.PlayerDataObject.MoonBurstLevel],
            DataManager.Instance.PlayerDataObject.MBAttackKnockback[DataManager.Instance.PlayerDataObject.MoonBurstLevel],
            DataManager.Instance.PlayerDataObject.MBAttackRange[DataManager.Instance.PlayerDataObject.MoonBurstLevel],
            DataManager.Instance.PlayerDataObject.MBAttackPierce[DataManager.Instance.PlayerDataObject.MoonBurstLevel]);
    }

    private void SpawnFloodOfHopeProjectile(int projectileIndex)
    {
        ProjectileAttack projectile = (ProjectileAttack)PoolManager.Instance.Spawn(FloodOfHopePrefab.name, DataManager.Instance.PlayerDataObject.Player.transform.position, Quaternion.identity);
        projectile.InitProjectile(Quaternion.Euler(0.0f, 0.0f, Random.Range(-DataManager.Instance.PlayerDataObject.FloodAngleDeviationPerProjectile * projectileIndex, DataManager.Instance.PlayerDataObject.FloodAngleDeviationPerProjectile * projectileIndex)) * -DataManager.Instance.PlayerDataObject.Player.PlayerDirectionObject.transform.up,
            DataManager.Instance.PlayerDataObject.FloodProjectileSpeed + Random.Range(-DataManager.Instance.PlayerDataObject.FloodSpeedDeviationPerProjectile * projectileIndex, DataManager.Instance.PlayerDataObject.FloodSpeedDeviationPerProjectile * projectileIndex),
            DataManager.Instance.PlayerDataObject.FloodAttackDamage[DataManager.Instance.PlayerDataObject.FloodOfHopeLevel],
            DataManager.Instance.PlayerDataObject.FloodAttackKnockback[DataManager.Instance.PlayerDataObject.FloodOfHopeLevel],
            DataManager.Instance.PlayerDataObject.FloodAttackRange[DataManager.Instance.PlayerDataObject.FloodOfHopeLevel],
            DataManager.Instance.PlayerDataObject.FloodAttackPierce[DataManager.Instance.PlayerDataObject.FloodOfHopeLevel]);
    }

    private void SpawnSurgeOfJoyProjectile(int projectileIndex)
    {
        //get nearest enemy direction
        float nearestEnemyDistance = 1000.0f;
        Vector3 nearestEnemyDirection = -DataManager.Instance.PlayerDataObject.Player.PlayerDirectionObject.transform.up;
        EnemyAIController[] enemies = EnemyManager.Instance.GetComponentsInChildren<EnemyAIController>();
        foreach (EnemyAIController enemy in enemies)
        {
            float enemyDistance = Vector3.Distance(enemy.transform.position, DataManager.Instance.PlayerDataObject.Player.transform.position);
            if (enemyDistance < nearestEnemyDistance)
            {
                nearestEnemyDistance = enemyDistance;
                nearestEnemyDirection = (enemy.transform.position - DataManager.Instance.PlayerDataObject.Player.transform.position).normalized;
            }
        }

        //fire projectile
        ProjectileAttack projectile = (ProjectileAttack)PoolManager.Instance.Spawn(SurgeOfJoyPrefab.name, DataManager.Instance.PlayerDataObject.Player.transform.position, Quaternion.identity);
        projectile.InitProjectile(Quaternion.Euler(0.0f, 0.0f, Random.Range(-DataManager.Instance.PlayerDataObject.FloodAngleDeviationPerProjectile * projectileIndex, DataManager.Instance.PlayerDataObject.FloodAngleDeviationPerProjectile * projectileIndex)) * nearestEnemyDirection,
            DataManager.Instance.PlayerDataObject.SurgeProjectileSpeed + Random.Range(-DataManager.Instance.PlayerDataObject.SurgeSpeedDeviationPerProjectile * projectileIndex, DataManager.Instance.PlayerDataObject.SurgeSpeedDeviationPerProjectile * projectileIndex),
            DataManager.Instance.PlayerDataObject.SurgeAttackDamage[DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel],
            DataManager.Instance.PlayerDataObject.SurgeAttackKnockback[DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel],
            DataManager.Instance.PlayerDataObject.SurgeAttackRange[DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel],
            DataManager.Instance.PlayerDataObject.SurgeAttackPierce[DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel]);
    }

    private void SpawnSunBurstProjectile(Vector3 direction)
    {
        ProjectileAttack projectile = (ProjectileAttack)PoolManager.Instance.Spawn(SunBurstPrefab.name, DataManager.Instance.PlayerDataObject.Player.transform.position, Quaternion.identity);
        projectile.InitProjectile(direction,
            DataManager.Instance.PlayerDataObject.SBProjectileSpeed,
            DataManager.Instance.PlayerDataObject.SBAttackDamage[DataManager.Instance.PlayerDataObject.SunBurstLevel],
            DataManager.Instance.PlayerDataObject.SBAttackKnockback[DataManager.Instance.PlayerDataObject.SunBurstLevel],
            DataManager.Instance.PlayerDataObject.SBAttackRange[DataManager.Instance.PlayerDataObject.SunBurstLevel],
            DataManager.Instance.PlayerDataObject.SBAttackPierce[DataManager.Instance.PlayerDataObject.SunBurstLevel]);
    }

    #endregion
}
