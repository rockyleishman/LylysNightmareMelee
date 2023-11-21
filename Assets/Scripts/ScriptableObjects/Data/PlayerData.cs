using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataObject", menuName = "Data/PlayerDataObject", order = 0)]
public class PlayerData : ScriptableObject
{
    internal PlayerController Player;
    internal GameObject CursorObject;
    [SerializeField] public float DefaultCursorDistance = 5.0f;

    [SerializeField] public float MaxHP = 100.0f;
    internal float CurrentHP;

    [SerializeField] public float MovementSpeed = 5.0f;

    [SerializeField] public float PillowAttackDamage = 10.0f;
    [SerializeField] public float PillowAttackKnockback = 10.0f;

    [SerializeField] public float PillowAttackDuration = 0.25f;
    [SerializeField] public float PillowAttackCooldown = 0.1f;

    internal float SpecialCharge;

    //multipliers
    internal float HPMultiplier;
    internal float MovementSpeedMultiplier;
    internal float DamageMultiplier;
    internal float KnockbackMultiplier;
}
