using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    private void Start()
    {
        //set initial enemy count
        DataManager.Instance.LevelDataObject.CurrentEnemyCount = 0;
    }

    public void SpawnEnemy(EnemyType enemyType, Vector3 position)
    {
        string enemyName = null;

        switch (enemyType)
        {
            case EnemyType.Enemy0:
                if (DataManager.Instance.LevelDataObject.CurrentEnemyCount < DataManager.Instance.LevelDataObject.MaxEnemyCount)
                {
                    enemyName = DataManager.Instance.LevelDataObject.Enemy0.name;
                }                
                break;

            case EnemyType.Enemy1:
                if (DataManager.Instance.LevelDataObject.CurrentEnemyCount < DataManager.Instance.LevelDataObject.MaxEnemyCount)
                {
                    enemyName = DataManager.Instance.LevelDataObject.Enemy1.name;
                }
                break;

            case EnemyType.Enemy2:
                if (DataManager.Instance.LevelDataObject.CurrentEnemyCount < DataManager.Instance.LevelDataObject.MaxEnemyCount)
                {
                    enemyName = DataManager.Instance.LevelDataObject.Enemy2.name;
                }
                break;

            case EnemyType.Enemy3:
                if (DataManager.Instance.LevelDataObject.CurrentEnemyCount < DataManager.Instance.LevelDataObject.MaxEnemyCount)
                {
                    enemyName = DataManager.Instance.LevelDataObject.Enemy3.name;
                }
                break;

            case EnemyType.Enemy4:
                if (DataManager.Instance.LevelDataObject.CurrentEnemyCount < DataManager.Instance.LevelDataObject.MaxEnemyCount)
                {
                    enemyName = DataManager.Instance.LevelDataObject.Enemy4.name;
                }
                break;

            case EnemyType.Enemy5:
                if (DataManager.Instance.LevelDataObject.CurrentEnemyCount < DataManager.Instance.LevelDataObject.MaxEnemyCount)
                {
                    enemyName = DataManager.Instance.LevelDataObject.Enemy5.name;
                }
                break;

            case EnemyType.Enemy6:
                if (DataManager.Instance.LevelDataObject.CurrentEnemyCount < DataManager.Instance.LevelDataObject.MaxEnemyCount)
                {
                    enemyName = DataManager.Instance.LevelDataObject.Enemy6.name;
                }
                break;

            case EnemyType.Enemy7:
                if (DataManager.Instance.LevelDataObject.CurrentEnemyCount < DataManager.Instance.LevelDataObject.MaxEnemyCount)
                {
                    enemyName = DataManager.Instance.LevelDataObject.Enemy7.name;
                }
                break;

            case EnemyType.Special0:
                enemyName = DataManager.Instance.LevelDataObject.Special0.name;
                break;

            case EnemyType.Special1:
                enemyName = DataManager.Instance.LevelDataObject.Special1.name;
                break;

            case EnemyType.Special2:
                enemyName = DataManager.Instance.LevelDataObject.Special2.name;
                break;

            case EnemyType.Special3:
                enemyName = DataManager.Instance.LevelDataObject.Special3.name;
                break;

            case EnemyType.Boss0:
                enemyName = DataManager.Instance.LevelDataObject.Boss0.name;
                break;

            case EnemyType.Boss1:
                enemyName = DataManager.Instance.LevelDataObject.Boss1.name;
                break;

            case EnemyType.Boss2:
                enemyName = DataManager.Instance.LevelDataObject.Boss2.name;
                break;

            case EnemyType.Boss3:
                enemyName = DataManager.Instance.LevelDataObject.Boss3.name;
                break;

            default:
                //no spawn
                break;
        }
        
        if (enemyName != null)
        {
            EnemyAIController enemy = (EnemyAIController)PoolManager.Instance.Spawn(enemyName, position, Quaternion.identity);
            enemy.transform.SetParent(transform);
            enemy.Init();
        }

        //set enemy count
        DataManager.Instance.LevelDataObject.CurrentEnemyCount = GetComponentsInChildren<EnemyAIController>().Length;
    }
}
