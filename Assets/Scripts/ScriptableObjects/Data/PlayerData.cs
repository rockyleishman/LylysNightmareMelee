using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataObject", menuName = "Data/PlayerDataObject", order = 0)]
public class PlayerData : ScriptableObject
{
    [SerializeField] public PlayerController Player;
    [SerializeField] public GameObject Cursor;

    [SerializeField] public float InitialMaxHP = 100.0f;
    internal float MaxHP;
    internal float CurrentHP;

    [SerializeField] public float InitialMovementSpeed = 5.0f;
    internal float MovementSpeed;

    [SerializeField] public float InitialPillowAttackDamage = 10.0f;
    internal float PillowAttackDamage;
    [SerializeField] public float PillowAttackActiveTime = 0.1f;

    internal float SpecialCharge;
}
