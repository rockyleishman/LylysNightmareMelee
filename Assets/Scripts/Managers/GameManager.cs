using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerController Player;
    [SerializeField] GameObject CursorObject;

    [SerializeField] GameObject GameOverUI;
    [SerializeField] GameObject VictoryUI;

    private bool _isGameFinished;

    private void Awake()
    {
        //set references
        DataManager.Instance.PlayerDataObject.Player = Player;
        DataManager.Instance.PlayerDataObject.CursorObject = CursorObject;

        //game is not finished
        _isGameFinished = false;
        GameOverUI.SetActive(false);
        VictoryUI.SetActive(false);

        //init difficulty multipliers
        DataManager.Instance.LevelDataObject.NewEnemyHPMultiplier = 1.0f;
        DataManager.Instance.LevelDataObject.NewEnemyDamageMultiplier = 1.0f;
        DataManager.Instance.LevelDataObject.NewEnemyCooldownDivisor = 1.0f;
        DataManager.Instance.LevelDataObject.NewEnemySpeedMultiplier = 1.0f;
        DataManager.Instance.LevelDataObject.NewEnemySpawnFrequencyMultiplier = 1.0f;
        DataManager.Instance.LevelDataObject.NewEnemySpecialChargeDivisor = 1.0f;
        DataManager.Instance.LevelDataObject.MaxEnemyCount = DataManager.Instance.LevelDataObject.InitialMaxEnemyCount;
        DataManager.Instance.LevelDataObject.MaxEnemyCountUnrounded = DataManager.Instance.LevelDataObject.InitialMaxEnemyCount;

        //set initial player stat multipliers
        DataManager.Instance.PlayerDataObject.HPMultiplier = 1.0f;
        DataManager.Instance.PlayerDataObject.MovementSpeedMultiplier = 1.0f;
        DataManager.Instance.PlayerDataObject.DamageMultiplier = 1.0f;
        DataManager.Instance.PlayerDataObject.KnockbackMultiplier = 1.0f;
        DataManager.Instance.PlayerDataObject.RangeMultiplier = 1.0f;
        DataManager.Instance.PlayerDataObject.CooldownDivisor = 1.0f;
        DataManager.Instance.PlayerDataObject.CountIncreaser = 0;
        DataManager.Instance.PlayerDataObject.PierceIncreaser = 0;

        //set secondary attack levels to 0
        DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired = new List<SecondaryAttack>();
        DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel = 0;
        DataManager.Instance.PlayerDataObject.ShieldOfLightLevel = 0; //TODO
        DataManager.Instance.PlayerDataObject.WishingWellLevel = 0; //TODO
        DataManager.Instance.PlayerDataObject.RadiantOrbLevel = 0; //TODO
        DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel = 0;
        DataManager.Instance.PlayerDataObject.SparkOfJoyLevel = 0;
        DataManager.Instance.PlayerDataObject.MoonBurstLevel = 0;
        DataManager.Instance.PlayerDataObject.FloodOfHopeLevel = 0;
        DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel = 0;
        DataManager.Instance.PlayerDataObject.SunBurstLevel = 0;
        DataManager.Instance.PlayerDataObject.WaveOfReliefLevel = 0;
        DataManager.Instance.PlayerDataObject.PendantOfLifeLevel = 0;

        //set initial special charge
        DataManager.Instance.PlayerDataObject.SpecialCharge = 0.0f;

        //set initial score (0)
        DataManager.Instance.PlayerDataObject.Score = 0;
        DataManager.Instance.PlayerDataObject.MirrorsDestroyed = 0;

        //spawn initial mirror
        StartCoroutine(SpawnInitialMirror());

        //start timed threat increase
        StartCoroutine(TimedThreatIncrease());

        //hide and constain cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        //play bg music
        SoundManager.Instance.PlayBG();
    }

    private void Update()
    {
        //prevent play if game is finished
        if (_isGameFinished)
        {
            Time.timeScale = 0.0f;
        }
    }

    public void OnGameOver()
    {
        //disable player controls
        DataManager.Instance.PlayerDataObject.Player.GetComponent<PlayerInput>().SwitchCurrentActionMap("ForcedMenu");

        _isGameFinished = true;
        Cursor.visible = true;
        GameOverUI.SetActive(true);
    }

    public void OnVictory()
    {
        //disable player controls
        DataManager.Instance.PlayerDataObject.Player.GetComponent<PlayerInput>().SwitchCurrentActionMap("ForcedMenu");

        _isGameFinished = true;
        Cursor.visible = true;
        VictoryUI.SetActive(true);
    }

    public void UpgradeStat(StatModifier statModifier)
    {
        switch (statModifier)
        {
            case StatModifier.hitPoints:
                float proposedHPMultiplier = DataManager.Instance.PlayerDataObject.HPMultiplier += DataManager.Instance.PlayerDataObject.HPMultiplierIncPerLevel;
                if (DataManager.Instance.PlayerDataObject.MaxHPMultiplier > 0.0f && proposedHPMultiplier > DataManager.Instance.PlayerDataObject.MaxHPMultiplier)
                {
                    proposedHPMultiplier = DataManager.Instance.PlayerDataObject.MaxHPMultiplier;
                }
                DataManager.Instance.PlayerDataObject.HPMultiplier = proposedHPMultiplier;
                break;

            case StatModifier.movementSpeed:
                float proposedMovementSpeedMultiplier = DataManager.Instance.PlayerDataObject.MovementSpeedMultiplier += DataManager.Instance.PlayerDataObject.MovementSpeedMultiplierIncPerLevel;
                if (DataManager.Instance.PlayerDataObject.MaxMovementSpeedMultiplier > 0.0f && proposedMovementSpeedMultiplier > DataManager.Instance.PlayerDataObject.MaxMovementSpeedMultiplier)
                {
                    proposedMovementSpeedMultiplier = DataManager.Instance.PlayerDataObject.MaxMovementSpeedMultiplier;
                }
                DataManager.Instance.PlayerDataObject.MovementSpeedMultiplier = proposedMovementSpeedMultiplier;
                break;

            case StatModifier.damage:
                float proposedDamageMultiplier = DataManager.Instance.PlayerDataObject.DamageMultiplier += DataManager.Instance.PlayerDataObject.DamageMultiplierIncPerLevel;
                if (DataManager.Instance.PlayerDataObject.MaxDamageMultiplier > 0.0f && proposedDamageMultiplier > DataManager.Instance.PlayerDataObject.MaxDamageMultiplier)
                {
                    proposedDamageMultiplier = DataManager.Instance.PlayerDataObject.MaxDamageMultiplier;
                }
                DataManager.Instance.PlayerDataObject.DamageMultiplier = proposedDamageMultiplier;
                break;

            case StatModifier.knockback:
                float proposedKnockbackMultiplier = DataManager.Instance.PlayerDataObject.KnockbackMultiplier += DataManager.Instance.PlayerDataObject.KnockbackMultiplierIncPerLevel;
                if (DataManager.Instance.PlayerDataObject.MaxKnockbackMultiplier > 0.0f && proposedKnockbackMultiplier > DataManager.Instance.PlayerDataObject.MaxKnockbackMultiplier)
                {
                    proposedKnockbackMultiplier = DataManager.Instance.PlayerDataObject.MaxKnockbackMultiplier;
                }
                DataManager.Instance.PlayerDataObject.KnockbackMultiplier = proposedKnockbackMultiplier;
                break;

            case StatModifier.range:
                float proposedRangeMultiplier = DataManager.Instance.PlayerDataObject.RangeMultiplier += DataManager.Instance.PlayerDataObject.RangeMultiplierIncPerLevel;
                if (DataManager.Instance.PlayerDataObject.MaxRangeMultiplier > 0.0f && proposedRangeMultiplier > DataManager.Instance.PlayerDataObject.MaxRangeMultiplier)
                {
                    proposedRangeMultiplier = DataManager.Instance.PlayerDataObject.MaxRangeMultiplier;
                }
                DataManager.Instance.PlayerDataObject.RangeMultiplier = proposedRangeMultiplier;
                break;

            case StatModifier.cooldown:
                float proposedCooldownDivisor = DataManager.Instance.PlayerDataObject.CooldownDivisor += DataManager.Instance.PlayerDataObject.CooldownDivisorIncPerLevel;
                if (DataManager.Instance.PlayerDataObject.MaxCooldownDivisor > 0.0f && proposedCooldownDivisor > DataManager.Instance.PlayerDataObject.MaxCooldownDivisor)
                {
                    proposedCooldownDivisor = DataManager.Instance.PlayerDataObject.MaxCooldownDivisor;
                }
                DataManager.Instance.PlayerDataObject.CooldownDivisor = proposedCooldownDivisor;
                break;

            case StatModifier.count:
                int proposedCountIncreaser = DataManager.Instance.PlayerDataObject.CountIncreaser += DataManager.Instance.PlayerDataObject.CountIncreaserIncPerLevel;
                if (DataManager.Instance.PlayerDataObject.MaxCountIncreaser > 0 && proposedCountIncreaser > DataManager.Instance.PlayerDataObject.MaxCountIncreaser)
                {
                    proposedCountIncreaser = DataManager.Instance.PlayerDataObject.MaxCountIncreaser;
                }
                DataManager.Instance.PlayerDataObject.CountIncreaser = proposedCountIncreaser;
                break;

            case StatModifier.pierce:
                int proposedPierceIncreaser = DataManager.Instance.PlayerDataObject.PierceIncreaser += DataManager.Instance.PlayerDataObject.PierceIncreaserIncPerLevel;
                if (DataManager.Instance.PlayerDataObject.MaxPierceIncreaser > 0 && proposedPierceIncreaser > DataManager.Instance.PlayerDataObject.MaxPierceIncreaser)
                {
                    proposedPierceIncreaser = DataManager.Instance.PlayerDataObject.MaxPierceIncreaser;
                }
                DataManager.Instance.PlayerDataObject.PierceIncreaser = proposedPierceIncreaser;
                break;

            default:
                //do nothing
                break;
        }
    }

    public void UpgradeAttack(SecondaryAttack secondaryAttack)
    {
        //do not upgrade unaquired attack if maximum number of attacks are aquired
        if (!(DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired.Count >= DataManager.Instance.PlayerDataObject.MaximumNumberOfSecondaryAttacks && DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired.Contains(secondaryAttack)))
        {
            switch (secondaryAttack)
            {
                case SecondaryAttack.trailOfAssurace:
                    //do not upgrade attack if fully upgraded
                    if (DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel < DataManager.Instance.PlayerDataObject.TOAAttackDamage.Length - 1)
                    {
                        if (DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel == 0)
                        {
                            DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired.Add(secondaryAttack);
                        }
                        DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel++;
                    }
                    break;

                case SecondaryAttack.shieldOfLight:
                    //do not upgrade attack if fully upgraded
                    if (DataManager.Instance.PlayerDataObject.ShieldOfLightLevel < DataManager.Instance.PlayerDataObject.SOLAttackDamage.Length - 1)
                    {
                        if (DataManager.Instance.PlayerDataObject.ShieldOfLightLevel == 0)
                        {
                            DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired.Add(secondaryAttack);
                        }
                        DataManager.Instance.PlayerDataObject.ShieldOfLightLevel++;
                    }
                    break;

                case SecondaryAttack.wishingWell:
                    //do not upgrade attack if fully upgraded
                    if (DataManager.Instance.PlayerDataObject.WishingWellLevel < DataManager.Instance.PlayerDataObject.WWAttackDamage.Length - 1)
                    {
                        if (DataManager.Instance.PlayerDataObject.WishingWellLevel == 0)
                        {
                            DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired.Add(secondaryAttack);
                        }
                        DataManager.Instance.PlayerDataObject.WishingWellLevel++;
                    }
                    break;

                case SecondaryAttack.radiantOrb:
                    //do not upgrade attack if fully upgraded
                    if (DataManager.Instance.PlayerDataObject.RadiantOrbLevel < DataManager.Instance.PlayerDataObject.ROAttackDamage.Length - 1)
                    {
                        if (DataManager.Instance.PlayerDataObject.RadiantOrbLevel == 0)
                        {
                            DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired.Add(secondaryAttack);
                        }
                        DataManager.Instance.PlayerDataObject.RadiantOrbLevel++;
                    }
                    break;

                case SecondaryAttack.flickerOfHope:
                    //do not upgrade attack if fully upgraded
                    if (DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel < DataManager.Instance.PlayerDataObject.FlickAttackDamage.Length - 1)
                    {
                        if (DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel == 0)
                        {
                            DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired.Add(secondaryAttack);
                        }
                        DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel++;
                    }
                    break;

                case SecondaryAttack.sparkOfJoy:
                    //do not upgrade attack if fully upgraded
                    if (DataManager.Instance.PlayerDataObject.SparkOfJoyLevel < DataManager.Instance.PlayerDataObject.SparkAttackDamage.Length - 1)
                    {
                        if (DataManager.Instance.PlayerDataObject.SparkOfJoyLevel == 0)
                        {
                            DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired.Add(secondaryAttack);
                        }
                        DataManager.Instance.PlayerDataObject.SparkOfJoyLevel++;
                    }
                    break;

                case SecondaryAttack.moonBurst:
                    //do not upgrade attack if fully upgraded
                    if (DataManager.Instance.PlayerDataObject.MoonBurstLevel < DataManager.Instance.PlayerDataObject.MBAttackDamage.Length - 1)
                    {
                        if (DataManager.Instance.PlayerDataObject.MoonBurstLevel == 0)
                        {
                            DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired.Add(secondaryAttack);
                        }
                        DataManager.Instance.PlayerDataObject.MoonBurstLevel++;
                    }
                    break;

                case SecondaryAttack.floodOfHope:
                    //do not upgrade attack if fully upgraded
                    if (DataManager.Instance.PlayerDataObject.FloodOfHopeLevel < DataManager.Instance.PlayerDataObject.FloodAttackDamage.Length - 1)
                    {
                        if (DataManager.Instance.PlayerDataObject.FloodOfHopeLevel == 0)
                        {
                            DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired.Add(secondaryAttack);
                        }
                        DataManager.Instance.PlayerDataObject.FloodOfHopeLevel++;
                    }
                    break;

                case SecondaryAttack.surgeOfJoy:
                    //do not upgrade attack if fully upgraded
                    if (DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel < DataManager.Instance.PlayerDataObject.SurgeAttackDamage.Length - 1)
                    {
                        if (DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel == 0)
                        {
                            DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired.Add(secondaryAttack);
                        }
                        DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel++;
                    }
                    break;

                case SecondaryAttack.sunBurst:
                    //do not upgrade attack if fully upgraded
                    if (DataManager.Instance.PlayerDataObject.SunBurstLevel < DataManager.Instance.PlayerDataObject.SBAttackDamage.Length - 1)
                    {
                        if (DataManager.Instance.PlayerDataObject.SunBurstLevel == 0)
                        {
                            DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired.Add(secondaryAttack);
                        }
                        DataManager.Instance.PlayerDataObject.SunBurstLevel++;
                    }
                    break;

                case SecondaryAttack.waveOfRelief:
                    //do not upgrade attack if fully upgraded
                    if (DataManager.Instance.PlayerDataObject.WaveOfReliefLevel < DataManager.Instance.PlayerDataObject.WORAttackDamage.Length - 1)
                    {
                        if (DataManager.Instance.PlayerDataObject.WaveOfReliefLevel == 0)
                        {
                            DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired.Add(secondaryAttack);
                        }
                        DataManager.Instance.PlayerDataObject.WaveOfReliefLevel++;
                    }
                    break;

                case SecondaryAttack.pendantOfLife:
                    //do not upgrade attack if fully upgraded
                    if (DataManager.Instance.PlayerDataObject.PendantOfLifeLevel < DataManager.Instance.PlayerDataObject.POLHealingSpeed.Length - 1)
                    {
                        if (DataManager.Instance.PlayerDataObject.PendantOfLifeLevel == 0)
                        {
                            DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired.Add(secondaryAttack);
                        }
                        DataManager.Instance.PlayerDataObject.PendantOfLifeLevel++;
                    }
                    break;

                default:
                    //do nothing
                    break;
            }
        }
    }

    private IEnumerator SpawnInitialMirror()
    {
        yield return null;

        //find point to spawn mirror at
        Vector3 point;
        do
        {
            point = new Vector3(Random.Range(-DataManager.Instance.LevelDataObject.MirrorSpawningBounds.x / 2.0f, DataManager.Instance.LevelDataObject.MirrorSpawningBounds.x / 2.0f), Random.Range(-DataManager.Instance.LevelDataObject.MirrorSpawningBounds.y / 2.0f, DataManager.Instance.LevelDataObject.MirrorSpawningBounds.y / 2.0f), 0.0f);
        }
        while (Vector3.Distance(point, DataManager.Instance.PlayerDataObject.Player.transform.position) < DataManager.Instance.LevelDataObject.MinMirrorSpawnDistance);

        //spawn mirror
        MirrorAIController mirror = (MirrorAIController)PoolManager.Instance.Spawn(DataManager.Instance.LevelDataObject.InitialMirror.name, point, Quaternion.identity);
        mirror.Init();

        //Look at new Mirror
        EventManager.Instance.LookAtMirrorTriggered.TriggerEvent(mirror.transform.position);
    }

    private IEnumerator TimedThreatIncrease()
    {
        while (true)
        {
            yield return new WaitForSeconds(DataManager.Instance.LevelDataObject.ThreatIncreaseTime);

            IncreaseThreat();
        }
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

            //increase new enemy speed multiplier
            DataManager.Instance.LevelDataObject.NewEnemyWeightMultiplier += (DataManager.Instance.LevelDataObject.FinalWeightMultiplier - 1.0f) / DataManager.Instance.LevelDataObject.FinalCalculatedThreatLevel;
            //clamp new enemy HP multiplier
            if (DataManager.Instance.LevelDataObject.UseMaxWeightMultiplier && DataManager.Instance.LevelDataObject.NewEnemyWeightMultiplier > DataManager.Instance.LevelDataObject.MaxWeightMultiplier)
            {
                DataManager.Instance.LevelDataObject.NewEnemyWeightMultiplier = DataManager.Instance.LevelDataObject.MaxWeightMultiplier;
            }

            //increase new enemy spawn frequency multiplier
            DataManager.Instance.LevelDataObject.NewEnemySpawnFrequencyMultiplier += (DataManager.Instance.LevelDataObject.FinalSpawnFrequencyMultiplier - 1.0f) / DataManager.Instance.LevelDataObject.FinalCalculatedThreatLevel;
            //clamp new enemy HP multiplier
            if (DataManager.Instance.LevelDataObject.UseMaxSpawnFrequencyMultiplier && DataManager.Instance.LevelDataObject.NewEnemySpawnFrequencyMultiplier > DataManager.Instance.LevelDataObject.MaxSpawnFrequencyMultiplier)
            {
                DataManager.Instance.LevelDataObject.NewEnemySpawnFrequencyMultiplier = DataManager.Instance.LevelDataObject.MaxSpawnFrequencyMultiplier;
            }

            //increase new enemy special charge divisor
            DataManager.Instance.LevelDataObject.NewEnemySpecialChargeDivisor += (DataManager.Instance.LevelDataObject.FinalSpecialChargeDivisor - 1.0f) / DataManager.Instance.LevelDataObject.FinalCalculatedThreatLevel;
            //clamp new enemy HP multiplier
            if (DataManager.Instance.LevelDataObject.UseMaxSpecialChargeDivisor && DataManager.Instance.LevelDataObject.NewEnemySpecialChargeDivisor > DataManager.Instance.LevelDataObject.MaxSpecialChargeDivisor)
            {
                DataManager.Instance.LevelDataObject.NewEnemySpecialChargeDivisor = DataManager.Instance.LevelDataObject.MaxSpecialChargeDivisor;
            }

            //inrease max enemy count
            DataManager.Instance.LevelDataObject.MaxEnemyCountUnrounded += ((float)DataManager.Instance.LevelDataObject.FinalMaxEnemyCount - (float)DataManager.Instance.LevelDataObject.InitialMaxEnemyCount) / (float)DataManager.Instance.LevelDataObject.FinalCalculatedThreatLevel;
            DataManager.Instance.LevelDataObject.MaxEnemyCount = (int)DataManager.Instance.LevelDataObject.MaxEnemyCountUnrounded;
            //clamp max enemy count
            if (DataManager.Instance.LevelDataObject.MaxEnemyCount > DataManager.Instance.LevelDataObject.AbsoluteMaxEnemyCount)
            {
                DataManager.Instance.LevelDataObject.MaxEnemyCount = DataManager.Instance.LevelDataObject.AbsoluteMaxEnemyCount;
            }
        }
    }
}
