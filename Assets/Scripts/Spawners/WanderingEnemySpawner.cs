using System.Collections;
using UnityEngine;

public class WanderingEnemySpawner : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SpawnTimer());
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(DataManager.Instance.LevelDataObject.FirstSpawnTime);
        while (true)
        {
            SpawnRandomEnemy();
            
            yield return new WaitForSeconds(Random.Range(DataManager.Instance.LevelDataObject.BaseMinSpawnTime, DataManager.Instance.LevelDataObject.BaseMaxSpawnTime) / DataManager.Instance.LevelDataObject.NewEnemySpawnFrequencyMultiplier);
        }
    }

    private void SpawnRandomEnemy()
    {
        Vector2 point = Random.insideUnitCircle.normalized * DataManager.Instance.LevelDataObject.SpawnDistance;
        int index = Random.Range(0, DataManager.Instance.LevelDataObject.WanderingEnemiesWithinThreatLevels[DataManager.Instance.LevelDataObject.CurrentThreatLevel].List.Count);
        EnemyType enemyType = DataManager.Instance.LevelDataObject.WanderingEnemiesWithinThreatLevels[DataManager.Instance.LevelDataObject.CurrentThreatLevel].List[index];

        EnemyManager.Instance.SpawnEnemy(enemyType, transform.position + new Vector3(point.x, point.y, 0.0f));
    }
}
