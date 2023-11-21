using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerController Player;
    [SerializeField] GameObject CursorObject;

    private void Awake()
    {
        //set references
        DataManager.Instance.PlayerDataObject.Player = Player;
        DataManager.Instance.PlayerDataObject.CursorObject = CursorObject;

        //init difficulty multipliers
        DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier = 1.0f;
        DataManager.Instance.LevelDataObject.NewEnemyDamageMultiplier = 1.0f;
        DataManager.Instance.LevelDataObject.NewEnemyCooldownDivisor = 1.0f;
        DataManager.Instance.LevelDataObject.NewEnemySpeedMultiplier = 1.0f;
        DataManager.Instance.LevelDataObject.NewEnemySpawnFrequencyMultiplier = 1.0f;
        DataManager.Instance.LevelDataObject.MaxEnemyCount = DataManager.Instance.LevelDataObject.InitialMaxEnemyCount;
        DataManager.Instance.LevelDataObject.MaxEnemyCountUnrounded = DataManager.Instance.LevelDataObject.InitialMaxEnemyCount;

        //set initial player stat multipliers
        DataManager.Instance.PlayerDataObject.HPMultiplier = 1.0f;
        DataManager.Instance.PlayerDataObject.MovementSpeedMultiplier = 1.0f;
        DataManager.Instance.PlayerDataObject.DamageMultiplier = 1.0f;
        DataManager.Instance.PlayerDataObject.KnockbackMultiplier = 1.0f;

        //spawn initial mirror
        StartCoroutine(SpawnInitialMirror());

        //hide and constain cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private IEnumerator SpawnInitialMirror()
    {
        yield return null;
        
        Vector2 point = Random.insideUnitCircle.normalized * Random.Range(DataManager.Instance.LevelDataObject.MinMirrorSpawnDistance, DataManager.Instance.LevelDataObject.MaxMirrorSpawnDistance);
        MirrorAIController mirror = (MirrorAIController)PoolManager.Instance.Spawn(DataManager.Instance.LevelDataObject.InitialMirror.name, Player.transform.position + new Vector3(point.x, point.y, 0.0f), Quaternion.identity);
        mirror.Init();
    }

    public void IncreaseThreat()
    {
        if (DataManager.Instance.LevelDataObject.CurrentThreatLevel < DataManager.Instance.LevelDataObject.WanderingEnemiesWithinThreatLevels.Count - 1)
        {
            //set threat level (for wandering enemy spawning)
            DataManager.Instance.LevelDataObject.CurrentThreatLevel++;
        }

        if (DataManager.Instance.LevelDataObject.FinalCalculatedThreatLevel > 0)
        {
            //increase new enemy HP multiplier
            DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier += (DataManager.Instance.LevelDataObject.FinalHPMultiplier - 1.0f) / DataManager.Instance.LevelDataObject.FinalCalculatedThreatLevel;
            //clamp new enemy HP multiplier
            if (DataManager.Instance.LevelDataObject.UseMaxHPMultiplier && DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier > DataManager.Instance.LevelDataObject.MaxHPMultiplier)
            {
                DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier = DataManager.Instance.LevelDataObject.MaxHPMultiplier;
            }

            //increase new enemy damage multiplier
            DataManager.Instance.LevelDataObject.NewEnemyDamageMultiplier += (DataManager.Instance.LevelDataObject.FinalDamageMultiplier - 1.0f) / DataManager.Instance.LevelDataObject.FinalCalculatedThreatLevel;
            //clamp new enemy damage multiplier
            if (DataManager.Instance.LevelDataObject.UseMaxDamageMultiplier && DataManager.Instance.LevelDataObject.NewEnemyDamageMultiplier > DataManager.Instance.LevelDataObject.MaxDamageMultiplier)
            {
                DataManager.Instance.LevelDataObject.NewEnemyDamageMultiplier = DataManager.Instance.LevelDataObject.MaxDamageMultiplier;
            }

            //increase new enemy cooldown divisor
            DataManager.Instance.LevelDataObject.NewEnemyCooldownDivisor += (DataManager.Instance.LevelDataObject.FinalCooldownDivisor - 1.0f) / DataManager.Instance.LevelDataObject.FinalCalculatedThreatLevel;
            //clamp new enemy cooldown divisor
            if (DataManager.Instance.LevelDataObject.UseMaxCooldownDivisor && DataManager.Instance.LevelDataObject.NewEnemyCooldownDivisor > DataManager.Instance.LevelDataObject.MaxCooldownDivisor)
            {
                DataManager.Instance.LevelDataObject.NewEnemyCooldownDivisor = DataManager.Instance.LevelDataObject.MaxCooldownDivisor;
            }

            //increase new enemy speed multiplier
            DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier += (DataManager.Instance.LevelDataObject.FinalHPMultiplier - 1.0f) / DataManager.Instance.LevelDataObject.FinalCalculatedThreatLevel;
            //clamp new enemy HP multiplier
            if (DataManager.Instance.LevelDataObject.UseMaxHPMultiplier && DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier > DataManager.Instance.LevelDataObject.MaxHPMultiplier)
            {
                DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier = DataManager.Instance.LevelDataObject.MaxHPMultiplier;
            }

            //increase new enemy spawn frequency multiplier
            DataManager.Instance.LevelDataObject.NewEnemySpawnFrequencyMultiplier += (DataManager.Instance.LevelDataObject.FinalSpawnFrequencyMultiplier - 1.0f) / DataManager.Instance.LevelDataObject.FinalCalculatedThreatLevel;
            //clamp new enemy HP multiplier
            if (DataManager.Instance.LevelDataObject.UseMaxSpawnFrequencyMultiplier && DataManager.Instance.LevelDataObject.NewEnemySpawnFrequencyMultiplier > DataManager.Instance.LevelDataObject.MaxSpawnFrequencyMultiplier)
            {
                DataManager.Instance.LevelDataObject.NewEnemySpawnFrequencyMultiplier = DataManager.Instance.LevelDataObject.MaxSpawnFrequencyMultiplier;
            }

            //inrease max enemy count
            DataManager.Instance.LevelDataObject.MaxEnemyCountUnrounded = ((float)DataManager.Instance.LevelDataObject.FinalMaxEnemyCount - (float)DataManager.Instance.LevelDataObject.InitialMaxEnemyCount) / (float)DataManager.Instance.LevelDataObject.FinalCalculatedThreatLevel;
            DataManager.Instance.LevelDataObject.MaxEnemyCount = (int)DataManager.Instance.LevelDataObject.MaxEnemyCountUnrounded;
            //clamp max enemy count
            if (DataManager.Instance.LevelDataObject.MaxEnemyCount > DataManager.Instance.LevelDataObject.AbsoluteMaxEnemyCount)
            {
                DataManager.Instance.LevelDataObject.MaxEnemyCount = DataManager.Instance.LevelDataObject.AbsoluteMaxEnemyCount;
            }
        }
    }
}
