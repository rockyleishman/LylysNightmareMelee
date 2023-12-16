using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorAIController : PoolObject
{
    [Header("Death Settings")]
    [SerializeField] public int Score = 1000;
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

    public void Death()
    {
        //add score
        ScoreManager.Instance.AddScore(Score);

        //check for victory
        DataManager.Instance.PlayerDataObject.MirrorsDestroyed++;
        if (DataManager.Instance.PlayerDataObject.MirrorsDestroyed >= DataManager.Instance.LevelDataObject.MirrorsToDestroy)
        {
            EventManager.Instance.VictoryTriggered.TriggerEvent(transform.position);
        }

        //upgrade
        EventManager.Instance.TriggerUpgrade.TriggerEvent(transform.position);

        //spawn new mirrors
        for (int i = AmountOfMirrorsToSpawnOnDeath[Random.Range(0, AmountOfMirrorsToSpawnOnDeath.Length)]; i > 0; i--)
        {
            //select mirror to spawn
            string mirrorName = MirrorsToSpawnOnDeath[Random.Range(0, MirrorsToSpawnOnDeath.Length)].name;

            //find point to spawn mirror at
            Vector3 point;
            do
            {
                point = new Vector3(Random.Range(-DataManager.Instance.LevelDataObject.MirrorSpawningBounds.x / 2.0f, DataManager.Instance.LevelDataObject.MirrorSpawningBounds.x / 2.0f), Random.Range(-DataManager.Instance.LevelDataObject.MirrorSpawningBounds.y / 2.0f, DataManager.Instance.LevelDataObject.MirrorSpawningBounds.y / 2.0f), 0.0f);
            }
            while (Vector3.Distance(point, DataManager.Instance.PlayerDataObject.Player.transform.position) < DataManager.Instance.LevelDataObject.MinMirrorSpawnDistance);

            //spawn mirror
            MirrorAIController mirror = (MirrorAIController)PoolManager.Instance.Spawn(mirrorName, point, Quaternion.identity);
            mirror.Init();

            //Look at new Mirror
            EventManager.Instance.LookAtMirrorTriggered.TriggerEvent(mirror.transform.position);
        }

        //increase threat
        EventManager.Instance.IncreaseThreat.TriggerEvent(transform.position);

        //end coroutines
        StopAllCoroutines();

        //TODO: effects

        //despawn
        OnDespawn();
    }
}
