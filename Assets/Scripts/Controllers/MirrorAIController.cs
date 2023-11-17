using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorAIController : PoolObject
{
    [Header("Death Settings")]
    [SerializeField] public int Points = 1000;
    [Space]
    [SerializeField] public MirrorAIController[] MirrorsToSpawnOnDeath;
    [SerializeField] [Range(1, 10)] public int[] AmountOfMirrorsToSpawnOnDeath;

    [Space(30)]
    [Header("Mirror Settings")]
    [SerializeField] public EnemyType[] Enemies;
    [Space]
    [SerializeField] public int MinEnemiesPerWave = 3;
    [SerializeField] public int MaxEnemiesPerWave = 5;
    [Space]
    [SerializeField] public float MinWaveSpawnTime = 10.0f;
    [SerializeField] public float MaxWaveSpawnTime = 15.0f;

    //TODO: remove start when mirror spawning implemented
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        StartCoroutine(WaveTimer());
    }

    private IEnumerator WaveTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(MinWaveSpawnTime, MaxWaveSpawnTime) / DataManager.Instance.LevelDataObject.NewEnemySpawnFrequencyMultiplier);

            for (int i = Random.Range(MinEnemiesPerWave, MaxEnemiesPerWave + 1); i > 0; i--)
            {
                SpawnRandomEnemy();
            }
        }
    }

    private void SpawnRandomEnemy()
    {
        Vector2 point = Random.insideUnitCircle.normalized * 0.001f;
        int index = Random.Range(0, Enemies.Length);
        EnemyType enemyType = Enemies[index];

        EnemyManager.Instance.SpawnEnemy(enemyType, transform.position + new Vector3(point.x, point.y, 0.0f));
    }

    private void OnDeath()
    {
        //TODO: spawn new mirrors

        //end coroutines
        StopAllCoroutines();

        //despawn
        OnDespawn();
    }
}
