using System.Collections.Generic;
using UnityEngine;

//secondary attack types
public enum SecondaryAttack
{
    trailOfAssurace,
    shieldOfLight,
    wishingWell,
    radiantOrb,
    flickerOfHope,
    sparkOfJoy,
    moonBurst,
    floodOfHope,
    surgeOfJoy,
    sunBurst,
    waveOfRelief,
    pendantOfLife
}

//stat types
public enum StatModifier
{
    hitPoints,
    movementSpeed,
    damage,
    knockback,
    range,
    cooldown,
    count,
    pierce
}

[CreateAssetMenu(fileName = "PlayerDataObject", menuName = "Data/PlayerDataObject", order = 0)]
public class PlayerData : ScriptableObject
{
    internal PlayerController Player;
    internal GameObject CursorObject;

    [Header("Base Stats")]
    [SerializeField] public float DefaultCursorDistance = 5.0f;
    [SerializeField] public float MaxHP = 100.0f;
    internal float CurrentHP;
    [SerializeField] public float MovementSpeed = 5.0f;
    [SerializeField] public int MaximumNumberOfSecondaryAttacks = 4;
    internal List<SecondaryAttack> SecondaryAttacksAquired;

    [Header("Stat Modifiers (Max 0 is Infinity)")]
    [SerializeField] public float HPMultiplierIncPerLevel = 0.1f;
    [SerializeField] public float MaxHPMultiplier = 0.0f;
    internal float HPMultiplier;
    [SerializeField] public float MovementSpeedMultiplierIncPerLevel = 0.1f;
    [SerializeField] public float MaxMovementSpeedMultiplier = 2.0f;
    internal float MovementSpeedMultiplier;
    [SerializeField] public float DamageMultiplierIncPerLevel = 0.1f;
    [SerializeField] public float MaxDamageMultiplier = 0.0f;
    internal float DamageMultiplier;
    [SerializeField] public float KnockbackMultiplierIncPerLevel = 0.1f;
    [SerializeField] public float MaxKnockbackMultiplier = 0.0f;
    internal float KnockbackMultiplier;
    [SerializeField] public float RangeMultiplierIncPerLevel = 0.1f;
    [SerializeField] public float MaxRangeMultiplier = 0.0f;
    internal float RangeMultiplier;
    [SerializeField] public float CooldownDivisorIncPerLevel = 0.1f;
    [SerializeField] public float MaxCooldownDivisor = 0.0f;
    internal float CooldownDivisor;
    [SerializeField] public int CountIncreaserIncPerLevel = 1;
    [SerializeField] public int MaxCountIncreaser = 5;
    internal int CountIncreaser;
    [SerializeField] public int PierceIncreaserIncPerLevel = 1;
    [SerializeField] public int MaxPierceIncreaser = 5;
    internal int PierceIncreaser;

    [Header("Pillow Attack")]
    [SerializeField] public float PillowAttackDamage = 10.0f;
    [SerializeField] public float PillowAttackKnockback = 10.0f;
    [SerializeField] public float PillowAttackDuration = 0.25f;
    [SerializeField] public float PillowAttackCooldown = 0.1f;
    [SerializeField] public float PillowAttackContinuousCooldown = 0.375f;

    [Header("Special Attack")]
    [SerializeField] public float SpecialAttackDamage = 1000.0f;
    [SerializeField] public float SpecialAttackKnockback = 10.0f;
    [SerializeField] public float SpecialAttackDuration = 3.0f;
    internal float SpecialCharge;

    [Header("Secondary Attack: \"Trail of Assurance\"")]
    [SerializeField] public float SparkleTime = 0.25f;
    [SerializeField] public float TOASpawnRadiusPerProjectile = 0.25f;
    [SerializeField] public float[] TOAAttackDamage = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] TOAAttackRange = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f }; //inverse distance traveled to trigger
    [SerializeField] public float[] TOAAttackCooldown = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f }; //inverse time to despawn
    [SerializeField] public int[] TOAAttackCount = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    [SerializeField] public int[] TOAAttackPierce = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    internal int TrailOfAssuranceLevel;

    [Header("Secondary Attack: \"Shield of Light\"")]
    [SerializeField] public float[] SOLAttackDamage = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] SOLAttackRange = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] SOLAttackCooldown = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    internal int ShieldOfLightLevel;

    [Header("Secondary Attack: \"Wishing Well\"")]
    [SerializeField] public float[] WWAttackDamage = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] WWAttackKnockback = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] WWAttackRange = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] WWAttackCooldown = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public int[] WWAttackCount = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    internal int WishingWellLevel;

    [Header("Secondary Attack: \"Radiant Orb\"")]
    [SerializeField] public float ROProjectileSpeed = 10.0f;
    [SerializeField] public float[] ROAttackDamage = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] ROAttackKnockback = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] ROAttackRange = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] ROAttackCooldown = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    internal int RadiantOrbLevel;

    [Header("Secondary Attack: \"Flicker of Hope\"")]
    [SerializeField] public float FlickProjectileSpeed = 10.0f;
    [SerializeField] public float[] FlickAttackDamage = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] FlickAttackKnockback = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] FlickAttackRange = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] FlickAttackCooldown = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public int[] FlickAttackPierce = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    internal int FlickerOfHopeLevel;

    [Header("Secondary Attack: \"Spark of Joy\"")]
    [SerializeField] public float SparkProjectileSpeed = 10.0f;
    [SerializeField] public float[] SparkAttackDamage = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] SparkAttackKnockback = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] SparkAttackRange = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] SparkAttackCooldown = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public int[] SparkAttackPierce = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    internal int SparkOfJoyLevel;

    [Header("Secondary Attack: \"Moon-Burst\"")]
    [SerializeField] public float MBProjectileSpeed = 10.0f;
    [SerializeField] public float[] MBAttackDamage = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] MBAttackKnockback = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] MBAttackRange = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] MBAttackCooldown = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public int[] MBAttackCount = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    [SerializeField] public int[] MBAttackPierce = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    internal int MoonBurstLevel;

    [Header("Secondary Attack: \"Flood of Hope\"")]
    [SerializeField] public float FloodProjectileSpeed = 10.0f;
    [SerializeField] public float FloodAngleDeviationPerProjectile = 1.0f;
    [SerializeField] public float FloodSpeedDeviationPerProjectile = 0.1f;
    [SerializeField] public float[] FloodAttackDamage = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] FloodAttackKnockback = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] FloodAttackRange = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] FloodAttackCooldown = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public int[] FloodAttackCount = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    [SerializeField] public int[] FloodAttackPierce = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    internal int FloodOfHopeLevel;

    [Header("Secondary Attack: \"Surge of Joy\"")]
    [SerializeField] public float SurgeProjectileSpeed = 10.0f;
    [SerializeField] public float SurgeAngleDeviationPerProjectile = 1.0f;
    [SerializeField] public float SurgeSpeedDeviationPerProjectile = 0.1f;
    [SerializeField] public float[] SurgeAttackDamage = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] SurgeAttackKnockback = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] SurgeAttackRange = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] SurgeAttackCooldown = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public int[] SurgeAttackCount = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    [SerializeField] public int[] SurgeAttackPierce = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    internal int SurgeOfJoyLevel;

    [Header("Secondary Attack: \"Sun-Burst\"")]
    [SerializeField] public float SBProjectileSpeed = 10.0f;
    [SerializeField] public float[] SBAttackDamage = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] SBAttackKnockback = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] SBAttackRange = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] SBAttackCooldown = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public int[] SBAttackCount = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    [SerializeField] public int[] SBAttackPierce = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    internal int SunBurstLevel;

    [Header("Secondary Attack: \"Wave of Relief\"")]
    [SerializeField] public float WORProjectileSpeed = 10.0f;
    [SerializeField] public float[] WORAttackDamage = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] WORAttackKnockback = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] WORAttackRange = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public float[] WORAttackCooldown = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField] public int[] WORAttackPierce = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    internal int WaveOfReliefLevel;

    [Header("Secondary Attack: \"Pendant of Life\"")]
    [SerializeField] public float[] POLHealingSpeed = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    internal int PendantOfLifeLevel;
}
