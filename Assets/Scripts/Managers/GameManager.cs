using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerController Player;
    [SerializeField] GameObject Cursor;

    private void Awake()
    {
        //set references
        DataManager.Instance.PlayerDataObject.Player = Player;
        DataManager.Instance.PlayerDataObject.Cursor = Cursor;

        //init difficulty multipliers
        DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier = 1.0f;
        DataManager.Instance.LevelDataObject.NewEnemyDamageMultiplier = 1.0f;
        DataManager.Instance.LevelDataObject.NewEnemySpeedMultiplier = 1.0f;
        DataManager.Instance.LevelDataObject.NewEnemySpawnFrequencyMultiplier = 1.0f;
        DataManager.Instance.LevelDataObject.MaxEnemyCount = DataManager.Instance.LevelDataObject.InitialMaxEnemyCount;
        DataManager.Instance.LevelDataObject.MaxEnemyCountUnrounded = DataManager.Instance.LevelDataObject.InitialMaxEnemyCount;

        //set initial player stat multipliers
        DataManager.Instance.PlayerDataObject.HPMultiplier = 1.0f;
        DataManager.Instance.PlayerDataObject.MovementSpeedMultiplier = 1.0f;
        DataManager.Instance.PlayerDataObject.DamageMultiplier = 1.0f;
        DataManager.Instance.PlayerDataObject.KnockbackMultiplier = 1.0f;
    }

    public void IncreaseDifficulty()
    {
        if (DataManager.Instance.LevelDataObject.FinalDifficultyLevel > 0)
        {
            //increase new enemy HP multiplier
            DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier += (DataManager.Instance.LevelDataObject.FinalHPMultiplier - 1.0f) / DataManager.Instance.LevelDataObject.FinalDifficultyLevel;
            //clamp new enemy HP multiplier
            if (DataManager.Instance.LevelDataObject.UseMaxHPMultiplier && DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier > DataManager.Instance.LevelDataObject.MaxHPMultiplier)
            {
                DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier = DataManager.Instance.LevelDataObject.MaxHPMultiplier;
            }

            //increase new enemy damage multiplier
            DataManager.Instance.LevelDataObject.NewEnemyDamageMultiplier += (DataManager.Instance.LevelDataObject.FinalDamageMultiplier - 1.0f) / DataManager.Instance.LevelDataObject.FinalDifficultyLevel;
            //clamp new enemy damage multiplier
            if (DataManager.Instance.LevelDataObject.UseMaxDamageMultiplier && DataManager.Instance.LevelDataObject.NewEnemyDamageMultiplier > DataManager.Instance.LevelDataObject.MaxDamageMultiplier)
            {
                DataManager.Instance.LevelDataObject.NewEnemyDamageMultiplier = DataManager.Instance.LevelDataObject.MaxDamageMultiplier;
            }

            //increase new enemy speed multiplier
            DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier += (DataManager.Instance.LevelDataObject.FinalHPMultiplier - 1.0f) / DataManager.Instance.LevelDataObject.FinalDifficultyLevel;
            //clamp new enemy HP multiplier
            if (DataManager.Instance.LevelDataObject.UseMaxHPMultiplier && DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier > DataManager.Instance.LevelDataObject.MaxHPMultiplier)
            {
                DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier = DataManager.Instance.LevelDataObject.MaxHPMultiplier;
            }

            //increase new enemy spawn frequency multiplier
            DataManager.Instance.LevelDataObject.NewEnemySpawnFrequencyMultiplier += (DataManager.Instance.LevelDataObject.FinalSpawnFrequencyMultiplier - 1.0f) / DataManager.Instance.LevelDataObject.FinalDifficultyLevel;
            //clamp new enemy HP multiplier
            if (DataManager.Instance.LevelDataObject.UseMaxSpawnFrequencyMultiplier && DataManager.Instance.LevelDataObject.NewEnemySpawnFrequencyMultiplier > DataManager.Instance.LevelDataObject.MaxSpawnFrequencyMultiplier)
            {
                DataManager.Instance.LevelDataObject.NewEnemySpawnFrequencyMultiplier = DataManager.Instance.LevelDataObject.MaxSpawnFrequencyMultiplier;
            }

            //inrease max enemy count
            DataManager.Instance.LevelDataObject.MaxEnemyCountUnrounded = ((float)DataManager.Instance.LevelDataObject.FinalMaxEnemyCount - (float)DataManager.Instance.LevelDataObject.InitialMaxEnemyCount) / (float)DataManager.Instance.LevelDataObject.FinalDifficultyLevel;
            DataManager.Instance.LevelDataObject.MaxEnemyCount = (int)DataManager.Instance.LevelDataObject.MaxEnemyCountUnrounded;
            //clamp max enemy count
            if (DataManager.Instance.LevelDataObject.MaxEnemyCount > LevelData.AbsoluteMaxEnemyCount)
            {
                DataManager.Instance.LevelDataObject.MaxEnemyCount = LevelData.AbsoluteMaxEnemyCount;
            }
        }
    }
}
