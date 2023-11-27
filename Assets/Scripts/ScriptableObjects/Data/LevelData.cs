using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ListWrapper<T>
{
    public List<T> List;
}

//enemy types
public enum EnemyType
{
    Enemy0,
    Enemy1,
    Enemy2,
    Enemy3,
    Enemy4,
    Enemy5,
    Enemy6,
    Enemy7,
    Special0,
    Special1,
    Special2,
    Special3,
    Boss0,
    Boss1,
    Boss2,
    Boss3
}

[CreateAssetMenu(fileName = "LevelDataObject", menuName = "Data/LevelDataObject", order = 0)]
public class LevelData : ScriptableObject
{
    [Header("Feedback")]
    [SerializeField] public float FlashTime = 0.1f;
    [SerializeField] public float FlashWhiteness = 0.75f;

    [Header("Initial Mirror Prefab")]
    [SerializeField] public MirrorAIController InitialMirror;

    [Header("Enemy Prefabs")]
    [SerializeField] public EnemyAIController Enemy0;
    [SerializeField] public EnemyAIController Enemy1;
    [SerializeField] public EnemyAIController Enemy2;
    [SerializeField] public EnemyAIController Enemy3;
    [SerializeField] public EnemyAIController Enemy4;
    [SerializeField] public EnemyAIController Enemy5;
    [SerializeField] public EnemyAIController Enemy6;
    [SerializeField] public EnemyAIController Enemy7;
    [SerializeField] public EnemyAIController Special0;
    [SerializeField] public EnemyAIController Special1;
    [SerializeField] public EnemyAIController Special2;
    [SerializeField] public EnemyAIController Special3;
    [SerializeField] public EnemyAIController Boss0;
    [SerializeField] public EnemyAIController Boss1;
    [SerializeField] public EnemyAIController Boss2;
    [SerializeField] public EnemyAIController Boss3;

    //if using boundless levels
    //[Space(30)]
    //[Header("Enemy Culling")]
    //[SerializeField] [Range(100.0f, 1000.0f)] public float EnemyCullDistance = 200.0f;

    [Space(30)]
    [Header("Mirror Spawning")]
    [SerializeField] [Range(10.0f, 500.0f)] public float MinMirrorSpawnDistance = 50.0f;
    //if using boundless levels
    [SerializeField] [Range(10.0f, 500.0f)] public float MaxMirrorSpawnDistance = 100.0f;

    [Space(30)]
    [Header("Wandering Enemy Spawning")]
    [SerializeField] public bool UseWanderingEnemySpawning = true;
    [SerializeField] [Range(0.001f, 60.0f)] public float BaseMinSpawnTime = 0.0f;
    [SerializeField] [Range(0.001f, 300.0f)] public float BaseMaxSpawnTime = 10.0f;
    [SerializeField] [Range(10.0f, 100.0f)] public float SpawnDistance = 25.0f;

    [Space(30)]
    [Header("Threat Difficulty Settings")]
    [Header("        Threat Calculations")]
    [SerializeField] [Range(0, 256)] public int FinalCalculatedThreatLevel = 10;
    [Header("        Enemy HP")]
    [SerializeField] [Range(1.0f, 1000.0f)] public float FinalHPMultiplier = 1.0f;
    [SerializeField] public bool UseMaxHPMultiplier = false;
    [SerializeField] [Range(1.0f, 1000.0f)] public float MaxHPMultiplier = 1.0f;
    internal float NewEnemyHPMultiplier;
    [Header("        Enemy Attack Damage")]
    [SerializeField] [Range(1.0f, 1000.0f)] public float FinalDamageMultiplier = 1.0f;
    [SerializeField] public bool UseMaxDamageMultiplier = false;
    [SerializeField] [Range(1.0f, 1000.0f)] public float MaxDamageMultiplier = 1.0f;
    internal float NewEnemyDamageMultiplier;
    [Header("        Enemy Attack Cooldown")]
    [SerializeField] [Range(1.0f, 100.0f)] public float FinalCooldownDivisor = 1.0f;
    [SerializeField] public bool UseMaxCooldownDivisor = true;
    [SerializeField] [Range(1.0f, 100.0f)] public float MaxCooldownDivisor = 1.0f;
    internal float NewEnemyCooldownDivisor;
    [Header("        Enemy Movement Speed")]
    [SerializeField] [Range(1.0f, 10.0f)] public float FinalSpeedMultiplier = 1.0f;
    [SerializeField] public bool UseMaxSpeedMultiplier = true;
    [SerializeField] [Range(1.0f, 10.0f)] public float MaxSpeedMultiplier = 1.0f;
    internal float NewEnemySpeedMultiplier;
    [Header("        Enemy Weight")]
    [SerializeField] [Range(1.0f, 10.0f)] public float FinalWeightMultiplier = 1.0f;
    [SerializeField] public bool UseMaxWeightMultiplier = true;
    [SerializeField] [Range(1.0f, 10.0f)] public float MaxWeightMultiplier = 1.0f;
    internal float NewEnemyWeightMultiplier;
    [SerializeField] [Range(0.0333333f, 5.0f)] public float KnockbackTime = 0.1f;
    [Header("        Enemy Spawn Frequency")]
    [SerializeField] [Range(1.0f, 100.0f)] public float FinalSpawnFrequencyMultiplier = 1.0f;
    [SerializeField] public bool UseMaxSpawnFrequencyMultiplier = false;
    [SerializeField] [Range(1.0f, 100.0f)] public float MaxSpawnFrequencyMultiplier = 1.0f;
    internal float NewEnemySpawnFrequencyMultiplier;
    [Header("        Enemy Special Charge")]
    [SerializeField] [Range(1.0f, 100.0f)] public float FinalSpecialChargeDivisor = 1.0f;
    [SerializeField] public bool UseMaxSpecialChargeDivisor = false;
    [SerializeField] [Range(1.0f, 100.0f)] public float MaxSpecialChargeDivisor = 1.0f;
    internal float NewEnemySpecialChargeDivisor;
    [Header("        Max Enemy Count")]
    [SerializeField] public int InitialMaxEnemyCount = 25;
    [SerializeField] public int FinalMaxEnemyCount = 275;
    [SerializeField] public int AbsoluteMaxEnemyCount = 500;
    internal float MaxEnemyCountUnrounded;
    internal int MaxEnemyCount;
    internal int CurrentEnemyCount;
    [Header("        Threat Progression")]
    [SerializeField] public bool IncreaseThreatAfterMirrorDestruction = true;
    [SerializeField] public bool IncreaseThreatAfterTime = true;
    [SerializeField] [Range(1.0f, 1800.0f)] public float ThreatIncreaseTime = 60.0f;
    [HideInInspector] internal int CurrentThreatLevel;
    [Header("        Wandering Enemies")]
    [SerializeField] public List<ListWrapper<EnemyType>> WanderingEnemiesWithinThreatLevels;
}