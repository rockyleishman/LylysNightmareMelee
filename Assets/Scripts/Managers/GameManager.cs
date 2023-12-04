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
        DataManager.Instance.PlayerDataObject.RangeMultiplier = 1.0f;
        DataManager.Instance.PlayerDataObject.CooldownDivisor = 1.0f;
        DataManager.Instance.PlayerDataObject.CountIncreaser = 0;
        DataManager.Instance.PlayerDataObject.PierceIncreaser = 0;

        //set secondary attack levels to 0
        DataManager.Instance.PlayerDataObject.SecondaryAttacksAquired = new List<SecondaryAttack>();
        DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel = 1;
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
        DataManager.Instance.PlayerDataObject.PendantOfLifeLevel = 0; //need to see health to test

        //set initial special charge
        DataManager.Instance.PlayerDataObject.SpecialCharge = 1.0f;

        //spawn initial mirror
        StartCoroutine(SpawnInitialMirror());

        //start timed threat increase
        StartCoroutine(TimedThreatIncrease());

        //hide and constain cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
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

            //increase new enemy speed multiplier
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
