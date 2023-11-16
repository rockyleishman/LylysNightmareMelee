using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataObject", menuName = "Data/LevelDataObject", order = 0)]
public class LevelData : ScriptableObject
{
    [Header("Difficulty Settings")]
    [SerializeField] public int FinalDifficultyLevel = 10;
    [Space]
    [SerializeField] public float FinalHPMultiplier = 1.0f;
    [SerializeField] public bool UseMaxHPMultiplier = false;
    [SerializeField] public float MaxHPMultiplier = 1.0f;
    internal float NewEnemyHPMultiplier = 1.0f;
    [Space]
    [SerializeField] public float FinalDamageMultiplier = 1.0f;
    [SerializeField] public bool UseMaxDamageMultiplier = false;
    [SerializeField] public float MaxDamageMultiplier = 1.0f;
    internal float NewEnemyDamageMultiplier = 1.0f;
    [Space]
    [SerializeField] public float FinalCooldownMultiplier = 1.0f;
    [SerializeField] public bool UseMaxCooldownMultiplier = true;
    [SerializeField] public float MaxCooldownMultiplier = 1.0f;
    internal float NewEnemyCooldownMultiplier = 1.0f;
    [Space]
    [SerializeField] public float FinalSpeedMultiplier = 1.0f;
    [SerializeField] public bool UseMaxSpeedMultiplier = true;
    [SerializeField] public float MaxSpeedMultiplier = 1.0f;
    internal float NewEnemySpeedMultiplier = 1.0f;
    [Space]
    [SerializeField] public float FinalSpawnFrequencyMultiplier = 1.0f;
    [SerializeField] public bool UseMaxSpawnFrequencyMultiplier = false;
    [SerializeField] public float MaxSpawnFrequencyMultiplier = 1.0f;
    internal float NewEnemySpawnFrequencyMultiplier = 1.0f;
    [Space]
    [SerializeField] public int InitialMaxEnemyCount = 25;
    [SerializeField] public int FinalMaxEnemyCount = 275;
    internal const int AbsoluteMaxEnemyCount = 500; //not exposed to editor for stability security
    internal float MaxEnemyCountUnrounded;
    internal int MaxEnemyCount;
}