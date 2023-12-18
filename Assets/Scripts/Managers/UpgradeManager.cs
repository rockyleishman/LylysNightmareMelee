using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] public float TimeToWaitBeforeUpgradeTriggers = 1.5f;

    [SerializeField] public GameObject UpgradeUI;

    [SerializeField] public TextMeshProUGUI UpgradeButtonTextField1;
    [SerializeField] public TextMeshProUGUI UpgradeButtonTextField2;
    [SerializeField] public TextMeshProUGUI UpgradeButtonTextField3;

    private UpgradeType _upgrade1;
    private UpgradeType _upgrade2;
    private UpgradeType _upgrade3;

    private List<UpgradeType> _availableStatUpgrades;
    private List<UpgradeType> _availableAttackUpgrades;
    private List<UpgradeType> _chosenAttackUpgrades;
    private bool _haveAttackUpgradesBeenLimited;

    private void Start()
    {
        //hide UI
        HideUpgradeUI();

        //init upgrade options
        _upgrade1 = UpgradeType.hitPoints;
        _upgrade1 = UpgradeType.hitPoints;
        _upgrade1 = UpgradeType.hitPoints;

        //init available stat upgrades
        _availableStatUpgrades = new List<UpgradeType>();
        _availableStatUpgrades.Add(UpgradeType.hitPoints);
        _availableStatUpgrades.Add(UpgradeType.movementSpeed);
        _availableStatUpgrades.Add(UpgradeType.damage);
        _availableStatUpgrades.Add(UpgradeType.knockback);
        _availableStatUpgrades.Add(UpgradeType.range);
        _availableStatUpgrades.Add(UpgradeType.cooldown);
        _availableStatUpgrades.Add(UpgradeType.count);
        _availableStatUpgrades.Add(UpgradeType.pierce);

        //init available attack upgrades
        _availableAttackUpgrades = new List<UpgradeType>();
        _availableAttackUpgrades.Add(UpgradeType.trailOfAssurace);
        //_availableAttackUpgrades.Add(UpgradeType.shieldOfLight);//not implemented yet
        //_availableAttackUpgrades.Add(UpgradeType.wishingWell);//not implemented yet
        //_availableAttackUpgrades.Add(UpgradeType.radiantOrb);//not implemented yet
        _availableAttackUpgrades.Add(UpgradeType.flickerOfHope);
        _availableAttackUpgrades.Add(UpgradeType.sparkOfJoy);
        _availableAttackUpgrades.Add(UpgradeType.moonBurst);
        _availableAttackUpgrades.Add(UpgradeType.floodOfHope);
        _availableAttackUpgrades.Add(UpgradeType.surgeOfJoy);
        _availableAttackUpgrades.Add(UpgradeType.sunBurst);
        _availableAttackUpgrades.Add(UpgradeType.waveOfRelief);
        _availableAttackUpgrades.Add(UpgradeType.pendantOfLife);

        //attack upgrades have not reached limit yet
        _chosenAttackUpgrades = new List<UpgradeType>();
        _haveAttackUpgradesBeenLimited = false;
    }

    private void ShowUpgradeUI()
    {
        SoundManager.Instance.PlayUpgrade();
        //disable player controls
        DataManager.Instance.PlayerDataObject.Player.GetComponent<PlayerInput>().SwitchCurrentActionMap("ForcedMenu");

        Time.timeScale = 0.0f;
        Cursor.visible = true;
        UpgradeUI.SetActive(true);
    }

    private void HideUpgradeUI()
    {
        //restore player controls
        DataManager.Instance.PlayerDataObject.Player.GetComponent<PlayerInput>().SwitchCurrentActionMap("MainGameplay");

        Cursor.visible = false;
        UpgradeUI.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void CheckAttackUpgradesLimit()
    {
        if (!_haveAttackUpgradesBeenLimited && _chosenAttackUpgrades.Count == DataManager.Instance.PlayerDataObject.MaximumNumberOfSecondaryAttacks)
        {
            foreach (UpgradeType upgradeType in _availableAttackUpgrades)
            {
                if (!_chosenAttackUpgrades.Contains(upgradeType))
                {
                    _availableAttackUpgrades.Remove(upgradeType);
                }
            }
        }        
    }

    public void StartUpgrading()
    {
        StartCoroutine(Upgrading());
    }

    private IEnumerator Upgrading()
    {
        yield return new WaitForSeconds(TimeToWaitBeforeUpgradeTriggers);

        //deep copy availability lists as option lists (for making sure the same option doesn't take up multiple option slots)
        List<UpgradeType> statUpgradeOptions = new List<UpgradeType>();
        foreach (UpgradeType upgradeType in _availableStatUpgrades)
        {
            statUpgradeOptions.Add(upgradeType);
        }
        List<UpgradeType> attackUpgradeOptions = new List<UpgradeType>();
        foreach (UpgradeType upgradeType in _availableAttackUpgrades)
        {
            attackUpgradeOptions.Add(upgradeType);
        }
        List<UpgradeType> chosenAttackUpgradeOptions = new List<UpgradeType>();
        foreach (UpgradeType upgradeType in _chosenAttackUpgrades)
        {
            if (attackUpgradeOptions.Contains(upgradeType))
            {
                chosenAttackUpgradeOptions.Add(upgradeType);
            }
        }

        //determine option 1
        int option1Index = Random.Range(0, statUpgradeOptions.Count);
        _upgrade1 = statUpgradeOptions[option1Index];
        statUpgradeOptions.RemoveAt(option1Index);

        //determine option 2
        if (attackUpgradeOptions.Count == 0)
        {
            int option2Index = Random.Range(0, statUpgradeOptions.Count);
            _upgrade2 = statUpgradeOptions[option2Index];
            statUpgradeOptions.RemoveAt(option2Index);
        }
        else
        {
            int option2type = Random.Range(0, 3);
            if (option2type == 0)
            {
                int option2Index = Random.Range(0, statUpgradeOptions.Count);
                _upgrade2 = statUpgradeOptions[option2Index];
                statUpgradeOptions.RemoveAt(option2Index);
            }
            else if (option2type == 1 && chosenAttackUpgradeOptions.Count > 0)
            {
                int option2Index = Random.Range(0, chosenAttackUpgradeOptions.Count);
                _upgrade2 = chosenAttackUpgradeOptions[option2Index];
                chosenAttackUpgradeOptions.RemoveAt(option2Index);
                attackUpgradeOptions.Remove(_upgrade2);
            }
            else
            {
                int option2type2 = Random.Range(0, 2);
                if (option2type2 == 0 && chosenAttackUpgradeOptions.Count > 0)
                {
                    int option2Index = Random.Range(0, chosenAttackUpgradeOptions.Count);
                    _upgrade2 = chosenAttackUpgradeOptions[option2Index];
                    chosenAttackUpgradeOptions.RemoveAt(option2Index);
                    attackUpgradeOptions.Remove(_upgrade2);
                }
                else
                {
                    int option2Index = Random.Range(0, attackUpgradeOptions.Count);
                    _upgrade2 = attackUpgradeOptions[option2Index];
                    attackUpgradeOptions.RemoveAt(option2Index);
                }
            }
        }

        //determine option 3
        if (attackUpgradeOptions.Count == 0)
        {
            int option3Index = Random.Range(0, statUpgradeOptions.Count);
            _upgrade3 = statUpgradeOptions[option3Index];
        }
        else
        {
            int option3type = Random.Range(0, 2);
            if (option3type == 0 && chosenAttackUpgradeOptions.Count > 0)
            {
                int option3Index = Random.Range(0, chosenAttackUpgradeOptions.Count);
                _upgrade3 = chosenAttackUpgradeOptions[option3Index];
            }
            else
            {
                int option3Index = Random.Range(0, attackUpgradeOptions.Count);
                _upgrade3 = attackUpgradeOptions[option3Index];
            }
        }

        //change option buttons
        ChangeUpgradeOptions();

        //show UI
        ShowUpgradeUI();
    }

    private void ChangeUpgradeOptions()
    {
        switch(_upgrade1)
        {
            case UpgradeType.hitPoints:
                UpgradeButtonTextField1.text = string.Format("<b>Health</b>\n\n<i>+{0}% base HP</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.HPMultiplierIncPerLevel * 100));
                break;

            case UpgradeType.movementSpeed:
                UpgradeButtonTextField1.text = string.Format("<b>Movement Speed</b>\n\n<i>+{0}% base movement speed</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.MovementSpeedMultiplierIncPerLevel * 100));
                break;

            case UpgradeType.damage:
                UpgradeButtonTextField1.text = string.Format("<b>Damage</b>\n\n<i>+{0}% base damage for all attacks</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.DamageMultiplierIncPerLevel * 100));
                break;

            case UpgradeType.knockback:
                UpgradeButtonTextField1.text = string.Format("<b>Knockback</b>\n\n<i>+{0}% base knockback for all attacks</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.KnockbackMultiplierIncPerLevel * 100));
                break;

            case UpgradeType.range:
                UpgradeButtonTextField1.text = string.Format("<b>Range</b>\n\n<i>+{0}% base range for all attacks</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.RangeMultiplierIncPerLevel * 100));
                break;

            case UpgradeType.cooldown:
                UpgradeButtonTextField1.text = string.Format("<b>Attack Speed</b>\n\n<i>reduces cooldown for all attacks</i>");
                break;

            case UpgradeType.count:
                UpgradeButtonTextField1.text = string.Format("<b>Projectile Count</b>\n\n<i>multi-projectile attacks shoot {0} additional projectile</i>", DataManager.Instance.PlayerDataObject.CountIncreaserIncPerLevel);
                break;

            case UpgradeType.pierce:
                UpgradeButtonTextField1.text = string.Format("<b>Pierce</b>\n\n<i>projectiles can hit {0} additional enemy</i>", DataManager.Instance.PlayerDataObject.PierceIncreaserIncPerLevel);
                break;
        }

        switch(_upgrade2)
        {
            case UpgradeType.hitPoints:
                UpgradeButtonTextField2.text = string.Format("<b>Health</b>\n\n<i>+{0}% base HP</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.HPMultiplierIncPerLevel* 100));
                break;

            case UpgradeType.movementSpeed:
                UpgradeButtonTextField2.text = string.Format("<b>Movement Speed</b>\n\n<i>+{0}% base movement speed</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.MovementSpeedMultiplierIncPerLevel* 100));
                break;

            case UpgradeType.damage:
                UpgradeButtonTextField2.text = string.Format("<b>Damage</b>\n\n<i>+{0}% base damage for all attacks</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.DamageMultiplierIncPerLevel* 100));
                break;

            case UpgradeType.knockback:
                UpgradeButtonTextField2.text = string.Format("<b>Knockback</b>\n\n<i>+{0}% base knockback for all attacks</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.KnockbackMultiplierIncPerLevel* 100));
                break;

            case UpgradeType.range:
                UpgradeButtonTextField2.text = string.Format("<b>Range</b>\n\n<i>+{0}% base range for all attacks</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.RangeMultiplierIncPerLevel* 100));
                break;

            case UpgradeType.cooldown:
                UpgradeButtonTextField2.text = string.Format("<b>Attack Speed</b>\n\n<i>reduces cooldown for all attacks</i>");
                break;

            case UpgradeType.count:
                UpgradeButtonTextField2.text = string.Format("<b>Projectile Count</b>\n\n<i>multi-projectile attacks shoot {0} additional projectile</i>", DataManager.Instance.PlayerDataObject.CountIncreaserIncPerLevel);
                break;

            case UpgradeType.pierce:
                UpgradeButtonTextField2.text = string.Format("<b>Pierce</b>\n\n<i>projectiles can hit {0} additional enemy</i>", DataManager.Instance.PlayerDataObject.PierceIncreaserIncPerLevel);
                break;

            case UpgradeType.trailOfAssurace:
                UpgradeButtonTextField2.text = string.Format("<b>Trail of Assurance</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel + 1, DataManager.Instance.PlayerDataObject.TrailOfAssuranceUpgradeInfo[DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel]);
                break;

            case UpgradeType.shieldOfLight:
                UpgradeButtonTextField2.text = string.Format("<b>Shield of Light</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.ShieldOfLightLevel + 1, DataManager.Instance.PlayerDataObject.ShieldOfLightUpgradeInfo[DataManager.Instance.PlayerDataObject.ShieldOfLightLevel]);
                break;

            case UpgradeType.wishingWell:
                UpgradeButtonTextField2.text = string.Format("<b>Wishing Well</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.WishingWellLevel + 1, DataManager.Instance.PlayerDataObject.WishingWellUpgradeInfo[DataManager.Instance.PlayerDataObject.WishingWellLevel]);
                break;

            case UpgradeType.radiantOrb:
                UpgradeButtonTextField2.text = string.Format("<b>Radiant Orb</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.RadiantOrbLevel + 1, DataManager.Instance.PlayerDataObject.RadiantOrbUpgradeInfo[DataManager.Instance.PlayerDataObject.RadiantOrbLevel]);
                break;

            case UpgradeType.flickerOfHope:
                UpgradeButtonTextField2.text = string.Format("<b>Flicker of Hope</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel + 1, DataManager.Instance.PlayerDataObject.FlickerOfHopeUpgradeInfo[DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel]);
                break;

            case UpgradeType.sparkOfJoy:
                UpgradeButtonTextField2.text = string.Format("<b>Spark of Joy</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.SparkOfJoyLevel + 1, DataManager.Instance.PlayerDataObject.SparkOfJoyUpgradeInfo[DataManager.Instance.PlayerDataObject.SparkOfJoyLevel]);
                break;

            case UpgradeType.moonBurst:
                UpgradeButtonTextField2.text = string.Format("<b>Moon-Burst</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.MoonBurstLevel + 1, DataManager.Instance.PlayerDataObject.MoonBurstUpgradeInfo[DataManager.Instance.PlayerDataObject.MoonBurstLevel]);
                break;

            case UpgradeType.floodOfHope:
                UpgradeButtonTextField2.text = string.Format("<b>Flood of Hope</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.FloodOfHopeLevel + 1, DataManager.Instance.PlayerDataObject.FloodOfHopeUpgradeInfo[DataManager.Instance.PlayerDataObject.FloodOfHopeLevel]);
                break;

            case UpgradeType.surgeOfJoy:
                UpgradeButtonTextField2.text = string.Format("<b>Surge of Joy</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel + 1, DataManager.Instance.PlayerDataObject.SurgeOfJoyUpgradeInfo[DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel]);
                break;

            case UpgradeType.sunBurst:
                UpgradeButtonTextField2.text = string.Format("<b>Sun-Burst</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.SunBurstLevel + 1, DataManager.Instance.PlayerDataObject.SunBurstUpgradeInfo[DataManager.Instance.PlayerDataObject.SunBurstLevel]);
                break;

            case UpgradeType.waveOfRelief:
                UpgradeButtonTextField2.text = string.Format("<b>Wave of Relief</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.WaveOfReliefLevel + 1, DataManager.Instance.PlayerDataObject.WaveOfReliefUpgradeInfo[DataManager.Instance.PlayerDataObject.WaveOfReliefLevel]);
                break;

            case UpgradeType.pendantOfLife:
                UpgradeButtonTextField2.text = string.Format("<b>Pendant of Life</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.PendantOfLifeLevel + 1, DataManager.Instance.PlayerDataObject.PendantOfLifeUpgradeInfo[DataManager.Instance.PlayerDataObject.PendantOfLifeLevel]);
                break;
        }

        switch (_upgrade3)
        {
            case UpgradeType.hitPoints:
                UpgradeButtonTextField3.text = string.Format("<b>Health</b>\n\n<i>+{0}% base HP</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.HPMultiplierIncPerLevel * 100));
                break;

            case UpgradeType.movementSpeed:
                UpgradeButtonTextField3.text = string.Format("<b>Movement Speed</b>\n\n<i>+{0}% base movement speed</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.MovementSpeedMultiplierIncPerLevel * 100));
                break;

            case UpgradeType.damage:
                UpgradeButtonTextField3.text = string.Format("<b>Damage</b>\n\n<i>+{0}% base damage for all attacks</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.DamageMultiplierIncPerLevel * 100));
                break;

            case UpgradeType.knockback:
                UpgradeButtonTextField3.text = string.Format("<b>Knockback</b>\n\n<i>+{0}% base knockback for all attacks</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.KnockbackMultiplierIncPerLevel * 100));
                break;

            case UpgradeType.range:
                UpgradeButtonTextField3.text = string.Format("<b>Range</b>\n\n<i>+{0}% base range for all attacks</i>", Mathf.RoundToInt(DataManager.Instance.PlayerDataObject.RangeMultiplierIncPerLevel * 100));
                break;

            case UpgradeType.cooldown:
                UpgradeButtonTextField3.text = string.Format("<b>Attack Speed</b>\n\n<i>reduces cooldown for all attacks</i>");
                break;

            case UpgradeType.count:
                UpgradeButtonTextField3.text = string.Format("<b>Projectile Count</b>\n\n<i>multi-projectile attacks shoot {0} additional projectile</i>", DataManager.Instance.PlayerDataObject.CountIncreaserIncPerLevel);
                break;

            case UpgradeType.pierce:
                UpgradeButtonTextField3.text = string.Format("<b>Pierce</b>\n\n<i>projectiles can hit {0} additional enemy</i>", DataManager.Instance.PlayerDataObject.PierceIncreaserIncPerLevel);
                break;

            case UpgradeType.trailOfAssurace:
                UpgradeButtonTextField3.text = string.Format("<b>Trail of Assurance</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel + 1, DataManager.Instance.PlayerDataObject.TrailOfAssuranceUpgradeInfo[DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel]);
                break;

            case UpgradeType.shieldOfLight:
                UpgradeButtonTextField3.text = string.Format("<b>Shield of Light</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.ShieldOfLightLevel + 1, DataManager.Instance.PlayerDataObject.ShieldOfLightUpgradeInfo[DataManager.Instance.PlayerDataObject.ShieldOfLightLevel]);
                break;

            case UpgradeType.wishingWell:
                UpgradeButtonTextField3.text = string.Format("<b>Wishing Well</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.WishingWellLevel + 1, DataManager.Instance.PlayerDataObject.WishingWellUpgradeInfo[DataManager.Instance.PlayerDataObject.WishingWellLevel]);
                break;

            case UpgradeType.radiantOrb:
                UpgradeButtonTextField3.text = string.Format("<b>Radiant Orb</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.RadiantOrbLevel + 1, DataManager.Instance.PlayerDataObject.RadiantOrbUpgradeInfo[DataManager.Instance.PlayerDataObject.RadiantOrbLevel]);
                break;

            case UpgradeType.flickerOfHope:
                UpgradeButtonTextField3.text = string.Format("<b>Flicker of Hope</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel + 1, DataManager.Instance.PlayerDataObject.FlickerOfHopeUpgradeInfo[DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel]);
                break;

            case UpgradeType.sparkOfJoy:
                UpgradeButtonTextField3.text = string.Format("<b>Spark of Joy</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.SparkOfJoyLevel + 1, DataManager.Instance.PlayerDataObject.SparkOfJoyUpgradeInfo[DataManager.Instance.PlayerDataObject.SparkOfJoyLevel]);
                break;

            case UpgradeType.moonBurst:
                UpgradeButtonTextField3.text = string.Format("<b>Moon-Burst</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.MoonBurstLevel + 1, DataManager.Instance.PlayerDataObject.MoonBurstUpgradeInfo[DataManager.Instance.PlayerDataObject.MoonBurstLevel]);
                break;

            case UpgradeType.floodOfHope:
                UpgradeButtonTextField3.text = string.Format("<b>Flood of Hope</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.FloodOfHopeLevel + 1, DataManager.Instance.PlayerDataObject.FloodOfHopeUpgradeInfo[DataManager.Instance.PlayerDataObject.FloodOfHopeLevel]);
                break;

            case UpgradeType.surgeOfJoy:
                UpgradeButtonTextField3.text = string.Format("<b>Surge of Joy</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel + 1, DataManager.Instance.PlayerDataObject.SurgeOfJoyUpgradeInfo[DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel]);
                break;

            case UpgradeType.sunBurst:
                UpgradeButtonTextField3.text = string.Format("<b>Sun-Burst</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.SunBurstLevel + 1, DataManager.Instance.PlayerDataObject.SunBurstUpgradeInfo[DataManager.Instance.PlayerDataObject.SunBurstLevel]);
                break;

            case UpgradeType.waveOfRelief:
                UpgradeButtonTextField3.text = string.Format("<b>Wave of Relief</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.WaveOfReliefLevel + 1, DataManager.Instance.PlayerDataObject.WaveOfReliefUpgradeInfo[DataManager.Instance.PlayerDataObject.WaveOfReliefLevel]);
                break;

            case UpgradeType.pendantOfLife:
                UpgradeButtonTextField3.text = string.Format("<b>Pendant of Life</b>\nLevel {0}\n\n<i>{1}</i>", DataManager.Instance.PlayerDataObject.PendantOfLifeLevel + 1, DataManager.Instance.PlayerDataObject.PendantOfLifeUpgradeInfo[DataManager.Instance.PlayerDataObject.PendantOfLifeLevel]);
                break;
        }
    }

    public void Upgrade1()
    {
        switch (_upgrade1)
        {
            case UpgradeType.hitPoints:
                GameManager.Instance.UpgradeStat(StatModifier.hitPoints);
                if (DataManager.Instance.PlayerDataObject.MaxHPMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.HPMultiplier >= DataManager.Instance.PlayerDataObject.MaxHPMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.hitPoints);
                }
                break;

            case UpgradeType.movementSpeed:
                GameManager.Instance.UpgradeStat(StatModifier.movementSpeed);
                if (DataManager.Instance.PlayerDataObject.MaxMovementSpeedMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.MovementSpeedMultiplier >= DataManager.Instance.PlayerDataObject.MaxMovementSpeedMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.movementSpeed);
                }
                break;

            case UpgradeType.damage:
                GameManager.Instance.UpgradeStat(StatModifier.damage);
                if (DataManager.Instance.PlayerDataObject.MaxDamageMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.DamageMultiplier >= DataManager.Instance.PlayerDataObject.MaxDamageMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.damage);
                }
                break;

            case UpgradeType.knockback:
                GameManager.Instance.UpgradeStat(StatModifier.knockback);
                if (DataManager.Instance.PlayerDataObject.MaxKnockbackMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.KnockbackMultiplier >= DataManager.Instance.PlayerDataObject.MaxKnockbackMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.knockback);
                }
                break;

            case UpgradeType.range:
                GameManager.Instance.UpgradeStat(StatModifier.range);
                if (DataManager.Instance.PlayerDataObject.MaxRangeMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.RangeMultiplier >= DataManager.Instance.PlayerDataObject.MaxRangeMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.range);
                }
                break;

            case UpgradeType.cooldown:
                GameManager.Instance.UpgradeStat(StatModifier.cooldown);
                if (DataManager.Instance.PlayerDataObject.MaxCooldownDivisor > 0.0f && DataManager.Instance.PlayerDataObject.CooldownDivisor >= DataManager.Instance.PlayerDataObject.MaxCooldownDivisor)
                {
                    _availableStatUpgrades.Remove(UpgradeType.cooldown);
                }
                break;

            case UpgradeType.count:
                GameManager.Instance.UpgradeStat(StatModifier.count);
                if (DataManager.Instance.PlayerDataObject.MaxCountIncreaser > 0 && DataManager.Instance.PlayerDataObject.CountIncreaser >= DataManager.Instance.PlayerDataObject.MaxCountIncreaser)
                {
                    _availableStatUpgrades.Remove(UpgradeType.count);
                }
                break;

            case UpgradeType.pierce:
                GameManager.Instance.UpgradeStat(StatModifier.pierce);
                if (DataManager.Instance.PlayerDataObject.MaxPierceIncreaser > 0 && DataManager.Instance.PlayerDataObject.PierceIncreaser >= DataManager.Instance.PlayerDataObject.MaxPierceIncreaser)
                {
                    _availableStatUpgrades.Remove(UpgradeType.pierce);
                }
                break;
        }

        //finish upgrade
        HideUpgradeUI();
    }

    public void Upgrade2()
    {
        switch (_upgrade2)
        {
            case UpgradeType.hitPoints:
                GameManager.Instance.UpgradeStat(StatModifier.hitPoints);
                if (DataManager.Instance.PlayerDataObject.MaxHPMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.HPMultiplier >= DataManager.Instance.PlayerDataObject.MaxHPMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.hitPoints);
                }
                break;

            case UpgradeType.movementSpeed:
                GameManager.Instance.UpgradeStat(StatModifier.movementSpeed);
                if (DataManager.Instance.PlayerDataObject.MaxMovementSpeedMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.MovementSpeedMultiplier >= DataManager.Instance.PlayerDataObject.MaxMovementSpeedMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.movementSpeed);
                }
                break;

            case UpgradeType.damage:
                GameManager.Instance.UpgradeStat(StatModifier.damage);
                if (DataManager.Instance.PlayerDataObject.MaxDamageMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.DamageMultiplier >= DataManager.Instance.PlayerDataObject.MaxDamageMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.damage);
                }
                break;

            case UpgradeType.knockback:
                GameManager.Instance.UpgradeStat(StatModifier.knockback);
                if (DataManager.Instance.PlayerDataObject.MaxKnockbackMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.KnockbackMultiplier >= DataManager.Instance.PlayerDataObject.MaxKnockbackMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.knockback);
                }
                break;

            case UpgradeType.range:
                GameManager.Instance.UpgradeStat(StatModifier.range);
                if (DataManager.Instance.PlayerDataObject.MaxRangeMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.RangeMultiplier >= DataManager.Instance.PlayerDataObject.MaxRangeMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.range);
                }
                break;

            case UpgradeType.cooldown:
                GameManager.Instance.UpgradeStat(StatModifier.cooldown);
                if (DataManager.Instance.PlayerDataObject.MaxCooldownDivisor > 0.0f && DataManager.Instance.PlayerDataObject.CooldownDivisor >= DataManager.Instance.PlayerDataObject.MaxCooldownDivisor)
                {
                    _availableStatUpgrades.Remove(UpgradeType.cooldown);
                }
                break;

            case UpgradeType.count:
                GameManager.Instance.UpgradeStat(StatModifier.count);
                if (DataManager.Instance.PlayerDataObject.MaxCountIncreaser > 0 && DataManager.Instance.PlayerDataObject.CountIncreaser >= DataManager.Instance.PlayerDataObject.MaxCountIncreaser)
                {
                    _availableStatUpgrades.Remove(UpgradeType.count);
                }
                break;

            case UpgradeType.pierce:
                GameManager.Instance.UpgradeStat(StatModifier.pierce);
                if (DataManager.Instance.PlayerDataObject.MaxPierceIncreaser > 0 && DataManager.Instance.PlayerDataObject.PierceIncreaser >= DataManager.Instance.PlayerDataObject.MaxPierceIncreaser)
                {
                    _availableStatUpgrades.Remove(UpgradeType.pierce);
                }
                break;

            case UpgradeType.trailOfAssurace:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.trailOfAssurace);
                if (DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel >= DataManager.Instance.PlayerDataObject.TOAAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.trailOfAssurace);
                }
                break;

            case UpgradeType.shieldOfLight:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.shieldOfLight);
                if (DataManager.Instance.PlayerDataObject.ShieldOfLightLevel >= DataManager.Instance.PlayerDataObject.SOLAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.shieldOfLight);
                }
                break;

            case UpgradeType.wishingWell:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.wishingWell);
                if (DataManager.Instance.PlayerDataObject.WishingWellLevel >= DataManager.Instance.PlayerDataObject.WWAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.wishingWell);
                }
                break;

            case UpgradeType.radiantOrb:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.radiantOrb);
                if (DataManager.Instance.PlayerDataObject.RadiantOrbLevel >= DataManager.Instance.PlayerDataObject.ROAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.radiantOrb);
                }
                break;

            case UpgradeType.flickerOfHope:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.flickerOfHope);
                if (DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel >= DataManager.Instance.PlayerDataObject.FlickAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.flickerOfHope);
                }
                break;

            case UpgradeType.sparkOfJoy:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.sparkOfJoy);
                if (DataManager.Instance.PlayerDataObject.SparkOfJoyLevel >= DataManager.Instance.PlayerDataObject.SparkAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.sparkOfJoy);
                }
                break;

            case UpgradeType.moonBurst:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.moonBurst);
                if (DataManager.Instance.PlayerDataObject.MoonBurstLevel >= DataManager.Instance.PlayerDataObject.MBAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.moonBurst);
                }
                break;

            case UpgradeType.floodOfHope:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.floodOfHope);
                if (DataManager.Instance.PlayerDataObject.FloodOfHopeLevel >= DataManager.Instance.PlayerDataObject.FloodAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.floodOfHope);
                }
                break;

            case UpgradeType.surgeOfJoy:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.surgeOfJoy);
                if (DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel >= DataManager.Instance.PlayerDataObject.SurgeAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.surgeOfJoy);
                }
                break;

            case UpgradeType.sunBurst:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.sunBurst);
                if (DataManager.Instance.PlayerDataObject.SunBurstLevel >= DataManager.Instance.PlayerDataObject.SBAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.sunBurst);
                }
                break;

            case UpgradeType.waveOfRelief:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.waveOfRelief);
                if (DataManager.Instance.PlayerDataObject.WaveOfReliefLevel >= DataManager.Instance.PlayerDataObject.WORAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.waveOfRelief);
                }
                break;

            case UpgradeType.pendantOfLife:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.pendantOfLife);
                if (DataManager.Instance.PlayerDataObject.PendantOfLifeLevel >= DataManager.Instance.PlayerDataObject.POLHealingSpeed.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.pendantOfLife);
                }
                break;
        }

        //finish upgrade
        HideUpgradeUI();
    }

    public void Upgrade3()
    {
        switch (_upgrade3)
        {
            case UpgradeType.hitPoints:
                GameManager.Instance.UpgradeStat(StatModifier.hitPoints);
                if (DataManager.Instance.PlayerDataObject.MaxHPMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.HPMultiplier >= DataManager.Instance.PlayerDataObject.MaxHPMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.hitPoints);
                }
                break;

            case UpgradeType.movementSpeed:
                GameManager.Instance.UpgradeStat(StatModifier.movementSpeed);
                if (DataManager.Instance.PlayerDataObject.MaxMovementSpeedMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.MovementSpeedMultiplier >= DataManager.Instance.PlayerDataObject.MaxMovementSpeedMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.movementSpeed);
                }
                break;

            case UpgradeType.damage:
                GameManager.Instance.UpgradeStat(StatModifier.damage);
                if (DataManager.Instance.PlayerDataObject.MaxDamageMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.DamageMultiplier >= DataManager.Instance.PlayerDataObject.MaxDamageMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.damage);
                }
                break;

            case UpgradeType.knockback:
                GameManager.Instance.UpgradeStat(StatModifier.knockback);
                if (DataManager.Instance.PlayerDataObject.MaxKnockbackMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.KnockbackMultiplier >= DataManager.Instance.PlayerDataObject.MaxKnockbackMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.knockback);
                }
                break;

            case UpgradeType.range:
                GameManager.Instance.UpgradeStat(StatModifier.range);
                if (DataManager.Instance.PlayerDataObject.MaxRangeMultiplier > 0.0f && DataManager.Instance.PlayerDataObject.RangeMultiplier >= DataManager.Instance.PlayerDataObject.MaxRangeMultiplier)
                {
                    _availableStatUpgrades.Remove(UpgradeType.range);
                }
                break;

            case UpgradeType.cooldown:
                GameManager.Instance.UpgradeStat(StatModifier.cooldown);
                if (DataManager.Instance.PlayerDataObject.MaxCooldownDivisor > 0.0f && DataManager.Instance.PlayerDataObject.CooldownDivisor >= DataManager.Instance.PlayerDataObject.MaxCooldownDivisor)
                {
                    _availableStatUpgrades.Remove(UpgradeType.cooldown);
                }
                break;

            case UpgradeType.count:
                GameManager.Instance.UpgradeStat(StatModifier.count);
                if (DataManager.Instance.PlayerDataObject.MaxCountIncreaser > 0 && DataManager.Instance.PlayerDataObject.CountIncreaser >= DataManager.Instance.PlayerDataObject.MaxCountIncreaser)
                {
                    _availableStatUpgrades.Remove(UpgradeType.count);
                }
                break;

            case UpgradeType.pierce:
                GameManager.Instance.UpgradeStat(StatModifier.pierce);
                if (DataManager.Instance.PlayerDataObject.MaxPierceIncreaser > 0 && DataManager.Instance.PlayerDataObject.PierceIncreaser >= DataManager.Instance.PlayerDataObject.MaxPierceIncreaser)
                {
                    _availableStatUpgrades.Remove(UpgradeType.pierce);
                }
                break;

            case UpgradeType.trailOfAssurace:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.trailOfAssurace);
                if (DataManager.Instance.PlayerDataObject.TrailOfAssuranceLevel >= DataManager.Instance.PlayerDataObject.TOAAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.trailOfAssurace);
                }
                break;

            case UpgradeType.shieldOfLight:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.shieldOfLight);
                if (DataManager.Instance.PlayerDataObject.ShieldOfLightLevel >= DataManager.Instance.PlayerDataObject.SOLAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.shieldOfLight);
                }
                break;

            case UpgradeType.wishingWell:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.wishingWell);
                if (DataManager.Instance.PlayerDataObject.WishingWellLevel >= DataManager.Instance.PlayerDataObject.WWAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.wishingWell);
                }
                break;

            case UpgradeType.radiantOrb:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.radiantOrb);
                if (DataManager.Instance.PlayerDataObject.RadiantOrbLevel >= DataManager.Instance.PlayerDataObject.ROAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.radiantOrb);
                }
                break;

            case UpgradeType.flickerOfHope:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.flickerOfHope);
                if (DataManager.Instance.PlayerDataObject.FlickerOfHopeLevel >= DataManager.Instance.PlayerDataObject.FlickAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.flickerOfHope);
                }
                break;

            case UpgradeType.sparkOfJoy:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.sparkOfJoy);
                if (DataManager.Instance.PlayerDataObject.SparkOfJoyLevel >= DataManager.Instance.PlayerDataObject.SparkAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.sparkOfJoy);
                }
                break;

            case UpgradeType.moonBurst:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.moonBurst);
                if (DataManager.Instance.PlayerDataObject.MoonBurstLevel >= DataManager.Instance.PlayerDataObject.MBAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.moonBurst);
                }
                break;

            case UpgradeType.floodOfHope:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.floodOfHope);
                if (DataManager.Instance.PlayerDataObject.FloodOfHopeLevel >= DataManager.Instance.PlayerDataObject.FloodAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.floodOfHope);
                }
                break;

            case UpgradeType.surgeOfJoy:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.surgeOfJoy);
                if (DataManager.Instance.PlayerDataObject.SurgeOfJoyLevel >= DataManager.Instance.PlayerDataObject.SurgeAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.surgeOfJoy);
                }
                break;

            case UpgradeType.sunBurst:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.sunBurst);
                if (DataManager.Instance.PlayerDataObject.SunBurstLevel >= DataManager.Instance.PlayerDataObject.SBAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.sunBurst);
                }
                break;

            case UpgradeType.waveOfRelief:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.waveOfRelief);
                if (DataManager.Instance.PlayerDataObject.WaveOfReliefLevel >= DataManager.Instance.PlayerDataObject.WORAttackDamage.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.waveOfRelief);
                }
                break;

            case UpgradeType.pendantOfLife:
                GameManager.Instance.UpgradeAttack(SecondaryAttack.pendantOfLife);
                if (DataManager.Instance.PlayerDataObject.PendantOfLifeLevel >= DataManager.Instance.PlayerDataObject.POLHealingSpeed.Length - 1)
                {
                    _availableStatUpgrades.Remove(UpgradeType.pendantOfLife);
                }
                break;
        }

        //finish upgrade
        HideUpgradeUI();
    }
}
